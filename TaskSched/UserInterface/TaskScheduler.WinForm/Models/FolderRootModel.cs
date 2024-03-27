namespace TaskScheduler.WinForm.Models
{
    public class FolderRootModel : RootModel
    {
        public override TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.FolderRootItem;

        public FolderRootModel(ITreeItem? parent)
            :base(parent, "")
        {
            DisplayName = "Folders";
            AllowedChildTypes = [TreeItemTypeEnum.FolderItem, TreeItemTypeEnum.EventItem];
        }




    }

}
