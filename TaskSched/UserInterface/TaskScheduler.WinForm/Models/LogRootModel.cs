using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TaskScheduler.WinForm.Models
{

    public class LogRootModel : RootModel
    {
        public override TreeItemTypeEnum TreeItemType => TreeItemTypeEnum.LogRootItem;

        public LogRootModel(ITreeItem parent)
            :base(parent, "")
        {
            DisplayName = "Logs";
        }


    }

}
