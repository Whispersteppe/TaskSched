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
            _eventModel.Name = txtName.Text;
            _eventModel.CalendarId = _eventModel.ParentItem.ID;
            _eventModel.CatchUpOnStartup = cbCatchUpOnStartup.Checked;
            _eventModel.IsActive = cbIsActive.Checked;

            //  activities
            //  schedules
            //  add the new ones
            foreach (var scheduleItem in lstScheduleItems.Items)
            {
                if (scheduleItem is EventSchedule eventSchedule)
                {
                    if (_eventModel.Schedules.Contains(eventSchedule) == false)
                    {
                        _eventModel.Schedules.Add(eventSchedule);
                    }
                }
            }
            List<EventSchedule> deleteSchedules = new List<EventSchedule>();
            //  remove the old ones
            foreach (var scheduleItem in _eventModel.Schedules)
            {
                if (lstScheduleItems.Items.Contains(scheduleItem) == false)
                {
                    deleteSchedules.Add(scheduleItem);
                }
            }
            foreach (var schedule in deleteSchedules)
            {
                _eventModel.Schedules.Remove(schedule);
            }

            var rslt = await _scheduleManager.SaveModel(_eventModel.ParentItem, _eventModel);
        }

        public async Task Initialize(ScheduleManager scheduleManager, EventModel o)
        {
            _scheduleManager = scheduleManager;

            TreeItem = o;
            _eventModel = o;

            txtName.Text = o.DisplayName;
            lblLastExecution.Text = o.LastExecution.ToString();
            lbNextExecution.Text = o.NextExecution.ToString();
            cbCatchUpOnStartup.Checked = o.CatchUpOnStartup;
            cbIsActive.Checked = o.IsActive;

            lstScheduleItems.Items.Clear();

            lstActivities.Items.Clear();

            lstActivityFields.Items.Clear();

            foreach (var schedule in o.Schedules)
            {
                lstScheduleItems.Items.Add(schedule);
            }

            lstScheduleItems.SelectedIndex = 0;

            foreach (var activity in o.Activities)
            {
                lstActivities.Items.Add(activity);
            }

            lstActivities.SelectedIndex = 0;


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

        private void txtScheduleCRON_TextChanged(object sender, EventArgs e)
        {

        }

        private void lstScheduleItems_SelectedIndexChanged(object sender, EventArgs e)
        {

            SaveSchedule();

            if (lstScheduleItems.SelectedItem != null)
            {
                if (lstScheduleItems.SelectedItem is EventSchedule eventSchedule)
                {
                    cronValue = new CronValue(eventSchedule.CRONData);

                    txtScheduleCRON.Text = eventSchedule.CRONData;
                    txtScheduleName.Text = eventSchedule.Name;

                    cronPieceHours.Initialize(cronValue.Hours);
                    cronPieceMinutes.Initialize(cronValue.Minutes);
                    _currentSchedule = eventSchedule;
                }
            }
        }

        private void lstActivities_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveActivity();
            _currentActivity = null;

            if (lstActivities.SelectedItem != null)
            {
                if (lstActivities.SelectedItem is EventActivity activity)
                {
                    lstActivityFields.Items.Clear();
                    foreach (var field in activity.Fields)
                    {
                        lstActivityFields.Items.Add(field);
                    }
                    if (lstActivityFields.Items.Count > 0)
                    {
                        lstActivityFields.SelectedIndex = 0;
                    }

                    _currentActivity = activity;
                }
            }
        }

        private void SaveSchedule()
        {
            if (_currentSchedule != null)
            {

                _currentSchedule.CRONData = txtScheduleCRON.Text;
                _currentSchedule.Name = txtScheduleCRON.Text;
                txtScheduleName.Text = _currentSchedule.Name;

                //TODO once the pieces are up and running, we'll want to get the data from there.
            }
        }

        private void SaveActivity()
        {
            if (_currentActivity != null)
            {
                if (_currentActivityField != null)
                {

                    _currentActivityField.Value = txtFieldValue.Text;

                }
            }
        }

        private void lstActivityFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveActivity();

            if (lstActivityFields.SelectedItem != null) 
            {
                if (lstActivityFields.SelectedItem is EventActivityField field)
                {
                    txtFieldValue.Text = field.Value;
                    txtFieldName.Text = field.Name;

                    _currentActivityField = field;

                }
            }

        }


        private async Task Save()
        {
            if (_eventModel != null)
            {

                _eventModel.IsActive = cbIsActive.Checked;
                _eventModel.CatchUpOnStartup = cbCatchUpOnStartup.Checked;

                SaveSchedule();
                SaveActivity();

                await _scheduleManager.SaveModel(_eventModel.ParentItem, _eventModel);
            }
        }

        private async void btnScheduleSave_Click(object sender, EventArgs e)
        {
            await Save();
        }
    }
}
