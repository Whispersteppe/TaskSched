using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaskScheduler.WinForm.Configuration;

namespace TaskScheduler.WinForm.Models
{

    public class ConfigurationRootModel : RootModel
    {
        public override TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.StatusRootItem;

        public ConfigurationRootModel(ITreeItem? parent)
            : base(parent, "")
        {
            DisplayName = "Configuration";

        }

        public override bool CanHaveChildren()
        {
            return true;
        }

    }


    public class ConfigurationModel : ITreeItem, INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler? PropertyChanged;

        public ScheduleManagerConfig? ScheduleManagerConfiguration { get; set; }

        [ReadOnly(true)]
        [Browsable(false)]
        public bool InstanceChanged { get; set; }

        public string DisplayName => "Configuration";

        public Guid? ID => Guid.Empty;

        public Guid? ParentId => null;

        public TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.ConfigItem;

        public List<TreeItemTypeEnum> AllowedMoveToParentTypes => [];

        public List<TreeItemTypeEnum> AllowedChildTypes => [];

        public List<ITreeItem> Children => [];

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            InstanceChanged = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public bool CanMoveItem(ITreeItem possibleNewParent)
        {
            return false;
        }

        public bool CanAddItem(ITreeItem possibleNewChild)
        {
            return false;
        }

        public bool CanHaveChildren()
        {
            return false;
        }

        public bool CanAddCreateChild(TreeItemTypeEnum itemType)
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

        public bool ContainsText(string text)
        {
            return false;
        }
    }
}
