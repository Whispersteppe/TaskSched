using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm.Models
{
    public class SchedulerStatusModel : ITreeItem
    {
        public string DisplayName
        {
            get
            {
                return Name;
            }
        }

        ScheduleManager _manager;
        bool _showActive;
        public SchedulerStatusModel(ScheduleManager manager, string name, bool showActive)
        {
            _manager = manager;
            _showActive = showActive;
            Name = name;
        }

        public string Name { get; set; }
        #region ITreeItem stuff

        public Guid? ID => Guid.Empty;

        public Guid? ParentId { get => null; }

        public TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.SchedulerStatusItem;

        public List<TreeItemTypeEnum> AllowedMoveToParentTypes => [];

        public List<TreeItemTypeEnum> AllowedChildTypes => [];

        public List<ITreeItem> Children => [];


        public event PropertyChangedEventHandler? PropertyChanged;

        public bool CanAddCreateChild(TreeItemTypeEnum itemType)
        {
            return false;
        }

        public bool CanAddItem(ITreeItem possibleNewChild)
        {
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

        public bool CanHaveChildren()
        {
            return false;
        }

        public bool CanMoveItem(ITreeItem possibleNewParent)
        {
            return false;
        }

        public bool ContainsText(string text)
        {
            return true;
        }

        #endregion

        public async Task<List<ScheduleStatusItem>> StatusItems()
        {
            var scheduleItems = await _manager.GetScheduleItems(_showActive);
            return scheduleItems;

        }

        public ContextMenuStrip? GetContextMenu()
        {

            return null;

        }
    }

    public class ScheduleStatusItem
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public DateTime NextExecution { get; set; }
        public DateTime PreviousExecution { get; set; }
        public bool IsActive { get; set; }
    }
}
