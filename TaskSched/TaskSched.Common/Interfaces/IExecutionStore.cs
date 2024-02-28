using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.DataModel;

namespace TaskSched.Common.Interfaces
{
    public interface IExecutionStore
    {

        Task<List<ExecutionHandlerInfo>> GetHandlerInfo();
        Task<List<IExecutionHandler>> GetExecutionHandlers();
        Task<IExecutionHandler?> GetExecutionHandler(Guid handlerId);

    }

    public interface IExecutionHandler
    {
        public ExecutionHandlerInfo HandlerInfo { get; }
        public Task<ActivityContext> HandleActivity(ActivityContext context);
        public Task<ExpandedResult<bool>> ValidateActivity(Activity? activity);
        public Task<ExpandedResult<bool>> ValidateEventActivity(Activity? activity, EventActivity? eventActivity);
    }

    public class ExecutionHandlerInfo
    {
        public string Name { get; set; }
        public Guid HandlerId { get; set; }
        public List<ActivityField> RequiredFields { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }
}
