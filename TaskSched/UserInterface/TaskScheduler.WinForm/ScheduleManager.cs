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

namespace TaskScheduler.WinForm
{

    public delegate Task ITreeItemEvent(ITreeItem treeItem);
    public delegate Task ITreeItemParentEvent(ITreeItem? parentItem, ITreeItem childItem);


    /// <summary>
    /// primary manager for scheduling and execution of activities
    /// </summary>
    public class ScheduleManager : IDisposable
    {

        ScheduleManagerConfig _config;
        //database information
        TaskSchedDbContextFactory _dbContextFactory;

        ManagerMapper _managerMapper;

        // all of the data stores and mappers
        IDataStoreMapper _mapper;
        IExecutionStore _executionStore;
        IActivityStore _activityStore;
        IEventStore _eventStore;
        IFolderStore _folderStore;

        IFieldValidatorSet _fieldValidators;

        ILoggerFactory _loggerFactory;
        ILogger _logger;

        IExecutionEngine _executionEngine;
        ISchedulerEngine _schedulerEngine;

        #region Events

        public event ITreeItemParentEvent? OnTreeItemCreated;
        public event ITreeItemEvent? OnTreeItemRemoved;
        public event ITreeItemEvent? OnTreeItemUpdated;
        public event ITreeItemEvent? OnTreeItemSelected;

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
            ILoggerFactory loggerFactory) 
        {
            _config = configuration;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<ScheduleManager>();
            _managerMapper = new ManagerMapper();

            ExecutionStatus = ExecutionStatusEnum.Stopped;

            _dbContextFactory = new TaskSchedDbContextFactory(_config.DatabaseConfig);

            _mapper = new DataStoreMapper();
            _fieldValidators = new FieldValidatorSet();


            _eventStore = new EventStore(_dbContextFactory, _mapper, _fieldValidators);
            _activityStore = new ActivityStore(_dbContextFactory, _mapper, _fieldValidators);
            _folderStore = new FolderStore(_dbContextFactory, _mapper);

            _executionStore = new TaskSched.ExecutionStore.ExecutionStore(_loggerFactory.CreateLogger<TaskSched.ExecutionStore.ExecutionStore>());

            _executionEngine = new ActivityEngine(_loggerFactory.CreateLogger<ActivityEngine>(), _executionStore);

            _schedulerEngine = new SchedulerEngine(_executionEngine, _eventStore, _activityStore, _loggerFactory.CreateLogger<SchedulerEngine>());


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

        public async Task<ActivityRootModel> GetActivityModels()
        {

            ActivityRootModel topModel = new ActivityRootModel(null);

            var getActivitiesResult = await _activityStore.GetAll();

            foreach(var activity in getActivitiesResult.Result)
            {
                ActivityModel model = _managerMapper.Map<dataModel.Activity, ActivityModel>(activity);
                model.ParentItem = topModel;
                //new ActivityModel(activity, topModel);
                topModel.Children.Add(model);
            }

            return topModel;
        }


        private void MapFolderChildren(FolderModel folderModel, dataModel.Folder folder)
        {
            folderModel.ChildFolders.Clear();
            folderModel.Events.Clear();

            foreach(var childFolder in folder.ChildFolders)
            {
                FolderModel childModel = _managerMapper.Map<dataModel.Folder, FolderModel>(childFolder);
                childModel.ParentItem = folderModel;
                folderModel.ChildFolders.Add(childModel);

                //  map all the children
                MapFolderChildren(childModel, childFolder);

            }

            foreach(var childEvent in folder.Events)
            {
                EventModel eventModel = _managerMapper.Map<dataModel.Event, EventModel>(childEvent);

                eventModel.ParentItem = folderModel;

                folderModel.Events.Add(eventModel);

            }
        }

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
                    foreach (var childEvent in folder.Events)
                    {

                        EventModel eventModel = _managerMapper.Map<dataModel.Event, EventModel>(childEvent);

                        eventModel.ParentItem = topModel;

                        topModel.Children.Add(eventModel);

                    }
                }
                else
                {
                    FolderModel model = _managerMapper.Map<dataModel.Folder, FolderModel>(folder);
                    model.ParentItem = topModel;

                    topModel.Children.Add(model);

                    //  map all the children
                    MapFolderChildren(model, folder);

                }

            }

            return topModel;
            
        }

        public async Task<StatusRootModel> GetStatusModels()
        {
            StatusRootModel topModel = new StatusRootModel(null);



            return topModel;
        }

        public async Task<LogRootModel> GetLogModels()
        {
            LogRootModel topModel = new LogRootModel(null);



            return topModel;
        }

        public async Task<List<ITreeItem>> GetAllRoots()
        {
            List<ITreeItem> rootItems = new List<ITreeItem>()
            {
                await GetFolderModels(),
                await GetActivityModels(),
                await GetStatusModels(),
                await GetLogModels()
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




        #region data manipulation items


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
                        model.ParentItem = parentItem;
                        newItem = model;
                        break;
                    }
                case TreeItemTypeEnum.EventItem:
                    {
                        //  get the default activity

                        var rsltActivity = await _activityStore.GetDefault();
                        var defaultActivity = rsltActivity.Result;
                        Event eventItem = new Event()
                        {
                            CatchUpOnStartup = false, 
                            Name = "New Item", 
                            IsActive = false,
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
                                             FieldType = x.FieldType}))
                                 }
                            },
                            Schedules = new List<EventSchedule>()
                            {
                                new EventSchedule()
                                {
                                     Name = "8 am each day",
                                      CRONData = "0 0 8 * * ? *"
                                }
                            }
                        };

                        var rsltCreate = await _eventStore.Create(eventItem);
                        var rsltGet = await _eventStore.Get(rsltCreate.Result);


                        EventModel model = _managerMapper.Map<EventModel>(rsltGet.Result);
                        model.ParentItem = parentItem;

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

        public async Task<ITreeItem> SaveModel(ITreeItem? parentTreeItem, ITreeItem item)
        {

            switch (item.TreeItemType)
            {
                case TreeItemTypeEnum.ActivityItem:
                    {
                        ActivityModel activityModel = item as ActivityModel;

                        if (activityModel.Id == Guid.Empty)
                        {
                            var rslt = await _activityStore.Create(activityModel);
                            var rsltGet = await _activityStore.Get(rslt.Result);
                            _managerMapper.Map<dataModel.Activity, ActivityModel>(rsltGet.Result, activityModel);

                            item = activityModel;
                        }
                        else
                        {
                            var rslt = await _activityStore.Update(activityModel);
                            var rsltGet = await _activityStore.Get(activityModel.Id);
                            _managerMapper.Map<dataModel.Activity, ActivityModel>(rsltGet.Result, activityModel);

                            item = activityModel;

                        }
                        break;
                    }
                case TreeItemTypeEnum.FolderItem:
                    {
                        FolderModel folderModel = item as FolderModel;

                        if (folderModel.Id == Guid.Empty)
                        {
                            var rslt = await _folderStore.Create(folderModel);
                            var rsltGet = await _folderStore.Get(rslt.Result);
                            _managerMapper.Map<dataModel.Folder, FolderModel>(rsltGet.Result, folderModel);

                            item = folderModel;
                        }
                        else
                        {
                            var rslt = await _folderStore.Update(folderModel);
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

                        if (eventModel.Id == Guid.Empty)
                        {
                            var rslt = await _eventStore.Create(eventModel);
                            currentId = rslt.Result;
                        }
                        else
                        {
                            var rslt = await _eventStore.Update(eventModel);
                            currentId = eventModel.Id;
                        }

                        var rsltGet = await _eventStore.Get(currentId);
                        _managerMapper.Map<dataModel.Event, EventModel>(rsltGet.Result, eventModel);
                        eventItem = rsltGet.Result;

                        item = eventModel;
                        await _schedulerEngine.UpdateEvent(eventItem);

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

            switch (model.TreeItemType)
            {
                case TreeItemTypeEnum.ActivityItem:
                    {
                        var rslt = await _activityStore.Delete(model.ID);

                        break;
                    }
                case TreeItemTypeEnum.FolderItem:
                    {
                        var rslt = await _folderStore.Delete(model.ID);
                        break;
                    }
                case TreeItemTypeEnum.EventItem:
                    {
                        await _schedulerEngine.DeleteEvent(model.ID);
                        var rslt = await _eventStore.Delete(model.ID);
                        break;
                    }
                default:
                    {
                        return;
                    }
            }


            if (OnTreeItemCreated != null)
            {
                await OnTreeItemRemoved(model);
            }

            return;
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

            foreach (var eventActivity in eventItem.Activities)
            {
                var rsltActivity = await _activityStore.Get(eventActivity.ActivityId);

                dataModel.ActivityContext activityContext = new dataModel.ActivityContext()
                {
                    Activity = rsltActivity.Result,
                    EventActivity = eventActivity,
                    EventItem = eventItem
                };

                await _executionEngine.DoActivity(activityContext);
            }
        }

    }
}
