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



        public ScheduleManagerConfig? ScheduleManagerConfiguration { get; set; }

        [ReadOnly(true)]
        [Browsable(false)]
        public bool InstanceChanged { get; set; }

    }
}
