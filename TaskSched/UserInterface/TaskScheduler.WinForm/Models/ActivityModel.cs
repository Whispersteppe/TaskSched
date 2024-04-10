using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskSched.Common.DataModel;
using TaskSched.Common.FieldValidator;
using TaskSched.Common.Interfaces;
using TaskScheduler.WinForm.Controls.PropertyGridHelper;

namespace TaskScheduler.WinForm.Models
{

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
        [Category("Fields")]
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
    }

    public class ActivityFieldModel : INotifyPropertyChanged
    {

        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public Guid Id { get; set; }

        [ReadOnly(false)]
        [Browsable(true)]
        [Category("ID")]
        public string Name 
        { 
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        string fieldValue = "";

        [ReadOnly(false)]
        [Browsable(true)]
        [Category("ID")]
        public string Value 
        { 
            get => fieldValue;
            set
            {
                fieldValue = value;
                OnPropertyChanged();
            }
        }

        [ReadOnly(false)]
        [Browsable(true)]
        [Category("ID")]
        public FieldTypeEnum FieldType 
        { 
            get => fieldType;
            set
            {
                fieldType = value;
                OnPropertyChanged();
            }
        }


        [ReadOnly(false)]
        [Browsable(true)]
        [Category("ID")]
        public bool IsReadOnly { get; set; } //  read only makes it fixed for the activity, such as an executable path.

        public event PropertyChangedEventHandler? PropertyChanged;

        private bool instanceChanged;
        private string name;
        private FieldTypeEnum fieldType;

        [ReadOnly(true)]
        [Browsable(false)]
        public bool InstanceChanged
        {
            get => instanceChanged;
            set => instanceChanged = value;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            InstanceChanged = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }



        public override string ToString()
        {
            return Name;
        }

    }

}
