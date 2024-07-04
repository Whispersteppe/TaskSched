using TaskSched.Common.DataModel;

namespace TaskSched.Common.Extensions
{
    public static class EventScheduleExtensions
    {
        public static void SetForCreate(this EventSchedule eventSchedule)
        {
            eventSchedule.Id = Guid.NewGuid();
        }
    }




}
