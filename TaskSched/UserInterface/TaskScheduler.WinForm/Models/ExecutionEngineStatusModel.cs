using System.ComponentModel;

namespace TaskScheduler.WinForm.Models
{
    public class ExecutionEngineStatusModel : ITreeItem
    {
        public string DisplayName => "Execution Status";

        public Guid? ID => Guid.Empty;

        public Guid? ParentId { get => null; }

        public TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.ExecutionEngineStatusItem;

        public List<TreeItemTypeEnum> AllowedMoveToParentTypes => [];

        public List<TreeItemTypeEnum> AllowedChildTypes => [];

        public List<ITreeItem> Children => [];

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool CanAddCreateChild(TreeItemTypeEnum itemType)
        {
            return false;
        }

        public bool CanAddItem(ITreeItem possibleNewChild)
        {
            return false;
        }

        public bool CanDeleteItem()
        {
            return false;
        }

        public bool CanEdit()
        {
            return false;
        }

        public bool CanHaveChildren()
        {
            return false;
        }

        public bool CanMoveItem(ITreeItem possibleNewParent)
        {
            return false;
        }

        public bool ContainsText(string text)
        {
            return true;
        }
    }
}
