using TaskSched.Common.DataModel;

namespace TaskScheduler.WinForm.Models
{
    public class ActivityModel : BaseTreeItemModel<Activity>
    {

        public override TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.ActivityItem;

        public ActivityModel(Activity activity, ITreeItem? parent)
            :base(activity, parent)
        {
            ID = activity.Id;
        }

        public ActivityModel(ITreeItem? parent)
            :this(new Activity(), parent) 
        {
        }

        public override string Name 
        {
            get
            {
                if (Item == null) return base.Name;
                return Item.Name;
            }
            protected set
            {
                if (Item != null)
                {
                    Item.Name = value;
                }
                else
                {
                    base.Name = value;
                }
            }

        }



    }
}
