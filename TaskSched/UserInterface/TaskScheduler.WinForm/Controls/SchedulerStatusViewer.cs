﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskScheduler.WinForm.Controls
{
    public partial class SchedulerStatusViewer : UserControl
    {

        public SchedulerStatusViewer(ScheduleManager scheduleManager, object item)
        {
            InitializeComponent();

        }
    }
}
