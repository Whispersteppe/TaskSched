using TaskSched.Common.DataModel;

namespace TaskSched.Common.Interfaces
{
    public interface IExecutionEngine
    {
        Task DoActivity(ActivityContext activity);

        Task Stop();
        Task Start();

        ExecutionStatusEnum ExecutionStatus { get; }
    }
}
