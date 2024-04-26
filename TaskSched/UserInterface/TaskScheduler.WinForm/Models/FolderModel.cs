using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskSched.Common.DataModel;

namespace TaskScheduler.WinForm.Models
{
    public class FolderModel : ITreeItem, INotifyPropertyChanged
    {
        private string name;
        string? _defaultSchedule;

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


        [ReadOnly(false)]
        [Browsable(true)]
        [Category("ID")]
        [Description("Cron data for the schedule - Sec Min Hour DayOfMonth Month DayOfWeek Year")]
        public string? DefaultSchedule
        {
            get => _defaultSchedule;
            set
            {
                _defaultSchedule = value;
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

        public ContextMenuStrip? GetContextMenu()
        {
            ContextMenuStrip menu = menu = new ContextMenuStrip();
            menu.Items.AddRange((ToolStripItem[])[
                new ToolStripMenuItem("Save", null, MenuItem_Save_Click),
                new ToolStripMenuItem("Delete", null, MenuItem_Delete_Click),
                new ToolStripMenuItem("Add Folder", null, MenuItem_AddNewFolder_Click),
                new ToolStripMenuItem("Add Event", null, MenuItem_AddNewEvent_Click),
            ]);

            return menu;
        }

        private void MenuItem_Save_Click(object? sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        private void MenuItem_Delete_Click(object? sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        private void MenuItem_AddNewFolder_Click(object? sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        private void MenuItem_AddNewEvent_Click(object? sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

    }

}
