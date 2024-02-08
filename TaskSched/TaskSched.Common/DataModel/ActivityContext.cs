using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSched.Common.DataModel
{
    public class ActivityContext
    {
        public Event EventItem { get; set; }
        public EventActivity EventActivity { get; set; }
        public Activity Activity { get; set; }
    }
}
