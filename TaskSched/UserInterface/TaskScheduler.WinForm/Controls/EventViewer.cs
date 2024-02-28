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
    public partial class EventViewer : UserControl, ICanvasItem<EventModel>
    {
        ScheduleManager? _scheduleManager;
        EventModel _eventModel;

        public EventViewer()
        {
            InitializeComponent();
        }

        public List<ToolStripItem> ToolStripItems
        {
            get
            {
                ToolStripBuilder builder = new ToolStripBuilder();
                builder.AddButton("Save", TsSave_Click);
                builder.AddButton("Delete", TsDelete_Click);

                return builder.ToolStripItems;
            }
        }

        private async void TsDelete_Click(object? sender, EventArgs e)
        {
            await _scheduleManager.DeleteItem(_eventModel);
        }

        private async void TsSave_Click(object? sender, EventArgs e)
        {
            _eventModel.Item.Name = txtName.Text;
            _eventModel.Item.CalendarId = _eventModel.ParentItem.ID;
            var rslt = await _scheduleManager.SaveModel(_eventModel.ParentItem, _eventModel);
        }

        public async Task Initialize(ScheduleManager scheduleManager, EventModel o)
        {
            _scheduleManager = scheduleManager;

            TreeItem = o;
            _eventModel = o;

            this.txtName.Text = o.Name;
            this.lblLastExecution.Text = o.LastExecutionDate.ToString();
            this.lbNextExecution.Text = o.NextExecutionDate.ToString();

        }

        public async Task Initialize(ScheduleManager scheduleManager, object treeItem)
        {
            await Initialize(scheduleManager, treeItem as EventModel);
        }


        public ITreeItem? TreeItem { get; private set; }

        public bool CanClose()
        {
            return true;
        }

    }
}
