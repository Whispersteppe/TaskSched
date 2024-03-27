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
        public string Name { get; set; } = "New Event";
        public DateTime LastExecution { get; set; } = DateTime.MinValue;
        public DateTime NextExecution { get; set; } = DateTime.MinValue;
        public bool IsActive { get; set; }
        public bool CatchUpOnStartup { get; set; }
        public Guid? FolderId { get; set; }

        public List<EventSchedule>? Schedules { get; set; } = new List<EventSchedule>();
        public List<EventActivity>? Activities { get; set; } = new List<EventActivity>();


    }
}
