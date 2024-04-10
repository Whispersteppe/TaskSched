using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskSched.Common.DataModel;
using TaskSched.Common.FieldValidator;
using TaskSched.Common.Interfaces;
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm.Controls
{
    public partial class ActivityViewer : UserControl, ICanvasItem<ActivityModel>
    {

        public ITreeItem TreeItem { get; private set; }
        ActivityModel _activityModel;
        ActivityModel _activity;
        ScheduleManager? _scheduleManager;

        ActivityFieldModel _currentActivityField;
        bool _modelChanged;


        public ActivityViewer()
        {
            InitializeComponent();
        }

        //JObject _objectModel;

        public async Task Initialize(ScheduleManager scheduleManager, ActivityModel o)
        {

            //_objectModel = JObject.FromObject(o);

            _scheduleManager = scheduleManager;
            _activityModel = o;
            TreeItem = o;

            _activity = _activityModel;

            grdActivityProperties.SelectedObject = _activity;

            _modelChanged = false;

        }

        public async Task Initialize(ScheduleManager scheduleManager, object treeItem)
        {
            await Initialize(scheduleManager, treeItem as ActivityModel);
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
            if (await _scheduleManager.CanDeleteItem(_activityModel))
            {
                await _scheduleManager.DeleteItem(_activityModel);
            }
        }

        private async void TsSave_Click(object? sender, EventArgs e)
        {
            _modelChanged = true;
            await Save();
        }

        #region handlers

        #region field handlers


        #endregion

        #endregion

        public async Task Save()
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

                await _scheduleManager.SaveModel(_activityModel);
            }

        }


        public async Task LeavingItem()
        {
            await Save();
        }

    }
}
