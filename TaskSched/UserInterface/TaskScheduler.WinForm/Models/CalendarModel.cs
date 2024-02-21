using TaskSched.Common.DataModel;

namespace TaskScheduler.WinForm.Models
{
    public class CalendarModel : ITreeItem
    {
        Calendar _calendar;
        List<CalendarModel> _childCalendars;
        List<EventModel> _childEvents;

        
        public CalendarModel(Calendar calendar)
        {
            _calendar = calendar;
            _childEvents = new List<EventModel>();
            _childCalendars = new List<CalendarModel>();

            foreach(var childCalendar in calendar.ChildCalendars)
            {
                _childCalendars.Add(new CalendarModel(childCalendar));
            }

            foreach(var childEvent in calendar.Events)
            {
                _childEvents.Add(new EventModel(childEvent));
            }
        }

        public string Name
        {
            get
            {
                return _calendar.Name;
            }
        }

        public List<ITreeItem>? Children
        {
            get
            {
                List<ITreeItem> allChildren = [.. _childCalendars, .. _childEvents];

                return allChildren;
            }

        }

        public void Revert()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }

    //public class ProcessModel : ITreeItem
    //{
    //    Proce

    //}
}
