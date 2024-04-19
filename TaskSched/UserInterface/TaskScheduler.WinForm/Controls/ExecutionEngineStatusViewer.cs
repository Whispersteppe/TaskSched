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
    public partial class ExecutionEngineStatusViewer : UserControl
    {
        ScheduleManager _scheduleManager;
        ExecutionEngineStatusModel _item;

        public ExecutionEngineStatusViewer(ScheduleManager scheduleManager, ExecutionEngineStatusModel item)
        {
            InitializeComponent();

            _scheduleManager = scheduleManager;
            _item = item;
        }
    }
}
