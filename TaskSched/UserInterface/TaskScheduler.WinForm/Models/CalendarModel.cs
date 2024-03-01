using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskSched.Common.DataModel;

namespace TaskScheduler.WinForm.Models
{
    public class CalendarModel : Calendar, ITreeItem
    {

        public string DisplayName => this.Name;
        public virtual Guid ID => this.Id;

        public virtual object? UnderlyingItem { get; set; }

        public virtual TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.CalendarItem;
        public ITreeItem? ParentItem { get; set; }


        public List<TreeItemTypeEnum> AllowedMoveToParentTypes { get; protected set; } = [TreeItemTypeEnum.CalendarItem, TreeItemTypeEnum.CalendarRootItem];

        public List<TreeItemTypeEnum> AllowedChildTypes { get; protected set; } = [TreeItemTypeEnum.CalendarItem, TreeItemTypeEnum.EventItem];

        public List<ITreeItem> Children 
        {
            get
            {
                List<ITreeItem> children = new List<ITreeItem>();

                foreach (var childCalendar in ChildCalendars)
                {
                    if (childCalendar is CalendarModel model)
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

}
