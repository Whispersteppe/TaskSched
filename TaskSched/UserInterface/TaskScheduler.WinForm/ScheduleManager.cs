﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.DataModel;
using TaskSched.Common.FieldValidator;
using TaskSched.Common.Interfaces;
using TaskSched.DataStore;
using TaskSched.ExecutionEngine;
using TaskSched.SchedulerEngine;
using TaskScheduler.WinForm.Configuration;
using TaskScheduler.WinForm.Models;

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
        ICalendarStore _calendarStore;

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
            _calendarStore = new CalendarStore(_dbContextFactory, _mapper);

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
                ActivityModel model = _managerMapper.Map<Activity, ActivityModel>(activity);
                model.ParentItem = topModel;
                //new ActivityModel(activity, topModel);
                topModel.Children.Add(model);
            }

            return topModel;
        }


        private void MapCalendarChildren(CalendarModel calendarModel, Calendar calendar)
        {
            calendarModel.ChildCalendars.Clear();
            calendarModel.Events.Clear();

            foreach(var childCalendar in calendar.ChildCalendars)
            {
                CalendarModel childModel = _managerMapper.Map<Calendar, CalendarModel>(childCalendar);
                childModel.ParentItem = calendarModel;
                calendarModel.ChildCalendars.Add(childModel);

                //  map all the children
                MapCalendarChildren(childModel, childCalendar);

            }

            foreach(var childEvent in calendar.Events)
            {
                EventModel eventModel = _managerMapper.Map<Event, EventModel>(childEvent);

                eventModel.ParentItem = calendarModel;

                calendarModel.Events.Add(eventModel);

            }
        }

        public async Task<CalendarRootModel> GetCalendarModels()
        {
            CalendarRootModel topModel = new CalendarRootModel(null);

            var calendars = await _calendarStore.GetAll(new CalendarRetrievalParameters() { AddChildEvents = true, AddChildFolders = true, AsTree = true }); 
            foreach (var calendar in calendars.Result)
            {

                if (calendar.Id == Guid.Empty)
                {
                    //  we've got the unassigned folder.  lets unravel these separately
                    //  there won't be any calendars.  those already fall at the top
                    foreach(var childEvent in calendar.Events)
                    {

                        EventModel eventModel = _managerMapper.Map<Event, EventModel>(childEvent);

                        eventModel.ParentItem = topModel;

                        topModel.Children.Add(eventModel);

                    }
                }
                else
                {
                    CalendarModel model = _managerMapper.Map<Calendar, CalendarModel>(calendar);
                    model.ParentItem = topModel;

                    topModel.Children.Add(model);

                    //  map all the children
                    MapCalendarChildren(model, calendar);

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
                await GetCalendarModels(),
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
                        ActivityModel model = new ActivityModel();
                        model.ParentItem = parentItem;

                        newItem = model;
                        break;
                    }
                case TreeItemTypeEnum.CalendarItem:
                    {
                        CalendarModel model = new CalendarModel();
                        model.ParentItem = parentItem;
                        newItem = model;
                        break;
                    }
                case TreeItemTypeEnum.EventItem:
                    {
                        EventModel model = new EventModel();
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
                            _managerMapper.Map<Activity, ActivityModel>(rsltGet.Result, activityModel);

                            item = activityModel;
                        }
                        else
                        {
                            var rslt = await _activityStore.Update(activityModel);
                            var rsltGet = await _activityStore.Get(activityModel.Id);
                            _managerMapper.Map<Activity, ActivityModel>(rsltGet.Result, activityModel);

                            item = activityModel;

                        }
                        break;
                    }
                case TreeItemTypeEnum.CalendarItem:
                    {
                        CalendarModel calendarModel = item as CalendarModel;

                        if (calendarModel.Id == Guid.Empty)
                        {
                            var rslt = await _calendarStore.Create(calendarModel);
                            var rsltGet = await _calendarStore.Get(rslt.Result);
                            _managerMapper.Map<Calendar, CalendarModel>(rsltGet.Result, calendarModel);

                            item = calendarModel;
                        }
                        else
                        {
                            var rslt = await _calendarStore.Update(calendarModel);
                            var rsltGet = await _calendarStore.Get(calendarModel.Id);
                            _managerMapper.Map<Calendar, CalendarModel>(rsltGet.Result, calendarModel);

                            item = calendarModel;

                        }
                        break;
                    }
                case TreeItemTypeEnum.EventItem:
                    {
                        EventModel eventModel = item as EventModel;

                        if (eventModel.Id == Guid.Empty)
                        {
                            var rslt = await _eventStore.Create(eventModel);
                            var rsltGet = await _eventStore.Get(rslt.Result);
                            _managerMapper.Map<Event, EventModel>(rsltGet.Result, eventModel);

                            item = eventModel;
                        }
                        else
                        {
                            var rslt = await _eventStore.Update(eventModel);
                            var rsltGet = await _eventStore.Get(eventModel.Id);
                            _managerMapper.Map<Event, EventModel>(rsltGet.Result, eventModel);

                            item = eventModel;

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

                        break;
                    }
                case TreeItemTypeEnum.CalendarItem:
                    {
                        break;
                    }
                case TreeItemTypeEnum.EventItem:
                    {
                        break;
                    }
                default:
                    {
                        return false;
                    }
            }

            return true;
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
                case TreeItemTypeEnum.CalendarItem:
                    {
                        var rslt = await _calendarStore.Delete(model.ID);
                        break;
                    }
                case TreeItemTypeEnum.EventItem:
                    {
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

    }
}
