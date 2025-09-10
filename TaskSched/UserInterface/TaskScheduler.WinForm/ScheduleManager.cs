using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dataModel=TaskSched.Common.DataModel;
using TaskSched.Common.FieldValidator;
using TaskSched.Common.Interfaces;
using TaskSched.DataStore;
using TaskSched.ExecutionEngine;
using TaskSched.SchedulerEngine;
using TaskScheduler.WinForm.Configuration;
using TaskScheduler.WinForm.Models;
using TaskSched.Common.DataModel;
using TaskScheduler.WinForm.NLogCustom;
using SQLitePCL;
using TaskSched.Common.Delegates;

namespace TaskScheduler.WinForm
{

    public class EventCancellationArgs
    {
        public bool Cancel { get; set; } = false;
        public List<string> CancelReasons { get; private set; } = [];
        public void AddReason(string reason)
        {
            CancelReasons.Add(reason);
        }
    }


    public delegate Task ITreeItemEvent(ITreeItem treeItem);
    public delegate Task ITreeItemParentEvent(ITreeItem? parentItem, ITreeItem childItem);
    public delegate void ITreeItemEventCheck(ITreeItem treeItem, EventCancellationArgs cancelArgs);
    public delegate Task ItemEvent(Guid itemId);


    /// <summary>
    /// primary manager for scheduling and execution of activities
    /// </summary>
    public class ScheduleManager : IDisposable
    {

        readonly ScheduleManagerConfig _config;
        //database information
        readonly TaskSchedDbContextFactory _dbContextFactory;
        public static ScheduleManager? GlobalInstance { get; private set; }

        readonly ManagerMapper _managerMapper;

        // all of the data stores and mappers
        readonly IDataStoreMapper _mapper;
        readonly IExecutionStore _executionStore;
        readonly IActivityStore _activityStore;
        readonly IEventStore _eventStore;
        readonly IFolderStore _folderStore;

        readonly IFieldValidatorSet _fieldValidators;

        readonly ILoggerFactory _loggerFactory;
        readonly ILogger _logger;
        readonly ILogEmitter _logEmitter;

        readonly IExecutionEngine _executionEngine;
        readonly ISchedulerEngine _schedulerEngine;

        #region Events

        public event ITreeItemParentEvent? OnTreeItemCreated;
        public event ITreeItemEvent? OnTreeItemRemoved;
        public event ITreeItemEvent? OnTreeItemUpdated;
        public event ITreeItemEvent? OnTreeItemSelected;
        public event ITreeItemEventCheck? OnTreeItemRemoving;
        public event ItemEvent? OnItemSelected;


        public event EventAction OnStartEvent;
        public event EventAction OnFinishEvent;
        public event ActivityAction OnStartActivity;
        public event ActivityAction OnFinishActivity;

        #endregion


        private bool disposedValue;

