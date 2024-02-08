using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.Interfaces;

namespace TaskSched.SchedulerEngine
{
    internal class EngineJobFactory : IJobFactory
    {

        IExecutionEngine _executionEngine;
        IEventStore _eventStore;
        IActivityStore _activityStore;
        ILogger _logger;

        public EngineJobFactory(
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

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            JobExec handler = new JobExec(_executionEngine, _eventStore, _activityStore, _logger);

            return handler;
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
