using TaskSched.Common.DataModel;

namespace TaskSched.Common.Interfaces
{
    public interface ISchedulerEngine
    {

        Task<ExpandedResult<Event>> GetEvent(Guid eventId);
        Task<ExpandedResult<List<Event>>> GetAllEvents();
        Task<ExpandedResult> UpdateEvent(Event eventItem);
        Task<ExpandedResult> DeleteEvent(Guid eventId);
        Task<ExpandedResult<Guid>> CreateEvent(Event eventItem);

        Task Stop();
        Task Start();



    }
}
