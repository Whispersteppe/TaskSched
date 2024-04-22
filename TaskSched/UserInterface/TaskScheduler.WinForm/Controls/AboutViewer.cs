using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm.Controls
{
    public partial class AboutViewer : UserControl, ICanvasItem<AboutModel>
    {

        ScheduleManager _scheduleManager;
        AboutModel _item;


        public AboutViewer(ScheduleManager scheduleManager, AboutModel item)
        {
            InitializeComponent();
            _scheduleManager = scheduleManager;
            _item = item;

            gridAbout.SelectedObject = item;
        }

        public List<ToolStripItem> ToolStripItems => [];

        public ITreeItem? TreeItem => _item;

        public async Task LeavingItem()
        {
            //  nothing needs to be done.
        }
    }
}
