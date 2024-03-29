namespace TaskScheduler.WinForm.Models
{
    public class StatusRootModel : RootModel
    {
        public override TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.StatusRootItem;

        public StatusRootModel(ITreeItem? parent)
            :base(parent, "")
        {
            DisplayName = "Status";

        }

        public override bool CanHaveChildren()
        {
            return true;
        }

    }

}
