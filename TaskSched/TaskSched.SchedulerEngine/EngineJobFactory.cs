 using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.Delegates;
using TaskSched.Common.Interfaces;

namespace TaskSched.SchedulerEngine
{
    internal class EngineJobFactory : IJobFactory
    {

        IExecutionEngine _executionEngine;
        IEventStore _eventStore;
        IActivityStore _activityStore;
        ILogger _logger;

        public event EventAction OnStartEvent;
        public event EventAction OnFinishEvent;

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

            handler.OnFinishEvent += Handler_OnFinishEvent;
            handler.OnStartEvent += Handler_OnStartEvent;

            return handler;
        }

        private void Handler_OnStartEvent(Common.DataModel.Event context)
        {
            OnStartEvent?.Invoke(context);
        }

        private void Handler_OnFinishEvent(Common.DataModel.Event context)
        {
            OnFinishEvent?.Invoke(context);
        }

        public void ReturnJob(IJob job)
        {
            if (job is JobExec jobExec)
            {
                jobExec.OnFinishEvent -= Handler_OnFinishEvent;
                jobExec.OnStartEvent -= Handler_OnStartEvent;
            }

            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
