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
        ScheduleManager? _scheduleManager;
        EventModel _eventModel;

        CronValue cronValue;
        EventActivity? _currentActivity;
        EventSchedule? _currentSchedule;
        EventActivityField? _currentActivityField;
        bool _modelChanged;

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
                builder.AddButton("Launch", TsLaunch_Click);

                return builder.ToolStripItems;
            }
        }

        private async void TsLaunch_Click(object? sender, EventArgs e)
        {
            await _scheduleManager.LaunchEvent(_eventModel);
        }

        private async void TsDelete_Click(object? sender, EventArgs e)
        {
            await _scheduleManager.DeleteItem(_eventModel);
        }

        private async void TsSave_Click(object? sender, EventArgs e)
        {
            _modelChanged = true;
            await Save();


        }

        //JObject _objectModel;

        public async Task Initialize(ScheduleManager scheduleManager, EventModel o)
        {

            //_objectModel = JObject.FromObject(o);

            _scheduleManager = scheduleManager;

            TreeItem = o;
            _eventModel = o;
            grdEventProperties.SelectedObject = o;



            _modelChanged = false;

        }

        public async Task Initialize(ScheduleManager scheduleManager, object treeItem)
        {
            await Initialize(scheduleManager, treeItem as EventModel);
        }


        public ITreeItem? TreeItem { get; private set; }

        public async Task LeavingItem()
        {
            await Save();
        }




        private async Task Save()
        {
            //JObject newModel = JObject.FromObject(_objectModel);
            //CompareLogic compareLogic = new CompareLogic();
            //ComparisonResult compareResult = compareLogic.Compare(_objectModel, newModel);
            //if (compareResult.AreEqual == false)
            //{
            //    _modelChanged = true;
            //}


            if (_modelChanged == true)
            {
                var rslt = await _scheduleManager.SaveModel(_eventModel);
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
