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

        Lazy<IScheduler> _scheduler;

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

            _scheduler = new Lazy<IScheduler>(InitializeQuartz);

        }


        #region lazy loader

        /// <summary>
        /// lazy load the scheduler on first use.
        /// </summary>
        /// <returns></returns>
        private IScheduler InitializeQuartz()
        {
            var properties = new NameValueCollection();

            var scheduler = SchedulerBuilder.Create(properties)
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

            scheduler.JobFactory = new EngineJobFactory(_executionEngine, _eventStore, _activityStore, _logger);

            //  we'll want to load the scheduler with the current jobs from the database
            LoadEventsFromDatastore().GetAwaiter().GetResult();

            return scheduler;


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
        /// loads a single event to quartz
        /// </summary>
        /// <param name="eventItem"></param>
        /// <returns></returns>
        private async Task LoadEvent(Event eventItem)
        {
            IJobDetail job = JobBuilder.Create<JobExec>()
                 .WithIdentity(eventItem.JobKey())
                 .Build();

            await _scheduler.Value.AddJob(job, true, true);

            foreach (var schedule in eventItem.Schedules)
            {
                ITrigger trigger = TriggerBuilder.Create()
                 .WithIdentity(schedule.TriggerKey())
                 .ForJob(job)
                 .StartNow()
                 .WithCronSchedule(schedule.CRONData)
                 .Build();

                await _scheduler.Value.ScheduleJob(trigger);
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

            await LoadEvent(rsltGet.Result);

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

            await _scheduler.Value.DeleteJob(rsltGet.Result.JobKey());

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

            await _scheduler.Value.DeleteJob(eventItem.JobKey());

            var rslt = await _eventStore.Update(eventItem);

            await LoadEvent(eventItem);

            return rslt;
        }

        #endregion

        #region scheduler control

        /// <summary>
        /// Starts the underlying quartz instance
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            await _scheduler.Value.Start();
        }



        /// <summary>
        /// stops the underlying scheduler
        /// </summary>
        /// <returns></returns>
        public async Task Stop()
        {
            if (_scheduler.IsValueCreated == false)
            {
                //  scheduler hasn't been created het, so don't create it and then destroy it.
                return;
            }

            await _scheduler.Value.Shutdown();
            
            IDisposable? disposable = _scheduler as IDisposable;
            disposable?.Dispose();

            _scheduler = new Lazy<IScheduler>(InitializeQuartz);
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
