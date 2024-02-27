using TaskSched.Common.DataModel;

namespace TaskSched.Common.Interfaces
{
    public interface IActivityStore
    {
        Task<ExpandedResult<Activity?>> Get(Guid activityId);
        Task<ExpandedResult<List<Activity>>> GetAll();
        Task<ExpandedResult> Update(Activity activity);
        Task<ExpandedResult> Delete(Guid activityId);
        Task<ExpandedResult<Guid>> Create(Activity activity);
    }
}
