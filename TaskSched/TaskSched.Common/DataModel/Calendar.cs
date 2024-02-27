using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSched.Common.DataModel
{
    public class Calendar
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "New Calendar";

        public Guid? ParentCalendarId { get; set; }
        public List<Event> Events { get; set; } = new List<Event>();
        public List<Calendar> ChildCalendars { get; set; } = new List<Calendar>();

    }


}
