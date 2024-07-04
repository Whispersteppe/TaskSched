using TaskSched.Common.DataModel;

namespace TaskSched.Common.Extensions
{
    public static class ActivityExtensions
    {
        public static void SetForCreate(this Activity activity)
        {
            activity.Id = Guid.NewGuid();
            foreach (var field in activity.DefaultFields)
            {

            }
        }
    }




}
