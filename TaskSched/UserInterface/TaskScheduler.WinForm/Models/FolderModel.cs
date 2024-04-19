using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskSched.Common.DataModel;

namespace TaskScheduler.WinForm.Models
{
    public class FolderModel : ITreeItem, INotifyPropertyChanged
    {
        private string name;

        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public Guid Id { get; set; }

        [ReadOnly(false)]
        [Browsable(true)]
        [Category("ID")]
        [Description("The name of the folder")]
        [DefaultValue("New Folder")]
        public string Name
        {
            get => name;
            set 
            { 
                name = value;
                OnPropertyChanged();
            }
        }

        [ReadOnly(true)]
        [Browsable(false)]
        public Guid? ParentFolderId { get; set; }

        [ReadOnly(true)]
        [Browsable(false)]
        public List<EventModel>? Events { get; set; } = new List<EventModel>();
        [ReadOnly(true)]
        [Browsable(false)]
        public List<FolderModel>? ChildFolders { get; set; } = new List<FolderModel>();

        [ReadOnly(true)]
        [Browsable(false)]
        public Guid? ParentId { get => ParentFolderId; }


        [ReadOnly(true)]
        [Browsable(false)]
        public string DisplayName => this.Name;
        [ReadOnly(true)]
        [Browsable(false)]
        public virtual Guid? ID => this.Id;

        [ReadOnly(true)]
        [Browsable(false)]
        public virtual object? UnderlyingItem { get; set; }

        [ReadOnly(true)]
        [Browsable(false)]
        public virtual TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.FolderItem;

        [ReadOnly(true)]
        [Browsable(false)]
        public List<TreeItemTypeEnum> AllowedMoveToParentTypes { get; protected set; } = [TreeItemTypeEnum.FolderItem, TreeItemTypeEnum.FolderRootItem];

        [ReadOnly(true)]
        [Browsable(false)]
        public List<TreeItemTypeEnum> AllowedChildTypes { get; protected set; } = [TreeItemTypeEnum.FolderItem, TreeItemTypeEnum.EventItem];

        [ReadOnly(true)]
        [Browsable(false)]
        public List<ITreeItem> Children 
        {
            get
            {
                List<ITreeItem> children = new List<ITreeItem>();

                foreach (var childFolder in ChildFolders)
                {
                    if (childFolder is FolderModel model)
                    {
                        children.Add(model);
                    }
                }

                foreach (var childEvent in Events)
                {
                    if (childEvent is EventModel model)
                    {
                        children.Add(model);
                    }
                }

                return children;


            }
            protected set
            {

            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        [ReadOnly(true)]
        [Browsable(false)]
        public bool InstanceChanged { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            InstanceChanged = true;
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
            if (DisplayName.Contains(text, StringComparison.InvariantCultureIgnoreCase)) return true;

            return false;
        }

    }

}
