namespace TaskScheduler.WinForm.Models
{
    public class ActivityRootModel : BaseTreeItemModel
    {

        public override TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.ActivityRootItem;

        public ActivityRootModel(ITreeItem? parent)
            :base(parent, "")
        {
            Name = "Activities";
            AllowedChildTypes = [TreeItemTypeEnum.ActivityItem];
        }


    }

}
