using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskSched.Common.DataModel;
using TaskSched.Component.Cron;
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm.Controls
{
    public partial class EventViewer : UserControl, ICanvasItem<EventModel>
    {
        readonly ScheduleManager _scheduleManager;
        readonly EventModel _eventModel;

        bool _modelChanged;

        public EventViewer(ScheduleManager scheduleManager, EventModel item)
        {
            InitializeComponent();

            _scheduleManager = scheduleManager;

            TreeItem = item;
            _eventModel = item;
            _eventModel.InstanceChanged = false;

            grdEventProperties.SelectedObject = item;



            _modelChanged = false;
        }

        public List<ToolStripItem> ToolStripItems
        {
            get
            {
                ToolStripBuilder builder = new ToolStripBuilder();
                builder.AddButton("Save", TsSave_Click);
                builder.AddButton("Delete", TsDelete_Click);
                builder.AddButton("Launch", TsLaunch_Click);

                return builder.ToolStripItems;
            }
        }

        private async void TsLaunch_Click(object? sender, EventArgs e)
        {
            if (_scheduleManager != null)
            {
                await _scheduleManager.LaunchEvent(_eventModel);
            }

        }

        private async void TsDelete_Click(object? sender, EventArgs e)
        {
            if (_scheduleManager != null)
            {
                await _scheduleManager.DeleteItem(_eventModel);
            }
        }

        private async void TsSave_Click(object? sender, EventArgs e)
        {
            _modelChanged = true;
            await Save();


        }




        public ITreeItem? TreeItem { get; private set; }

        public async Task LeavingItem()
        {
            await Save();
        }




        private async Task Save()
        {
            if (_eventModel.InstanceChanged == true)
            {
                _modelChanged = true;
            }


            if (_modelChanged == true)
            {
                if (_scheduleManager != null)
                {
                    var rslt = await _scheduleManager.SaveModel(_eventModel);
                }
            }

        }

        private async void btnScheduleSave_Click(object sender, EventArgs e)
        {
            await Save();
        }

        private void grdEventProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            _modelChanged = true;
        }
    }
}
