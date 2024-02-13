using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
using TaskSched.Common.DataModel;
using TaskSched.Common.Interfaces;
using ActivityContext = TaskSched.Common.DataModel.ActivityContext;

namespace TaskSched.ExecutionEngine
{
    /// <summary>
    ///  the engine that will handle doing all of the activities for an event.
    /// </summary>
    public class ActivityEngine : IExecutionEngine
    {
        ILogger _logger;
        IExecutionStore _executionStore;

        ITargetBlock<ActivityContext> _pipeline;

        /// <summary>
        /// constructor
        /// </summary>
        public ActivityEngine(ILogger logger, IExecutionStore executionStore) 
        {
            _logger = logger;
            _executionStore = executionStore;
            _pipeline = CreateDataflowPipeline();
        }

        /// <summary>
        /// do the activities against an event
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public async Task DoActivity(ActivityContext activity)
        {
            _logger.LogInformation($"Starting Activity {activity.EventItem.Name}");

            Debug.WriteLine($"in Activity Engine - {activity.EventItem.Name}");

            _pipeline.Post(activity);

            _logger.LogInformation($"Completing Activity {activity.EventItem.Name}");

        }

        internal ITargetBlock<ActivityContext> CreateDataflowPipeline()
        {
            var ingestBlock = new TransformBlock<ActivityContext, ActivityContext>(IngestProcess);
            var ingestBuffer = new BufferBlock<ActivityContext>();
            ingestBlock.LinkTo(ingestBuffer);



            var finalBuffer = new BufferBlock<ActivityContext>();
            var finalBlock = new ActionBlock<ActivityContext>(FinalProcess);
            finalBuffer.LinkTo(finalBlock);



            // all of the handlers
            var handlerList = _executionStore.GetExecutionHandlers().GetAwaiter().GetResult();

            foreach (IExecutionHandler handler in handlerList)
            {
                var executionHandler = new TransformBlock<ActivityContext, ActivityContext>(handler.HandleActivity);

                ingestBuffer.LinkTo(executionHandler, x => { return x.Activity.ActivityHandlerId == handler.HandlerInfo.HandlerId; });
                executionHandler.LinkTo(finalBuffer);

            }


            var notFoundHandler = new TransformBlock<ActivityContext, ActivityContext>(NotFoundHandler);
            ingestBuffer.LinkTo(notFoundHandler);
            notFoundHandler.LinkTo(finalBuffer);

            return ingestBlock;
        }

        internal async Task<ActivityContext> IngestProcess(ActivityContext context)
        {
            Debug.WriteLine($"Ingest process: {context}");
            return context;
        }


        internal async Task<ActivityContext> NotFoundHandler(ActivityContext context)
        {
            Debug.WriteLine($"Not Found Handler: {context}");
            return context;
        }


        internal async Task FinalProcess(ActivityContext context)
        {
            Debug.WriteLine($"Final Process: {context}");
        }


        #region scheduler control

        /// <summary>
        /// Starts the underlying quartz instance
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
        }



        /// <summary>
        /// stops the underlying scheduler
        /// </summary>
        /// <returns></returns>
        public async Task Stop()
        {

        }

        #endregion
    }
}
