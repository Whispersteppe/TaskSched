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

    public class BaseTreeItemModel: ITreeItem
    {
        public virtual string Name { get; protected set; }
        public virtual object? UnderlyingItem { get; set; }

        public virtual TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.Unknown;
        public ITreeItem? ParentItem {get;private set;}


        public List<TreeItemTypeEnum> AllowedMoveToParentTypes { get; protected set; } = new List<TreeItemTypeEnum>();

        public List<TreeItemTypeEnum> AllowedChildTypes { get; protected set; } = new List<TreeItemTypeEnum>();

        public List<ITreeItem> Children {get;protected set; } = new List<ITreeItem>();

        public virtual Guid ID {get;protected set; }


        public BaseTreeItemModel(ITreeItem? parentItem, object underlyingItem)
        {
            UnderlyingItem = underlyingItem;
            ParentItem = parentItem;
            if (string.IsNullOrEmpty(Name))
            {
                Name = "Not Set";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public virtual bool CanMoveItem(ITreeItem possibleNewParent)
        {
            if (CanAddCreateChild(possibleNewParent.TreeItemType))
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


    }

    public class BaseTreeItemModel<T> : BaseTreeItemModel where T: class
    {
        public T? Item
        {
            get 
            {
                return UnderlyingItem as T;
            }
            protected set
            {
                UnderlyingItem = value;
            }
        }

        public BaseTreeItemModel(T item, ITreeItem? parent)
            :base(parent, item)
        {
        }

    }
}
