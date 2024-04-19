using Microsoft.Extensions.Logging;
using Quartz;

namespace TaskSched.SchedulerEngine.Listeners
{
    internal class TriggerListenerLogger : ListenerBase,  ITriggerListener
    {
        public TriggerListenerLogger(ILogger logger) : base(logger)
        {
        }

        public string Name => "Trigger Listener";

        public async Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Trigger complete {ToJsonString(trigger)} {ToJsonString(context)} {ToJsonString(triggerInstructionCode)}");
        }

        public async Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Trigger fired {ToJsonString(trigger)} {ToJsonString(context)}");
        }

        public async Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Trigger misfired {ToJsonString(trigger)}");
        }

        public async Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Trigger veto {ToJsonString(trigger)} {ToJsonString(context)}");
            return false;  //  we're not vetoing anything
        }
    }
}
