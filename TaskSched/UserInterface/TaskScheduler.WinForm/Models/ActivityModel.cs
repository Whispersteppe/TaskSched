﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskSched.Common.DataModel;
using TaskSched.Common.FieldValidator;

namespace TaskScheduler.WinForm.Models
{

    public class ActivityModel : Activity, ITreeItem
    {

        public string DisplayName => this.Name;
        public virtual Guid ID => this.Id;
        public ActivityModel()
        {
        }


        public  TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.ActivityItem;
        public ITreeItem? ParentItem { get; set; }


        public List<TreeItemTypeEnum> AllowedMoveToParentTypes { get; protected set; } = [];

        public List<TreeItemTypeEnum> AllowedChildTypes { get; protected set; } = [];

        public List<ITreeItem> Children { get; protected set; } = new List<ITreeItem>();





        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public bool CanMoveItem(ITreeItem possibleNewParent)
        {
            if (CanAddCreateChild(possibleNewParent.TreeItemType))
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


    }

    //public class ActivityFieldModel : ActivityField
    //{

    //}

    //public class EventActivityModel : EventActivity
    //{

    //}

    //public class EventActivityFieldModel : EventActivityField
    //{

    //}

    //public class EventScheduleModel : EventSchedule
    //{
    //}
}
