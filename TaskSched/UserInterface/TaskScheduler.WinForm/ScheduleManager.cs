using AutoMapper;
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
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm
{
    /// <summary>
    /// primary manager for scheduling and execution of activities
    /// </summary>
    public class ScheduleManager : IDisposable
    {

        ScheduleManagerConfig _config;
        //database information
        TaskSchedDbContextFactory _dbContextFactory;

        // all of the data stores and mappers
        IDataStoreMapper _mapper;
        IExecutionStore _executionStore;
        IActivityStore _activityStore;
        IEventStore _eventStore;
        ICalendarStore _calendarStore;

        IFieldValidatorSet _fieldValidators;

        ILoggerFactory _loggerFactory;

        IExecutionEngine _executionEngine;
        ISchedulerEngine _schedulerEngine;

        private bool disposedValue;

        public bool IsRunning { get; private set; }

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
            IsRunning = false;

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
            if (IsRunning == false)
            {
                using (var _dbContext = _dbContextFactory.GetConnection())
                {
                    await _dbContext.EnsureCreated();
                    await _schedulerEngine.Start();
                    await _executionEngine.Start();
                    IsRunning = true;
                }
            }

        }

        public async Task Stop()
        {
            if (IsRunning == true)
            {
                await _executionEngine.Stop();
                await _schedulerEngine.Stop();
                IsRunning = false;
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

        public RootModel GetActivityModels()
        {
            RootModel topModel = new RootModel("Activities");

            var getActivitiesResult = _activityStore.GetAll().GetAwaiter().GetResult();

            foreach(var activity in getActivitiesResult.Result)
            {
                ActivityModel model = new ActivityModel(activity);
                topModel.Children.Add(model);
            }

            return topModel;
        }

        public RootModel GetCalendarModels()
        {
            RootModel topModel = new RootModel("Calendars");

            var calendars = _calendarStore.GetAll(new CalendarRetrievalParameters() { AddChildEvents = true, AddChildFolders = true, AsTree = true }).GetAwaiter().GetResult(); 
            foreach (var calendar in calendars.Result)
            {
                CalendarModel model = new CalendarModel(calendar);
                topModel.Children.Add(model);

                //  handle the child calendars
                //SetCalendarChildModels(model, calendar);
                //  being done in the parent itself
            }

            return topModel;
            
        }

        public RootModel GetStatusModels()
        {
            RootModel topModel = new RootModel("Status");



            return topModel;
        }

        public RootModel GetLogModels()
        {
            RootModel topModel = new RootModel("Log");



            return topModel;
        }

        public List<ITreeItem> GetAllRoots()
        {
            List<ITreeItem> rootItems = new List<ITreeItem>()
            {
                GetCalendarModels(),
                GetActivityModels(),
                GetStatusModels(),
                GetLogModels()
            };

            return rootItems;
        }

    }
}
