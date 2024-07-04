using TaskSched.Common.DataModel;

namespace TaskSched.Common.Extensions
{
    public static class ActivityFieldExtensions
    {
        public static void SetForCreate(this ActivityField activityField)
        {
            activityField.Id = Guid.NewGuid();
        }
    }




}
