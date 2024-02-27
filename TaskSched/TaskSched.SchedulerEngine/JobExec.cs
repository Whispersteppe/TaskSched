using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.DataModel;
using TaskSched.Common.Interfaces;

namespace TaskSched.SchedulerEngine
{
    internal class JobExec : IJob
    {
        IExecutionEngine _executionEngine;
        IEventStore _eventStore;
        IActivityStore _activityStore;
        ILogger _logger;

        public JobExec(
            IExecutionEngine executionEngine, 
            IEventStore eventStore, 
            IActivityStore activityStore,
            ILogger logger) 
        { 
            _executionEngine = executionEngine;
            _eventStore = eventStore;
            _activityStore = activityStore;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                Guid eventId = Guid.Parse(context.JobDetail.Key.Name);

                var eventGet = await _eventStore.Get(eventId);
                Event eventItem = eventGet.Result;

                _logger.LogInformation($"Trigger for {eventItem.Name}. Executing the activity");

                foreach (var eventActivity in eventItem.Activities)
                {
                    var activityGet = await _activityStore.Get(eventActivity.ActivityId);
                    Activity activity = activityGet.Result;

                    ActivityContext activityContext = new ActivityContext()
                    {
                        EventItem = eventItem,
                        EventActivity = eventActivity,
                        Activity = activity
                    };

                    _executionEngine.DoActivity(activityContext);

                }

                eventItem.LastExecution = DateTime.Now;
                DateTime? nextExecution = await SchedulerUtility.GetNextFireTimeForJob(context.JobDetail.Key, context.Scheduler);
                eventItem.NextExecution = nextExecution ?? DateTime.MaxValue;

                await _eventStore.Update(eventItem);


                Console.WriteLine($"{DateTime.Now}: {context.JobDetail.Key}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                //  rethrow as a job execution exception 
                var jee = new JobExecutionException(ex);
                throw jee;
            }
        }

    }
}
