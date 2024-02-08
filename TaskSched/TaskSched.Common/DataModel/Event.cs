using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSched.Common.DataModel
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime LastExecution { get; set; }
        public DateTime NextExecution { get; set; }
        public bool IsActive { get; set; }
        public bool CatchUpOnStartup { get; set; }
        public Guid? CalendarId { get; set; }

        public List<EventSchedule> Schedules { get; set; }
        public List<EventActivity> Activities { get; set; }


    }
}
