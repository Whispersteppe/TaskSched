using TaskSched.Common.DataModel;

namespace TaskSched.Common.Extensions
{
    public static class EventActivityExtensions
    {
        public static void SetForCreate(this EventActivity eventActivity)
        {
            eventActivity.Id = Guid.NewGuid();
            //eventActivity.ActivityId = Guid.NewGuid();
            foreach(var field in eventActivity.Fields)
            {
                field.SetForCreate();
            }
        }
    }




}
