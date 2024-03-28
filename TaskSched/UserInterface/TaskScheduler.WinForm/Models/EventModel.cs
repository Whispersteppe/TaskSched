using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskSched.Common.DataModel;

namespace TaskScheduler.WinForm.Models
{
    public class EventModel : Event, ITreeItem
    {
        public TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.EventItem;


        public string DisplayName => this.Name;
        public virtual Guid ID => this.Id;
        public virtual object? UnderlyingItem { get; set; }

        public ITreeItem? ParentItem { get; set; }


        public List<TreeItemTypeEnum> AllowedMoveToParentTypes { get; protected set; } = [TreeItemTypeEnum.FolderRootItem, TreeItemTypeEnum.FolderItem];

        public List<TreeItemTypeEnum> AllowedChildTypes { get; protected set; } = [];

        public List<ITreeItem> Children { get; protected set; } = [];


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public virtual bool CanMoveItem(ITreeItem possibleNewParent)
        {
            if (possibleNewParent.CanAddCreateChild(TreeItemType))
            {
                return true;
            }

            return false;
        }

        public virtual bool CanAddItem(ITreeItem possibleNewChild)
        {
            if (CanAddCreateChild(possibleNewChild.TreeItemType))
            {
                return true;
            }

            return false;
        }

        public virtual bool CanHaveChildren()
        {
            if (AllowedChildTypes.Count > 0)
            {
                return true;
            }

            return false;
        }

        public virtual bool CanAddCreateChild(TreeItemTypeEnum itemType)
        {
            if (AllowedChildTypes.Contains(itemType))
            {
                return true;
            }

            return false;
        }

        public virtual bool CanDeleteItem()
        {
            return false;
        }

        public virtual bool CanEdit()
        {
            return false;
        }

        public bool ContainsText(string text)
        {
            if (this.Name.Contains(text, StringComparison.InvariantCultureIgnoreCase)) return true;
            foreach(var activity in this.Activities)
            {
                if (activity.Name.Contains(text, StringComparison.InvariantCultureIgnoreCase)) return true;
                foreach(var field in activity.Fields)
                {
                    if (field.Value.Contains(text, StringComparison.InvariantCultureIgnoreCase)) return true;
                }
            }

            return false;
        }

    }
}
