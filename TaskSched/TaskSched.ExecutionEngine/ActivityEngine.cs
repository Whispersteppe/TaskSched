using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
using TaskSched.Common.DataModel;
using TaskSched.Common.Delegates;
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

        List<IDataflowBlock> _allBlocks;

        public ExecutionStatusEnum ExecutionStatus { get; private set; }

        public event ActivityAction OnStartActivity;
        public event ActivityAction OnFinishActivity;

        ITargetBlock<ActivityContext>? _pipeline;

        /// <summary>
        /// constructor
        /// </summary>
        public ActivityEngine(ILogger logger, IExecutionStore executionStore) 
        {
            _logger = logger;
            _executionStore = executionStore;

            ExecutionStatus = ExecutionStatusEnum.Stopped;

        }

        /// <summary>
        /// do the activities against an event
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public async Task DoActivity(ActivityContext activity)
        {
            if (ExecutionStatus == ExecutionStatusEnum.Running)
            {

                OnStartActivity?.Invoke(activity);

                _logger.LogInformation($"Starting Activity {activity.EventItem.Name}");

                Debug.WriteLine($"in Activity Engine - {activity.EventItem.Name}");

                _pipeline.Post(activity);

                _logger.LogInformation($"Completing Activity {activity.EventItem.Name}");
            }
            else
            {
                _logger.LogError($"Trying to queue activity {activity.EventItem.Name} while engine is not running");
            }

        }

        internal ITargetBlock<ActivityContext> CreateDataflowPipeline()
        {
            _logger.LogInformation($"Creating the dataflow pipeline");

            _allBlocks = new List<IDataflowBlock>();
            var ingestBlock = new TransformBlock<ActivityContext, ActivityContext>(IngestProcess);
            _allBlocks.Add(ingestBlock);
            var ingestBuffer = new BufferBlock<ActivityContext>();
            _allBlocks.Add(ingestBuffer);
            ingestBlock.LinkTo(ingestBuffer);



            var finalBuffer = new BufferBlock<ActivityContext>();
            _allBlocks.Add(finalBuffer);
            var finalBlock = new ActionBlock<ActivityContext>(FinalProcess);
            _allBlocks.Add(finalBlock);
            finalBuffer.LinkTo(finalBlock);



            // all of the handlers
            var handlerList = _executionStore.GetExecutionHandlers().GetAwaiter().GetResult();

            foreach (IExecutionHandler handler in handlerList)
            {
                var executionHandler = new TransformBlock<ActivityContext, ActivityContext>(handler.HandleActivity);
                _allBlocks.Add(executionHandler);
                ingestBuffer.LinkTo(executionHandler, x => { return x.Activity.ActivityHandlerId == handler.HandlerInfo.HandlerId; });
                executionHandler.LinkTo(finalBuffer);

            }

            var notFoundHandler = new TransformBlock<ActivityContext, ActivityContext>(NotFoundHandler);
            _allBlocks.Add(notFoundHandler);
            ingestBuffer.LinkTo(notFoundHandler);
            notFoundHandler.LinkTo(finalBuffer);

            _logger.LogInformation($"dataflow pipeline built");


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
            OnFinishActivity?.Invoke( context );
        }


        #region scheduler control



        /// <summary>
        /// Starts the underlying quartz instance
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            if (ExecutionStatus == ExecutionStatusEnum.Stopped)
            {
                _logger.LogInformation($"Starting Activity Engine");

                ExecutionStatus = ExecutionStatusEnum.Starting;
                _pipeline = CreateDataflowPipeline();

                ExecutionStatus = ExecutionStatusEnum.Running;
                _logger.LogInformation($"Activity Engine started");
            }
            else
            {
                {
                    _logger.LogError($"Cannot start Activity Engine - in non-stopped state - {ExecutionStatus}");

                }
            }
        }



        /// <summary>
        /// stops the underlying scheduler
        /// </summary>
        /// <returns></returns>
        public async Task Stop()
        {

            if (ExecutionStatus == ExecutionStatusEnum.Running)
            {
                _logger.LogInformation($"Stopping Activity Engine");

                ExecutionStatus = ExecutionStatusEnum.Stopping;

                foreach (var block in _allBlocks)
                {

                    block.Complete();

                    if (block is IDisposable disposableBlock)
                    {
                        disposableBlock.Dispose();
                    }
                }
                _allBlocks.Clear();

                _pipeline = null;

                ExecutionStatus = ExecutionStatusEnum.Stopped;

                _logger.LogInformation($"Activity Engine Stopped");
            }
            else
            {
                _logger.LogError($"Cannot stop Activity Engine - in non-running state - {ExecutionStatus}");

            }
        }

        #endregion
    }
}
