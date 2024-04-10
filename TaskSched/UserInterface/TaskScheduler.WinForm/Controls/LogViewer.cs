using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskScheduler.WinForm.Models;
using TaskScheduler.WinForm.NLogCustom;

namespace TaskScheduler.WinForm.Controls
{
    public partial class LogViewer : UserControl, ICanvasItem<LogViewModel>
    {

        readonly ScheduleManager _scheduleManager;
        readonly LogViewModel _logViewModel;
        public List<ToolStripItem> ToolStripItems => [];
        public ITreeItem? TreeItem => throw new NotImplementedException();

        public LogViewer(ScheduleManager scheduleManager, LogViewModel item)
        {
            InitializeComponent();


            _scheduleManager = scheduleManager;
            _logViewModel = item;
            _logViewModel.OnLogMessage += logViewModel_OnLogMessage;

            // load the current message set
            _logViewModel.LogItems.ToList().ForEach(logViewModel_OnLogMessage);

        }

        private void logViewModel_OnLogMessage(NLogCustom.LogEventInfoExtended logEventInfo)
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    logViewModel_OnLogMessage(logEventInfo);
                }));

                return;
            }

            ListViewItem logItem = new ListViewItem(logEventInfo.Extended.Level.ToString());
            logItem.Tag = logEventInfo;

            logItem.SubItems.Add(logEventInfo.Extended.TimeStamp.ToString());
            //            logItem.SubItems.Add(logEventInfo.Extended.LoggerName);
            logItem.SubItems.Add(logEventInfo.Extended.FormattedMessage);

            lvLogMessage.Items.Insert(0, logItem);

            foreach (ColumnHeader col in lvLogMessage.Columns)
            {
                Size t = TextRenderer.MeasureText(logItem.SubItems[col.Index].Text, logItem.SubItems[col.Index].Font);
                if (t.Width + 8 > col.Width)
                {
                    col.Width = t.Width + 8;
                }
            }

            Debug.WriteLine(logEventInfo.Extended.ToString());

            while (lvLogMessage.Items.Count > 500)
            {
                lvLogMessage.Items.RemoveAt(500);
            }
        }

        public async Task LeavingItem()
        {
            await Task.Run(() => { });

            _logViewModel.OnLogMessage -= logViewModel_OnLogMessage;
        }

        private void lvLogMessage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvLogMessage.SelectedItems.Count > 0)
            {
                if (lvLogMessage.SelectedItems[0].Tag is LogEventInfoExtended logEvent) 
                {
                    logDetails.SelectedObject = logEvent;
                }
            }
        }
    }
}
