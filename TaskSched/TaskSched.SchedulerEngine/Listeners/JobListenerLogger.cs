using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskSched.SchedulerEngine.Listeners
{


    internal class JobListenerLogger : ListenerBase,  IJobListener
    {
        public JobListenerLogger(ILogger logger) : base(logger)
        {
        }

        public string Name => "Job Listener";

        public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Job Vetoed - {ToJsonString(context)}");
        }

        public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Job to be executed - {ToJsonString(context)}");
        }

        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Job was executed - {ToJsonString(context)}");
        }
    }
}
