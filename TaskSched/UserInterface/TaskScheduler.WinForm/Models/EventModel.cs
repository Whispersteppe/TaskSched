using TaskSched.Common.DataModel;

namespace TaskScheduler.WinForm.Models
{
    public class EventModel : BaseTreeItemModel<Event>
    {
        public override TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.EventItem;

        public EventModel(Event eventItem, ITreeItem? parent)
            :base(eventItem, parent)
        {
            Name = eventItem.Name;
            ID = eventItem.Id;

        }

        public EventModel(ITreeItem? parent)
            :this(new Event(), parent)
        {

        }

        public DateTime LastExecutionDate
        {
            get
            {
                return Item?.LastExecution ?? DateTime.Now;
            }
        }

        public DateTime NextExecutionDate
        {
            get
            {
                return Item?.NextExecution ?? DateTime.Now;
            }
        }

    }
}