        public ExecutionStatusEnum ExecutionStatus { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbConfig"></param>
        /// <param name="loggerFactory"></param>
        public ScheduleManager(
            ScheduleManagerConfig configuration, 
            ILoggerFactory loggerFactory,
            ILogEmitter logEmitter) 
        {
            GlobalInstance = this;

            _config = configuration;
            _loggerFactory = loggerFactory;
            _logEmitter = logEmitter;
            _logger = loggerFactory.CreateLogger<ScheduleManager>();
            _managerMapper = new ManagerMapper();

            ExecutionStatus = ExecutionStatusEnum.Stopped;

            _dbContextFactory = new TaskSchedDbContextFactory(_config.DatabaseConfig);

            _mapper = new DataStoreMapper();
            _fieldValidators = new FieldValidatorSet();

            _eventStore = new EventStore(_dbContextFactory, _mapper, _fieldValidators, loggerFactory.CreateLogger<EventStore>());
            _activityStore = new ActivityStore(_dbContextFactory, _mapper, _fieldValidators, loggerFactory.CreateLogger<ActivityStore>()) ;
            _folderStore = new FolderStore(_dbContextFactory, _mapper, loggerFactory.CreateLogger<FolderStore>());

            _executionStore = new TaskSched.ExecutionStore.ExecutionStore(_loggerFactory.CreateLogger<TaskSched.ExecutionStore.ExecutionStore>());

            _executionEngine = new ActivityEngine(_loggerFactory.CreateLogger<ActivityEngine>(), _executionStore);
            _executionEngine.OnStartActivity += _executionEngine_OnStartActivity;
            _executionEngine.OnFinishActivity += _executionEngine_OnFinishActivity;

            _schedulerEngine = new SchedulerEngine(_executionEngine, _eventStore, _activityStore, _loggerFactory.CreateLogger<SchedulerEngine>());
            _schedulerEngine.OnStartEvent += _schedulerEngine_OnStartEvent;
            _schedulerEngine.OnFinishEvent += _schedulerEngine_OnFinishEvent;
        }

        private void _executionEngine_OnFinishActivity(ActivityContext context)
        {
            OnFinishActivity?.Invoke(context); 
        }

        private void _executionEngine_OnStartActivity(ActivityContext context)
        {
            OnStartActivity?.Invoke(context);
        }

        private void _schedulerEngine_OnFinishEvent(Event context)
        {
            OnFinishEvent?.Invoke(context);
        }

        private void _schedulerEngine_OnStartEvent(Event context)
        {
            OnStartEvent?.Invoke(context);
        }

        public async Task Start()
        {
            if (ExecutionStatus == ExecutionStatusEnum.Stopped)
            {
                _logger.LogInformation($"Starting the manager");

                ExecutionStatus = ExecutionStatusEnum.Starting;

                using (var _dbContext = _dbContextFactory.GetConnection())
                {
                    await _dbContext.EnsureCreated();
                    await _executionEngine.Start();
                    await _schedulerEngine.Start();

                    ExecutionStatus = ExecutionStatusEnum.Running;
                }

                _logger.LogInformation($"Manager started");

            }

        }

        public async Task Stop()
        {
            if (ExecutionStatus == ExecutionStatusEnum.Running)
            {
                _logger.LogInformation($"Stopping the manager");

                ExecutionStatus = ExecutionStatusEnum.Stopping;

                await _schedulerEngine.Stop();
                await _executionEngine.Stop();

                using (var _dbContext = _dbContextFactory.GetConnection())
                {
                    string backupFileName = String.Concat(_config.DatabaseConfig.DataSource, ".backup");

                    await _dbContext.BackupDatabase(backupFileName);
                    await _dbContext.CompactDatabase();
                }

                ExecutionStatus = ExecutionStatusEnum.Stopped;

                _logger.LogInformation($"Manager stopped");



            }

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Stop().GetAwaiter().GetResult();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~EngineManager()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #region Get Treeview models

        public async Task<List<ActivityModel>> GetActivities()
        {
            var getActivitiesResult = await _activityStore.GetAll();

            List<ActivityModel> results = [];

            foreach (var activity in getActivitiesResult.Result)
            {
                ActivityModel model = _managerMapper.Map<dataModel.Activity, ActivityModel>(activity);
                results.Add(model);
            }

            return results;
        }

        public async Task<ActivityRootModel> GetActivityModels()
        {

            ActivityRootModel topModel = new ActivityRootModel(null);

            var getActivitiesResult = await _activityStore.GetAll();

            foreach(var activity in getActivitiesResult.Result)
            {
                ActivityModel model = _managerMapper.Map<dataModel.Activity, ActivityModel>(activity);
                topModel.Children.Add(model);
            }

            return topModel;
        }

        public async Task<List<ScheduleStatusItem>> GetScheduleItems(bool showActive)
        {
            var eventsRslt = await _eventStore.GetAll();
            var scheduleItems = new List<ScheduleStatusItem>();
            foreach(var eventItem in eventsRslt.Result)
            {
                if (eventItem.IsActive == showActive)
                {
                    scheduleItems.Add(new ScheduleStatusItem()
                    {
                        IsActive = eventItem.IsActive,
                        Name = eventItem.Name,
                        NextExecution = eventItem.NextExecution,
                        PreviousExecution = eventItem.LastExecution,
                        ID = eventItem.Id,
                    });
                }
            }

            return scheduleItems;
        }

        //private void MapFolderChildren(FolderModel folderModel, dataModel.Folder folder)
        //{
        //    folderModel?.ChildFolders?.Clear();
        //    folderModel?.Events?.Clear();
        //    if (folder.ChildFolders != null)
        //    {
        //        foreach (var childFolder in folder.ChildFolders)
        //        {
        //            FolderModel childModel = _managerMapper.Map<dataModel.Folder, FolderModel>(childFolder);
        //            folderModel?.ChildFolders?.Add(childModel);

        //            //  map all the children
        //            MapFolderChildren(childModel, childFolder);

        //        }
        //    }

        //    if (folder.Events != null)
        //    {
        //        foreach (var childEvent in folder.Events)
        //        {
        //            EventModel eventModel = _managerMapper.Map<dataModel.Event, EventModel>(childEvent);

        //            folderModel?.Events?.Add(eventModel);

        //        }
        //    }
        //}

        public async Task<FolderRootModel> GetFolderModels()
        {
            FolderRootModel topModel = new FolderRootModel(null);

            var folders = await _folderStore.GetAll(new FolderRetrievalParameters() { AddChildEvents = true, AddChildFolders = true, AsTree = true }); 
            foreach (var folder in folders.Result)
            {

                if (folder.Id == Guid.Empty)
                {
                    //  we've got the unassigned folder.  lets unravel these separately
                    //  there won't be any folders.  those already fall at the top
                    if (folder.Events != null)
                    {
                        foreach (var childEvent in folder.Events)
                        {

                            EventModel eventModel = _managerMapper.Map<dataModel.Event, EventModel>(childEvent);

                            topModel.Children.Add(eventModel);

                        }
                    }
                }
                else
                {
                    FolderModel model = _managerMapper.Map<dataModel.Folder, FolderModel>(folder);

                    topModel.Children.Add(model);

                    //  map all the children
                    //MapFolderChildren(model, folder);

                }

            }

            return topModel;
            
        }


        public async Task<AboutModel> GetAboutModels()
        {
            await Task.Run(() => { });

            AboutModel topModel = new AboutModel()
            {
            };

            return topModel;
        }

        public async Task<ConfigurationRootModel> GetConfigModels()
        {
            await Task.Run(() => { });

            ConfigurationRootModel topModel = new ConfigurationRootModel(null)
            {
                ScheduleManagerConfiguration = _config
            };

            //ConfigurationModel config = new ConfigurationModel()
            //{
            //    ScheduleManagerConfiguration = _config
            //};


            //topModel.Children.Add(config);

            return topModel;
        }

        public async Task<StatusRootModel> GetStatusModels()
        {
            await Task.Run(() => { });

            StatusRootModel topModel = new StatusRootModel(null);

            SchedulerStatusModel activeStatus = new SchedulerStatusModel(this, "Active Items", true);
            SchedulerStatusModel inactiveStatus = new SchedulerStatusModel(this, "Inactive Items", false);
            ExecutionEngineStatusModel executionStatus = new ExecutionEngineStatusModel();

            topModel.Children.Add(activeStatus);
            topModel.Children.Add(inactiveStatus);
            topModel.Children.Add(executionStatus);

            return topModel;
        }

        public async Task<LogRootModel> GetLogModels()
        {
            await Task.Run(() => { });

            LogRootModel topModel = new LogRootModel(null);

            LogViewModelConfig _configError = new LogViewModelConfig { MaxLogCount = 200, MinLogLevel = NLog.LogLevel.Error, Name = "Error Log" };
            LogViewModel logviewError = new LogViewModel(_configError, _logEmitter);
            topModel.Children.Add(logviewError);

            LogViewModelConfig _configWarning = new LogViewModelConfig { MaxLogCount = 200, MinLogLevel = NLog.LogLevel.Warn, Name = "Warning Log" };
            LogViewModel logviewWarning = new LogViewModel(_configWarning, _logEmitter);
            topModel.Children.Add(logviewWarning);


            LogViewModelConfig _configScheduler = new LogViewModelConfig 
            { 
                MaxLogCount = 200, 
                MinLogLevel = NLog.LogLevel.Trace, 
                Name = "Scheduler Log", 
                AllowedLoggerNames = ["Quartz", "TaskSched.SchedulerEngine"], 
                DeniedMessageText = ["Batch acquisition of 0 triggers"]
            };
            LogViewModel logviewScheduler = new LogViewModel(_configScheduler, _logEmitter);
            topModel.Children.Add(logviewScheduler);

            LogViewModelConfig _configExecution = new LogViewModelConfig
            {
                MaxLogCount = 200,
                MinLogLevel = NLog.LogLevel.Trace,
                Name = "Execution Log",
                AllowedLoggerNames = ["TaskSched.ExecutionEngine"]
            };
            LogViewModel logviewExecution = new LogViewModel(_configExecution, _logEmitter);
            topModel.Children.Add(logviewExecution);

            LogViewModelConfig _datastoreConfig = new LogViewModelConfig
            {
                MaxLogCount = 200,
                MinLogLevel = NLog.LogLevel.Trace,
                Name = "Datastore Log",
                AllowedLoggerNames = ["TaskSched.DataStore"]
            };
            LogViewModel datastoreScheduler = new LogViewModel(_datastoreConfig, _logEmitter);
            topModel.Children.Add(datastoreScheduler);

            LogViewModelConfig _uiConfig = new LogViewModelConfig
            {
                MaxLogCount = 200,
                MinLogLevel = NLog.LogLevel.Trace,
                Name = "UI Log",
                AllowedLoggerNames = ["TaskScheduler.WinForm"]
            };
            LogViewModel logviewUI = new LogViewModel(_uiConfig, _logEmitter);
            topModel.Children.Add(logviewUI);

            LogViewModelConfig _configTrace = new LogViewModelConfig
            {
                MaxLogCount = 200,
                MinLogLevel = NLog.LogLevel.Trace,
                Name = "Miscellaneous Log",
                DeniedLoggerNames = [
                    "Quartz",
                    "TaskSched.DataStore",
                    "TaskSched.ExecutionEngine",
                    "TaskSched.SchedulerEngine",
                    "TaskScheduler.WinForm"
                    ]
            };
            LogViewModel logviewTrace = new LogViewModel(_configTrace, _logEmitter);
            topModel.Children.Add(logviewTrace);

            LogViewModelConfig _allConfig = new LogViewModelConfig
            {
                MaxLogCount = 200,
                MinLogLevel = NLog.LogLevel.Trace,
                Name = "All Logs"
            };
            LogViewModel logviewAll = new LogViewModel(_allConfig, _logEmitter);
            topModel.Children.Add(logviewAll);


            return topModel;
        }

        public async Task<List<ExecutionHandlerInfo>> GetExecutionHandlerInfo()
        {
            var handlers = await _executionStore.GetHandlerInfo();

            return handlers;
        }

        public async Task<List<ITreeItem>> GetAllRoots()
        {
            List<ITreeItem> rootItems = new List<ITreeItem>()
            {
                await GetFolderModels(),
                await GetActivityModels(),
                await GetStatusModels(),
                await GetLogModels(),
                await GetConfigModels(),
                await GetAboutModels(),
            };

            return rootItems;
        }

        #endregion

        public async Task SelectTreeViewItem(ITreeItem treeItem)
        {


            if (OnTreeItemSelected != null)
            {
                await OnTreeItemSelected(treeItem);
            }
        }


        public async Task SelectItem(Guid itemId)
        {

            if (OnItemSelected != null)
            {
                await OnItemSelected(itemId);
            }
        }


        #region data manipulation items

        public async Task<ITreeItem> RefreshModel(ITreeItem treeItem)
        {


            ITreeItem refreshedItem = treeItem;

            if (treeItem.ID == null) return treeItem; //  no id - maybe a root
            if (treeItem.ID == Guid.Empty) return treeItem; //  no id - maybe a root
            switch(treeItem.TreeItemType)
            {
                case (TreeItemTypeEnum.EventItem):
                    {
                        var rsltGet = await _eventStore.Get(treeItem.ID.Value);
                        refreshedItem = _managerMapper.Map<EventModel>(rsltGet.Result);
                        break;
                    }
                case (TreeItemTypeEnum.ActivityItem):
                    {
                        var rsltGet = await _activityStore.Get(treeItem.ID.Value);
                        refreshedItem = _managerMapper.Map<ActivityModel>(rsltGet.Result);
                        break;
                    }
                case (TreeItemTypeEnum.FolderItem):
                    {
                        var rsltGet = await _folderStore.Get(treeItem.ID.Value);
                        refreshedItem = _managerMapper.Map<FolderModel>(rsltGet.Result);
                        break;
                    }
                default:
                    break;
            }

            return refreshedItem;
        }

        public async Task<ITreeItem> CreateModel(ITreeItem? parentItem, TreeItemTypeEnum modelType)
        {

            ITreeItem newItem = null;

            switch (modelType)
            {
                case TreeItemTypeEnum.ActivityItem:
                    {
                        var handlerInfo = await _executionStore.GetHandlerInfo();

                        Activity activity = new Activity()
                        {
                            Name = "New Activity",
                            ActivityHandlerId = handlerInfo[0].HandlerId,
                            DefaultFields = new List<ActivityField>(
                                handlerInfo[0].RequiredFields.Select(x => new ActivityField()
                                {
                                    FieldType = x.FieldType,
                                    Value = "new value",
                                    Name = x.Name,
                                    IsReadOnly = true
                                })
                                )

                        };
                        var rsltCreate = await _activityStore.Create(activity);
                        var rsltGet = await _activityStore.Get(rsltCreate.Result);
                        var model = _managerMapper.Map<ActivityModel>(rsltGet.Result);

                        newItem = model;
                        break;
                    }
                case TreeItemTypeEnum.FolderItem:
                    {

                        Folder folder = new Folder()
                        {
                            Name = "New Folder",
                            ParentFolderId = parentItem != null && parentItem is RootModel == false ? parentItem.ID : null,
                        };

                        var rsltCreate = await _folderStore.Create(folder);
                        var rsltGet = await _folderStore.Get(rsltCreate.Result);

                        var model = _managerMapper.Map<FolderModel>(rsltGet.Result);
                        newItem = model;
                        break;
                    }
                case TreeItemTypeEnum.EventItem:
                    {
                        //  get the default activity

                        var rsltActivity = await _activityStore.GetDefault();
                        var defaultActivity = rsltActivity.Result;
                        string defaultSchedule = $"0 {DateTime.Now.Minute} {DateTime.Now.Hour} ? * {DateTime.Now.ToString("ddd").ToUpper()} *";

                        if (parentItem != null && parentItem is FolderModel folder)
                        {
                            if (string.IsNullOrEmpty(folder.DefaultSchedule) == false)
                            {
                                defaultSchedule = folder.DefaultSchedule;
                            }
                        }

                        Event eventItem = new Event()
                        {
                            CatchUpOnStartup = true, 
                            Name = "New Item", 
                            IsActive = true,
                            FolderId = parentItem != null && parentItem is RootModel == false ? parentItem.ID : null,
                            LastExecution = DateTime.Now, 
                            NextExecution = DateTime.Now, 
                            Activities = new List<EventActivity>()
                            {
                                 new EventActivity()
                                 {
                                     Name = "New Activity",
                                     ActivityId = defaultActivity.Id, 
                                     Fields = new List<EventActivityField>(
                                         defaultActivity.DefaultFields.Where(x=>x.IsReadOnly == false).Select(x => 
                                         new EventActivityField()
                                         { 
                                             ActivityFieldId = x.Id, 
                                             Name = x.Name, 
                                             Value = x.Value,
                                             FieldType = x.FieldType
                                         }))
                                 }
                            },
                            Schedules = new List<EventSchedule>()
                            {
                                new EventSchedule()
                                {
                                     Name = "Default Schedule",
                                      CRONData = defaultSchedule
                                }
                            }
                        };

                        //  see if I can use the current clipboard contents for the activity
                        //TODO - we'll want to use tags once ive defined that process.  but until then, we'll hardwire this one in.
                        if (Clipboard.ContainsText() == true)
                        {
                            string clipboardText = Clipboard.GetText();
                            Uri? uri;
                            if (Uri.TryCreate(clipboardText, new UriCreationOptions() { }, out uri) == true)
                            {
                                var urlField = eventItem.Activities[0].Fields.FirstOrDefault(x=>x.Name.Equals("url", StringComparison.InvariantCultureIgnoreCase));
                                if (urlField != null)
                                {
                                    urlField.Value = clipboardText;
                                }
                            }
                        }

                        var rsltCreate = await _eventStore.Create(eventItem);
                        var rsltGet = await _eventStore.Get(rsltCreate.Result);


                        EventModel model = _managerMapper.Map<EventModel>(rsltGet.Result);

                        newItem = model;
                        break;
                    }
                default:
                    {
                        return null;
                    }
            }


            if (OnTreeItemCreated != null)
            {
                await OnTreeItemCreated(parentItem, newItem);
            }

            return newItem;

        }

        public async Task MoveItem(ITreeItem landingItem, ITreeItem movingItem)
        {
            switch (movingItem.TreeItemType)
            {
                case TreeItemTypeEnum.ActivityItem:
                    {
                        if (movingItem is ActivityModel activityModel)
                       {

                            //  currently there isn't any movement of activities
                        }

                        break;
                    }
                case TreeItemTypeEnum.FolderItem:
                    {
                        if (movingItem is FolderModel folderModel)
                        {
                            folderModel.ParentFolderId = landingItem.ID;
                            Folder folder = _managerMapper.Map<dataModel.Folder>(folderModel);

                            await _folderStore.Update(folder);
                        }

                        break;
                    }
                case TreeItemTypeEnum.EventItem:
                    {
                        if (movingItem is EventModel eventModel)
                        {
                            eventModel.FolderId = landingItem.ID;
                            Event eventItem = _managerMapper.Map<dataModel.Event>(eventModel);

                            await _eventStore.Update(eventItem);
                        }


                        break;
                    }
                default:
                    {
                        return;
                    }
            }
        }

        public async Task<ITreeItem> SaveModel(ITreeItem item)
        {

            switch (item.TreeItemType)
            {
                case TreeItemTypeEnum.ActivityItem:
                    {
                        ActivityModel activityModel = item as ActivityModel;
                        Activity activity = _managerMapper.Map<Activity>(activityModel);

                        //  make sure we have all the necessary fields from the execution handler
                        var handler = await _executionStore.GetExecutionHandler(activity.ActivityHandlerId);
                        if (handler != null)
                        {
                            foreach (var field in handler.HandlerInfo.RequiredFields)
                            {
                                if (activity.DefaultFields.Any(x => x.Name == field.Name) == false)
                                {
                                    var newField = new ActivityField() { Name = field.Name, FieldType = field.FieldType, IsReadOnly = true, Value = field.Value };
                                    activity.DefaultFields.Add(newField);
                                }
                            }
                        }

                        if (activityModel.Id == Guid.Empty)
                        {
                            var rslt = await _activityStore.Create(activity);
                            activityModel.Id = rslt.Result;
                        }
                        else
                        {
                            var rslt = await _activityStore.Update(activity);

                        }
                        var rsltGet = await _activityStore.Get(activityModel.Id);
                        _managerMapper.Map<dataModel.Activity, ActivityModel>(rsltGet.Result, activityModel);

                        item = activityModel;
                        break;
                    }
                case TreeItemTypeEnum.FolderItem:
                    {
                        FolderModel folderModel = item as FolderModel;

                        if (folderModel.Id == Guid.Empty)
                        {
                            Folder folder = _managerMapper.Map<Folder>(folderModel);
                            var rslt = await _folderStore.Create(folder);
                            var rsltGet = await _folderStore.Get(rslt.Result);
                            _managerMapper.Map<dataModel.Folder, FolderModel>(rsltGet.Result, folderModel);

                            item = folderModel;
                        }
                        else
                        {
                            Folder folder = _managerMapper.Map<Folder>(folderModel);
                            var rslt = await _folderStore.Update(folder);
                            var rsltGet = await _folderStore.Get(folderModel.Id);
                            _managerMapper.Map<dataModel.Folder, FolderModel>(rsltGet.Result, folderModel);

                            item = folderModel;

                        }
                        break;
                    }
                case TreeItemTypeEnum.EventItem:
                    {
                        EventModel eventModel = item as EventModel;
                        Event eventItem;
                        Guid currentId;

                        //  make sure we've got activities
                        Event evt = _managerMapper.Map<Event>(eventModel);
                        if (evt.Activities == null)
                        {
                            evt.Activities = new List<EventActivity>();
                        }
                        if (evt.Activities.Count == 0)
                        {
                            var bestActivity = await _activityStore.GetDefault();
                            EventActivity newActivity = new EventActivity()
                            {
                                Fields = new List<EventActivityField>(),
                                Name = "new activity",
                                ActivityId = bestActivity.Result.Id
                            };
                            evt.Activities.Add(newActivity);
                        }
                        //  check the fields on the activities
                        foreach(var activityItem in evt.Activities)
                        {
                            var activityRslt = await _activityStore.Get(activityItem.ActivityId);
                            var activity = activityRslt.Result;

                            foreach (var field in activity.DefaultFields)
                            {
                                if (field.IsReadOnly == false && activityItem.Fields.Any(x => x.ActivityFieldId == field.Id) == false)
                                {
                                    if (activityItem.Fields.Any(x => x.Name == field.Name) == false)
                                    {
                                        EventActivityField newField = new EventActivityField()
                                        {
                                            ActivityFieldId = field.Id,
                                            FieldType = field.FieldType,
                                            Name = field.Name,
                                            Value = field.Value
                                        };
                                        activityItem.Fields.Add(newField);
                                    }
                                }
                            }
                        }

                        //  check schedules
                        if (evt.Schedules == null)
                        {
                            evt.Schedules = new List<EventSchedule>();
                        }
                        if (evt.Schedules.Count == 0)
                        {
                            EventSchedule newSchedle = new EventSchedule()
                            {
                                Name = "New Schedule",
                                CRONData = "0 0 8 * * ?"
                            };
                            evt.Schedules.Add(newSchedle);
                        }


                        if (eventModel.Id == Guid.Empty)
                        {
                            var rslt = await _eventStore.Create(evt);
                            currentId = rslt.Result;
                        }
                        else
                        {
                            var rslt = await _eventStore.Update(evt);
                            currentId = eventModel.Id;
                        }

                        var rsltGet = await _eventStore.Get(currentId);
                        if (rsltGet.Result != null)
                        {
                            _managerMapper.Map<dataModel.Event, EventModel>(rsltGet.Result, eventModel);
                            eventItem = rsltGet.Result;

                            item = eventModel;
                            await _schedulerEngine.UpdateEvent(eventItem);

                        }
                        break;
                    }
                default:
                    {
                        return null;
                    }
            }


            if (OnTreeItemCreated != null)
            {
                await OnTreeItemUpdated(item);
            }

            return item;

        }

        public async Task<bool> CanDeleteItem(ITreeItem model)
        {
            switch (model.TreeItemType)
            {
                case TreeItemTypeEnum.ActivityItem:
                    {
                        ActivityModel activityModel  = model as ActivityModel;
                        var events = await _eventStore.GetAll();
                        foreach(var eventItem in events.Result)
                        {
                            //  if in use by any activity, fail it.
                            foreach(var activity in eventItem.Activities)
                            {
                                if (activityModel.Id == activity.Id)
                                {
                                    return false;
                                }
                            }
                        }

                        return true;
                    }
                case TreeItemTypeEnum.FolderItem:
                    {
                        FolderModel folderModel = model as FolderModel;
                        if (folderModel.Events.Count == 0 && folderModel.ChildFolders.Count == 0)
                        {
                            return true;
                        }

                        break;
                    }
                case TreeItemTypeEnum.EventItem:
                    {
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }

            return false;
        }

        public async Task DeleteItem(ITreeItem model)
        {
            EventCancellationArgs args = new EventCancellationArgs();


            OnTreeItemRemoving?.Invoke(model, args);
            if (args.Cancel == true)
            {
                foreach (var item in args.CancelReasons)
                {
                    _logger.LogInformation($"Delete cancelled {model.DisplayName} - {item}");
                }
                return;
            }
            

            switch (model.TreeItemType)
            {
                case TreeItemTypeEnum.ActivityItem:
                    {
                        var rslt = await _activityStore.Delete(model.ID.GetValueOrDefault());

                        break;
                    }
                case TreeItemTypeEnum.FolderItem:
                    {
                        var rslt = await _folderStore.Delete(model.ID.GetValueOrDefault());
                        break;
                    }
                case TreeItemTypeEnum.EventItem:
                    {
                        await _schedulerEngine.DeleteEvent(model.ID.GetValueOrDefault());
                        var rslt = await _eventStore.Delete(model.ID.GetValueOrDefault());
                        break;
                    }
                default:
                    {
                        _logger.LogInformation($"Delete cancelled {model.DisplayName} - type is not recognized");
                        return;
                    }
            }


            if (OnTreeItemRemoved != null)
            {
                await OnTreeItemRemoved(model);
            }

        }

        #endregion

        public async Task<List<ExecutionHandlerInfo>> GetHandlerInfo()
        {
            var infoItems = await _executionStore.GetHandlerInfo();

            return infoItems;
        }

        public async Task LaunchEvent(EventModel eventModel)
        {
            var rslt = await _eventStore.Get(eventModel.Id);
            var eventItem = rslt.Result;

            await _schedulerEngine.ExecuteNow(eventItem);

        }

    }
}
