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
        public string Name { get; set; }

        public Guid? ParentCalendarId { get; set; }
        public List<Event> Events { get; set; }
        public List<Calendar> ChildCalendars { get; set; }

    }


}
