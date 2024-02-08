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

        ITargetBlock<ActivityContext> _pipeline;

        /// <summary>
        /// constructor
        /// </summary>
        public ActivityEngine(ILogger logger) 
        {
            _logger = logger;
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

            // all of the handlers
            var executionHandler = new TransformBlock<ActivityContext, ActivityContext>(FoundExecutionHandler);
            var notFoundHandler = new TransformBlock<ActivityContext, ActivityContext>(NotFoundHandler);


            var finalBuffer = new BufferBlock<ActivityContext>();
            var finalBlock = new ActionBlock<ActivityContext>(FinalProcess);

            //  now link everything up

            ingestBlock.LinkTo(ingestBuffer);

            ingestBuffer.LinkTo(executionHandler, x => { return x.Activity.ActivityType == ActivityTypeEnum.ExternalProgram; });
            ingestBuffer.LinkTo(notFoundHandler);


            executionHandler.LinkTo(finalBuffer);
            notFoundHandler.LinkTo(finalBuffer);
            finalBuffer.LinkTo(finalBlock);

            return ingestBlock;
        }

        internal async Task<ActivityContext> IngestProcess(ActivityContext context)
        {
            Debug.WriteLine($"Ingest process: {context}");
            return context;
        }

        internal async Task<ActivityContext> FoundExecutionHandler(ActivityContext context)
        {
            Debug.WriteLine($"Found Execution Handler: {context}");
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
