using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskSched.Common.DataModel;
using TaskSched.Common.FieldValidator;
using TaskScheduler.WinForm.Controls.PropertyGridHelper;

namespace TaskScheduler.WinForm.Models
{
    public class EventModel : ITreeItem, INotifyPropertyChanged
    {
        private string name;
        private bool instanceChanged;
        private bool isActive;
        private bool catchUpOnStartup;

        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public Guid Id { get; set; }

        [ReadOnly(false)]
        [Browsable(true)]
        [Description("Name of the event")]
        [Category("ID")]
        [DefaultValue("New Event")]
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
        [Browsable(true)]
        [Description("When the last execution was done")]
        [Category("Dates")]
        [DisplayName("Last Execution")]
        public DateTime LastExecution { get; set; } = DateTime.MinValue;

        [ReadOnly(true)]
        [Browsable(true)]
        [Category("Dates")]
        [Description("When the next execution is expected")]
        [DisplayName("Next Execution")]
        public DateTime NextExecution { get; set; } = DateTime.MinValue;

        [ReadOnly(false)]
        [Browsable(true)]
        [DisplayName("Is Active")]
        [Description("Indicates that this event is active and can be triggered")]
        [Category("ID")]
        public bool IsActive
        { 
            get => isActive;
            set
            {
                isActive = value;
                OnPropertyChanged();
            }
        }

        [ReadOnly(false)]
        [Browsable(true)]
        [Description("If the application is started, and the next execution date is in the past, this event will be triggered")]
        [DisplayName("Catch up on startup")]
        [Category("ID")]
        public bool CatchUpOnStartup 
        { 
            get => catchUpOnStartup;
            set
            {
                catchUpOnStartup = value;
                OnPropertyChanged();
            }
        }

        [ReadOnly(false)]
        [Browsable(false)]
        [Category("ID")]
        public Guid? FolderId { get; set; }


        [ReadOnly(false)]
        [Browsable(true)]
        [Description("The set of scheduling items used for determining the next time this event will be triggered")]
        [Category("Scheduling")]
        public List<EventScheduleModel>? Schedules { get; set; } = new List<EventScheduleModel>();

        [ReadOnly(false)]
        [Browsable(true)]
        [Description("The list of activities that will be performed when this event is triggered")]
        [Category("Activities")]
        public List<EventActivityModel>? Activities { get; set; } = new List<EventActivityModel>();


        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public Guid? ParentId { get => FolderId; }


        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.EventItem;


        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public string DisplayName => this.Name;
        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public virtual Guid? ID => this.Id;
        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public virtual object? UnderlyingItem { get; set; }


        [ReadOnly(true)]
        [Browsable(false)]
        public List<TreeItemTypeEnum> AllowedMoveToParentTypes { get; protected set; } = [TreeItemTypeEnum.FolderRootItem, TreeItemTypeEnum.FolderItem];

        [ReadOnly(true)]
        [Browsable(false)]
        public List<TreeItemTypeEnum> AllowedChildTypes { get; protected set; } = [];

        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public List<ITreeItem> Children { get; protected set; } = [];


        public event PropertyChangedEventHandler? PropertyChanged;

        [ReadOnly(true)]
        [Browsable(false)]
        public bool InstanceChanged
        {
            get
            {
                if (instanceChanged == true) return true;

                foreach (var schedule in Schedules)
                {
                    if (schedule.InstanceChanged == true) return true;
                }

                foreach(var activity in Activities)
                {
                    if (activity.InstanceChanged == true) return true;
                }

                return false;
            }
            set
            {
                instanceChanged = value;
                foreach(var schedule in Schedules)
                {
                    schedule.InstanceChanged = value;
                }
                foreach(var activity in Activities)
                {
                    activity.InstanceChanged = value;
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            InstanceChanged = true;
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
