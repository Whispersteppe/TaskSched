namespace TaskScheduler.WinForm.Models
{
    public class StatusRootModel : BaseTreeItemModel
    {
        public override TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.StatusRootItem;

        public StatusRootModel(ITreeItem? parent)
            :base(parent, "")
        {
            DisplayName = "Status";

        }

    }

}
