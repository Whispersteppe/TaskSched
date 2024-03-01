namespace TaskScheduler.WinForm.Models
{
    public class ActivityRootModel : BaseTreeItemModel
    {

        public override TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.ActivityRootItem;

        public ActivityRootModel(ITreeItem? parent)
            :base(parent, "")
        {
            DisplayName = "Activities";
            AllowedChildTypes = [TreeItemTypeEnum.ActivityItem];
        }


    }

}
