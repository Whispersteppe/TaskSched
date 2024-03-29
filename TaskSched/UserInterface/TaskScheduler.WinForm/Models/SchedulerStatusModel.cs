using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm.Models
{
    internal class SchedulerStatusModel : ITreeItem
    {
        public string DisplayName => "scheduler Status";

        public Guid? ID => Guid.Empty;

        public ITreeItem? ParentItem => null;

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
    }

    public class ExecutionEngineStatusModel : ITreeItem
    {
        public string DisplayName => "Execution Status";

        public Guid? ID => Guid.Empty;

        public ITreeItem? ParentItem => null;

        public TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.ExecutionEngineStatusItem;

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
    }
}
