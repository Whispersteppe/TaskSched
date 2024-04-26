using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskSched.Common.DataModel;
using TaskSched.Common.Interfaces;
using TaskScheduler.WinForm.Controls.PropertyGridHelper;

namespace TaskScheduler.WinForm.Models
{

    [TypeConverter(typeof(ActivityModelConverter))]
    public class ActivityModel : ITreeItem, INotifyPropertyChanged
    {
        private Guid activityHandlerId;
        private bool instanceChanged;
        private string name;

        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public Guid Id { get; set; }

        [ReadOnly(false)]
        [Browsable(false)]
        [Category("ID")]
        public Guid ActivityHandlerId 
        { 
            get => activityHandlerId; 
            set 
            { 
                activityHandlerId = value;
                OnPropertyChanged();
            }
        }

        [ReadOnly(false)]
        [Browsable(true)]
        [Description("the name of the current activity execution handler")]
        [DisplayName("Execution Handler")]
        [TypeConverter(typeof(ExecutionHandlerInfoConverter))]
        public string ExecutionHandlerName
        { 
            get
            {
                var items = ScheduleManager.GlobalInstance.GetExecutionHandlerInfo().Result;
                var selectedItem = items.FirstOrDefault(x=>x.HandlerId == ActivityHandlerId);
                return selectedItem.Name;
            }
            set
            {
                var items = ScheduleManager.GlobalInstance.GetExecutionHandlerInfo().Result;
                var selectedItem = items.FirstOrDefault(x => x.Name == value);
                ActivityHandlerId = selectedItem.HandlerId;
                OnPropertyChanged();
            }
        }

        [ReadOnly(false)]
        [Browsable(true)]
        [Description("the name of the activity")]
        [Category("ID")]
        [DefaultValue("New Activity")]
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
        [DisplayName("New Fields")]
        [Category("Fields")]
        [Description("the set of new fields associated with this activity")]
        public List<ActivityFieldModel>? Fields 
        { 
            get
            {
                return DefaultFields.Where(x => x.IsReadOnly == false).ToList();
            }
        }



        [ReadOnly(false)]
        [Browsable(false)]
        public ObservableCollection<ActivityFieldModel>? DefaultFields { get; set; } = new ObservableCollection<ActivityFieldModel>();

        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public Guid? ParentId { get => null; }


        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public string DisplayName => this.Name;

        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public virtual Guid? ID => this.Id;


        public ActivityModel()
        {
        }

  
        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]

        public TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.ActivityItem;
        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public List<TreeItemTypeEnum> AllowedMoveToParentTypes { get; protected set; } = [];

        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public List<TreeItemTypeEnum> AllowedChildTypes { get; protected set; } = [];

        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public List<ITreeItem> Children { get; protected set; } = new List<ITreeItem>();

        public event PropertyChangedEventHandler? PropertyChanged;

        [ReadOnly(true)]
        [Browsable(false)]
        public bool InstanceChanged
        {
            get
            {
                if (instanceChanged == true) return true;
                foreach (var item in DefaultFields)
                {
                    if (item.InstanceChanged == true) return true;
                }
                return false;
            }

            set
            {
                instanceChanged = value;
                foreach(var field in DefaultFields)
                {
                    field.InstanceChanged = value;
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            InstanceChanged = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public bool CanMoveItem(ITreeItem possibleNewParent)
        {
            if (possibleNewParent.CanAddCreateChild(TreeItemType))
            {
                return true;
            }

            return false;
        }

        public bool CanAddItem(ITreeItem possibleNewChild)
        {
            if (CanAddCreateChild(possibleNewChild.TreeItemType))
            {
                return true;
            }

            return false;
        }

        public bool CanHaveChildren()
        {
            if (AllowedChildTypes.Count > 0)
            {
                return true;
            }

            return false;
        }

        public bool CanAddCreateChild(TreeItemTypeEnum itemType)
        {
            if (AllowedChildTypes.Contains(itemType))
            {
                return true;
            }

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


        public bool ContainsText(string text)
        {
            if (this.DisplayName.Contains(text, StringComparison.InvariantCultureIgnoreCase)) return true;
            if (this.Name.Contains(text, StringComparison.InvariantCultureIgnoreCase)) return true;
            foreach(var field in this.DefaultFields)
            {
                if (field.Name.Contains(text, StringComparison.InvariantCultureIgnoreCase)) return true;
                if (field.Value.Contains(text, StringComparison.InvariantCultureIgnoreCase)) return true;
            }
            return false;
        }

        public ContextMenuStrip? GetContextMenu()
        {
            ContextMenuStrip menu = menu = new ContextMenuStrip();
            menu.Items.AddRange((ToolStripItem[])[
                new ToolStripMenuItem("Save", null, MenuItem_Save_Click),
                new ToolStripMenuItem("Delete", null, MenuItem_Delete_Click),
                new ToolStripMenuItem("Add New", null, MenuItem_AddNew_Click),
            ]);

            return menu;
        }

        private async void MenuItem_Save_Click(object? sender, EventArgs e)
        {
            await ScheduleManager.GlobalInstance.SaveModel(this);
        }
        private async void MenuItem_Delete_Click(object? sender, EventArgs e)
        {
            await ScheduleManager.GlobalInstance.DeleteItem(this);
        }
        private async void MenuItem_AddNew_Click(object? sender, EventArgs e)
        {
            await ScheduleManager.GlobalInstance.CreateModel(null, TreeItemTypeEnum.ActivityItem);
        }


    }

}
