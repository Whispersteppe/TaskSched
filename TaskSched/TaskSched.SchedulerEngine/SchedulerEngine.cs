using Microsoft.Extensions.Logging;
using Quartz;
using System.Collections.Specialized;
using TaskSched.Common.DataModel;
using TaskSched.Common.Interfaces;

namespace TaskSched.SchedulerEngine
{
    /// <summary>
    /// Scheduler Engine
    /// </summary>
    /// <remarks>
    /// this is a wrapper around Quartz to better support my events and activities
    /// </remarks>
    public class SchedulerEngine : ISchedulerEngine
    {
        IEventStore _eventStore;
        IExecutionEngine _executionEngine;
        IActivityStore _activityStore;
        ILogger _logger;

        IScheduler _scheduler;

        /// <summary>
        /// Scheduler Engine constructor
        /// </summary>
        /// <param name="executionEngine"></param>
        /// <param name="eventStore"></param>
        /// <param name="activityStore"></param>
        public SchedulerEngine(
            IExecutionEngine executionEngine, 
            IEventStore eventStore, 
            IActivityStore activityStore, 
            ILogger logger) 
        { 
            _executionEngine = executionEngine;
            _eventStore = eventStore;
            _activityStore = activityStore;
            _logger = logger;

            IsRunning = false;

            InitializeQuartz();

        }


        #region lazy loader

        /// <summary>
        /// lazy load the scheduler on first use.
        /// </summary>
        /// <returns></returns>
        private void InitializeQuartz()
        {
            var properties = new NameValueCollection();

            _scheduler = SchedulerBuilder.Create(properties)
                // default max concurrency is 10
                .UseDefaultThreadPool(x => x.MaxConcurrency = 5)
                // this is the default 
                // .WithMisfireThreshold(TimeSpan.FromSeconds(60))
                //.UseJobFactory<EngineJobFactory>()
                .BuildScheduler()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult()
                ;

            _scheduler.JobFactory = new EngineJobFactory(_executionEngine, _eventStore, _activityStore, _logger);

        }



        /// <summary>
        /// loads a single event to quartz
        /// </summary>
        /// <param name="eventItem"></param>
        /// <returns></returns>
        private async Task LoadEvent(Event eventItem)
        {
            if (eventItem == null) return;
            if (eventItem.IsActive == false) return;

            DateTime? nextExecution = await SchedulerUtility.GetNextFireTimeForJob(eventItem.JobKey(), _scheduler);
            if (nextExecution != null)
            {
                if (eventItem.NextExecution != nextExecution)
                {
                    eventItem.NextExecution = nextExecution.Value;
                    await _eventStore.Update(eventItem);
                }
            }

            IJobDetail job = JobBuilder.Create<JobExec>()
                 .WithIdentity(eventItem.JobKey())
                 .Build();

            await _scheduler.AddJob(job, true, true);

            foreach (var schedule in eventItem.Schedules)
            {
                ITrigger trigger = TriggerBuilder.Create()
                 .WithIdentity(schedule.TriggerKey())
                 .ForJob(job)
                 .StartNow()
                 .WithCronSchedule(schedule.CRONData)
                 .Build();

                await _scheduler.ScheduleJob(trigger);
            }

            if (eventItem.CatchUpOnStartup == true)
            {
                if (DateTime.Now > eventItem.NextExecution)
                {
                    await _scheduler.TriggerJob(eventItem.JobKey());
                }
            }

        }

        #endregion

        #region IActivityStore functionality

        /// <summary>
        /// creates a new event, saving to the data store and adding it to Quartz
        /// </summary>
        /// <param name="eventItem"></param>
        /// <returns></returns>
        public async Task<ExpandedResult<Guid>> CreateEvent(Event eventItem)
        {

            var rsltCreate = await _eventStore.Create(eventItem);
            var rsltGet = await _eventStore.Get(rsltCreate.Result);
            rsltGet.Messages.AddRange(rsltCreate.Messages);

            if (IsRunning == true)
            {
                await LoadEvent(rsltGet.Result);
            }

            return rsltCreate;
        }

        /// <summary>
        /// Deletes an event, removing it from the store and from quartz
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<ExpandedResult> DeleteEvent(Guid eventId)
        {
            var rsltGet = await _eventStore.Get(eventId);

            if (IsRunning == true)
            {
                await _scheduler.DeleteJob(rsltGet.Result.JobKey());
            }

            var rslt = await _eventStore.Delete(eventId);
            return rslt;
        }

        /// <summary>
        /// retrieves all events
        /// </summary>
        /// <returns></returns>
        public async Task<ExpandedResult<List<Event>>> GetAllEvents()
        {
            var rslt = await _eventStore.GetAll();
            return rslt;
        }

        /// <summary>
        /// retrieves a single specific event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<ExpandedResult<Event>> GetEvent(Guid eventId)
        {
            var rslt = await _eventStore.Get(eventId);

            return rslt;
        }


        /// <summary>
        /// updates the event, both in the data store and in quartz
        /// </summary>
        /// <param name="eventItem"></param>
        /// <returns></returns>
        /// <remarks>
        /// it doesn't update the quartz job.  instead, it removes it and readds it
        /// </remarks>
        public async Task<ExpandedResult> UpdateEvent(Event eventItem)
        {
            if (IsRunning == true)
            {
                await _scheduler.DeleteJob(eventItem.JobKey());

            }

            var rslt = await _eventStore.Update(eventItem);

            if (IsRunning == true)
            {
                await LoadEvent(eventItem);
            }

            return rslt;
        }

        #endregion

        #region scheduler control

        public bool IsRunning { get; private set; }

        /// <summary>
        /// Starts the underlying quartz instance
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            if (IsRunning == false)
            {

                //  we'll want to load the scheduler with the current jobs from the database
                await LoadEventsFromDatastore();

                await _scheduler.Start();
                IsRunning = true;
            }
        }

        /// <summary>
        /// loads all current events from the data store
        /// </summary>
        /// <returns></returns>
        private async Task LoadEventsFromDatastore()
        {
            var rslt = await _eventStore.GetAll();

            foreach (var eventItem in rslt.Result)
            {
                await LoadEvent(eventItem);
            }
        }


        /// <summary>
        /// stops the underlying scheduler
        /// </summary>
        /// <returns></returns>
        public async Task Stop()
        {
            if (IsRunning == true)
            {


                await _scheduler.Shutdown();

                IDisposable? disposable = _scheduler as IDisposable;
                disposable?.Dispose();

                InitializeQuartz();

                IsRunning = false;
            }
        }

        #endregion
    }

    /// <summary>
    /// extensions to help in scheduling
    /// </summary>
    internal static class SchedulerHelperExtensions
    {
        /// <summary>
        /// creates a job key from the event
        /// </summary>
        /// <param name="eventItem"></param>
        /// <returns></returns>
        public static JobKey JobKey(this Event eventItem)
        {
            JobKey jk = new JobKey(eventItem.Id.ToString());

            return jk;
        }

        /// <summary>
        /// creates a trigger key from the event schedule
        /// </summary>
        /// <param name="scheduleItem"></param>
        /// <returns></returns>
        public static TriggerKey TriggerKey(this EventSchedule scheduleItem)
        {
            TriggerKey tk = new TriggerKey(scheduleItem.Id.ToString());

            return tk;
        }
    }
}
