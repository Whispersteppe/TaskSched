using TaskSched.Common.DataModel;
using TaskSched.Common.Delegates;

namespace TaskSched.Common.Interfaces
{
    public interface IExecutionEngine
    {
        Task DoActivity(ActivityContext activity);

        Task Stop();
        Task Start();

        ExecutionStatusEnum ExecutionStatus { get; }

        public event ActivityAction OnStartActivity;
        public event ActivityAction OnFinishActivity;

    }
}
