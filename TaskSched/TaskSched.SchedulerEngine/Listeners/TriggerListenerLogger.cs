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
            Logger.LogInformation($"Trigger complete {trigger} {context} {triggerInstructionCode}");
        }

        public async Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Trigger fired {trigger} {context}");
        }

        public async Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Trigger misfired {trigger}");
        }

        public async Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Trigger veto {trigger} {context}");
            return false;  //  we're not vetoing anything
        }
    }
}
