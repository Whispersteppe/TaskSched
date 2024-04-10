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
        readonly ActivityModel _activityModel;
        readonly ScheduleManager? _scheduleManager;

        bool _modelChanged;


        public ActivityViewer(ScheduleManager scheduleManager, ActivityModel item)
        {
            InitializeComponent();

            _scheduleManager = scheduleManager;
            _activityModel = item;
            _activityModel.InstanceChanged = false;
            TreeItem = item;

            grdActivityProperties.SelectedObject = _activityModel;

            _modelChanged = false;
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
            if (_scheduleManager != null)
            {
                if (await _scheduleManager.CanDeleteItem(_activityModel))
                {
                    await _scheduleManager.DeleteItem(_activityModel);
                }
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

            if (_activityModel.InstanceChanged == true)
            {
                _modelChanged = true;
            }



            if (_modelChanged == true)
            {
                if (_scheduleManager != null)
                {

                    await _scheduleManager.SaveModel(_activityModel);
                }
            }

        }


        public async Task LeavingItem()
        {
            await Save();
        }

    }
}
