using TaskSched.Common.DataModel;

namespace TaskSched.Common.Extensions
{
    public static class EventActivityFieldExtensions
    {
        public static void SetForCreate(this EventActivityField field)
        {
            field.Id = Guid.NewGuid();
            //eventActivity.ActivityId = Guid.NewGuid();
        }
    }




}
