using TaskSched.Common.DataModel;

namespace TaskScheduler.WinForm.Models
{
    public class CalendarModel : BaseTreeItemModel<Calendar>
    {
        List<CalendarModel> _childCalendars = new List<CalendarModel>();
        List<EventModel> _childEvents = new List<EventModel>();


        public override TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.CalendarItem;

        public CalendarModel(Calendar calendar, ITreeItem? parent)
            :base(calendar, parent)
        {
            Name = calendar.Name;
            ID = calendar.Id;

            AllowedChildTypes = [TreeItemTypeEnum.CalendarItem, TreeItemTypeEnum.EventItem];
            this.AllowedMoveToParentTypes = [TreeItemTypeEnum.CalendarItem, TreeItemTypeEnum.CalendarRootItem]; 

            foreach (var childCalendar in calendar.ChildCalendars)
            {
                _childCalendars.Add(new CalendarModel(childCalendar, this));
            }

            foreach(var childEvent in calendar.Events)
            {
                _childEvents.Add(new EventModel(childEvent, this));
            }

            Children = [.. _childCalendars, .. _childEvents];
        }

        public CalendarModel(ITreeItem? parent)
            :this(new Calendar(), parent)
        {
        }
        public override bool CanHaveChildren()
        {
            return true;
        }


    }

}
