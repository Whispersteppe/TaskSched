using Microsoft.Extensions.Logging;
using TaskSched.Common.Interfaces;

namespace TaskSched.ExecutionStore
{
    public class ExecutionStore : IExecutionStore
    {

        List<IExecutionHandler> _handlers;
        ILogger _logger;

        public ExecutionStore(ILogger logger) 
        {
            _logger = logger;

            _handlers = new List<IExecutionHandler>()
            {
                new FileExecuteHandler(_logger)
            };
        }


        public async Task<List<IExecutionHandler>> GetExecutionHandlers()
        {
            return _handlers;
        }


        public async Task<IExecutionHandler?> GetExecutionHandler(Guid handlerId)
        {
            var handler = _handlers.FirstOrDefault(x=>x.HandlerInfo.HandlerId == handlerId);
            return handler;

        }

    }
}
