﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskSched.Common.DataModel;
using TaskSched.Common.FieldValidator;
using TaskScheduler.WinForm.Controls.PropertyGridHelper;

namespace TaskScheduler.WinForm.Models
{
    public class EventModel : ITreeItem
    {

        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public Guid Id { get; set; }

        [ReadOnly(false)]
        [Browsable(true)]
        [Category("ID")]
        public string Name { get; set; } = "New Event";

        [ReadOnly(true)]
        [Browsable(true)]
        [Category("Dates")]
        public DateTime LastExecution { get; set; } = DateTime.MinValue;

        [ReadOnly(true)]
        [Browsable(true)]
        [Category("Dates")]
        public DateTime NextExecution { get; set; } = DateTime.MinValue;

        [ReadOnly(false)]
        [Browsable(true)]
        [Category("ID")]
        public bool IsActive { get; set; }

        [ReadOnly(false)]
        [Browsable(true)]
        [Category("ID")]
        public bool CatchUpOnStartup { get; set; }

        [ReadOnly(false)]
        [Browsable(false)]
        [Category("ID")]
        public Guid? FolderId { get; set; }


        [ReadOnly(false)]
        [Browsable(true)]
        [Category("Scheduling")]
        public List<EventScheduleModel>? Schedules { get; set; } = new List<EventScheduleModel>();

        [ReadOnly(false)]
        [Browsable(true)]
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

    public class EventScheduleModel
    {
        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]

        public Guid Id { get; set; }
        [ReadOnly(false)]
        [Browsable(true)]
        [Category("ID")]

        public string Name { get; set; }
        [ReadOnly(false)]
        [Browsable(true)]
        [Category("ID")]
        public string CRONData { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class EventActivityModel
    {

        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public Guid Id { get; set; }

        [ReadOnly(false)]
        [Browsable(true)]
        [Category("ID")]
        public string Name { get; set; }


        [ReadOnly(false)]
        [Browsable(false)]
        [Category("ID")]
//        [TypeConverter(typeof(ActivityIdConverter))]
        public Guid ActivityId { get; set; }


        [ReadOnly(false)]
        [Browsable(true)]
        [TypeConverter(typeof(ActivityConverter))]
        public string ActivityName
        {
            get
            {
                var items = ScheduleManager.GlobalInstance.GetActivities().Result;
                var selectedItem = items.FirstOrDefault(x => x.ID == ActivityId);
                return selectedItem.Name;
            }
            set
            {
                var items = ScheduleManager.GlobalInstance.GetActivities().Result;
                var selectedItem = items.FirstOrDefault(x => x.Name == value);
                ActivityId = selectedItem.ID.Value;

                //TODO will also need to reswizzle fields as needed
            }
        }


        [ReadOnly(false)]
        [Browsable(true)]
        [Category("ID")]
        public List<EventActivityFieldModel>? Fields { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

    public class EventActivityFieldModel
    {

        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]
        public Guid Id { get; set; }

        [ReadOnly(true)]
        [Browsable(true)]
        [Category("ID")]
        public string Name { get; set; }

        [ReadOnly(false)]
        [Browsable(true)]
        [Category("ID")]
        public string Value { get; set; }

        [ReadOnly(true)]
        [Browsable(true)]
        [Category("ID")]
        public FieldTypeEnum FieldType { get; set; }



        [ReadOnly(false)]
        [Browsable(false)]
        [Category("ID")]
        public Guid ActivityFieldId { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }
}
