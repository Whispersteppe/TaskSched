using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.DataModel;

namespace TaskSched.Common.Extensions
{

    public static class EventExtensions
    {
        public static void SetForCreate(this Event eventItem)
        {
            eventItem.Id = Guid.NewGuid();
            foreach (var activity in eventItem.Activities)
            {
                activity.SetForCreate();
            }
        }
    }




}
