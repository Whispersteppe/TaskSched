using Microsoft.Extensions.Logging;
using Quartz;

namespace TaskSched.SchedulerEngine.Listeners
{
    internal class ScheduleListenerLogger : ListenerBase, ISchedulerListener
    {
        public ScheduleListenerLogger(ILogger logger) : base(logger)
        {
        }

        public async Task JobAdded(IJobDetail jobDetail, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Job Added {jobDetail}");
        }

        public async Task JobDeleted(JobKey jobKey, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Job Deleted {jobKey}");
        }

        public async Task JobInterrupted(JobKey jobKey, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Job Interrupted {jobKey}");
        }

        public async Task JobPaused(JobKey jobKey, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Job Paused {jobKey}");
        }

        public async Task JobResumed(JobKey jobKey, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Job Resumed {jobKey}");
        }

        public async Task JobScheduled(ITrigger trigger, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Job Scheduled {trigger}");
        }

        public async Task JobsPaused(string jobGroup, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Jobs Paused {jobGroup}");
        }

        public async Task JobsResumed(string jobGroup, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Jobs Resumed {jobGroup}");
        }

        public async Task JobUnscheduled(TriggerKey triggerKey, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Job Unscheduled {triggerKey}");
        }

        public async Task SchedulerError(string msg, SchedulerException cause, CancellationToken cancellationToken = default)
        {
            Logger.LogError(cause, $"Scheduler Error {msg}");
        }

        public async Task SchedulerInStandbyMode(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Scheduler In Standby Mode");
        }

        public async Task SchedulerShutdown(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Scheduler Shut Down");
        }

        public async Task SchedulerShuttingdown(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Scheduler Shutting Down");
        }

        public async Task SchedulerStarted(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Scheduler Started");
        }

        public async Task SchedulerStarting(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Scheduler Starting");
        }

        public async Task SchedulingDataCleared(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Scheduling Data Cleared");
        }

        public async Task TriggerFinalized(ITrigger trigger, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Trigger Finalized {trigger}");
        }

        public async Task TriggerPaused(TriggerKey triggerKey, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Trigger Paused {triggerKey}");
        }

        public async Task TriggerResumed(TriggerKey triggerKey, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Trigger Resumed {triggerKey}");
        }

        public async Task TriggersPaused(string? triggerGroup, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Triggers Paused {triggerGroup}");
        }

        public async Task TriggersResumed(string? triggerGroup, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Triggers Resumed {triggerGroup}");
        }
    }
}
