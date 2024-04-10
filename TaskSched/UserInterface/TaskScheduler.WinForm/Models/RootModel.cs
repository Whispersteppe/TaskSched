using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler.WinForm.Models
{

    public class RootModel: ITreeItem
    {
        public virtual string DisplayName { get; protected set; }
        public virtual object? UnderlyingItem { get; set; }

        public virtual TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.Unknown;
        public Guid? ParentId { get => null; }

        public List<TreeItemTypeEnum> AllowedMoveToParentTypes { get; protected set; } = new List<TreeItemTypeEnum>();

        public List<TreeItemTypeEnum> AllowedChildTypes { get; protected set; } = new List<TreeItemTypeEnum>();

        public List<ITreeItem> Children {get;protected set; } = new List<ITreeItem>();

        public virtual Guid? ID {get;protected set; }


        public RootModel(ITreeItem? parentItem, object underlyingItem)
        {
            UnderlyingItem = underlyingItem;

            if (string.IsNullOrEmpty(DisplayName))
            {
                DisplayName = "Not Set";
            }
        }

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
            return false;
        }
    }

}
