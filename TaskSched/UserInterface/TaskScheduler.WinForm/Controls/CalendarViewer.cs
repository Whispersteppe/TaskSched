using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm.Controls
{
    public partial class CalendarViewer : UserControl, ICanvasItem<CalendarModel>
    {
        ScheduleManager? _scheduleManager;
        CalendarModel _calendarModel;

        public CalendarViewer()
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
                builder.AddButton("Add Calendar", TsCreateChildCalendar);
                builder.AddButton("Add Event", TsCreateChildEvent);

                return builder.ToolStripItems;

            }
        }

        private async void TsCreateChildCalendar(object? sender, EventArgs e)
        {
            var treeItem = await _scheduleManager.CreateModel(TreeItem, TreeItemTypeEnum.CalendarItem);
        }

        private async void TsCreateChildEvent(object? sender, EventArgs e)
        {
            var treeItem = await _scheduleManager.CreateModel(TreeItem, TreeItemTypeEnum.EventItem);
        }


        private async void TsDelete_Click(object? sender, EventArgs e)
        {
            await _scheduleManager.DeleteItem(_calendarModel);
        }

        private async void TsSave_Click(object? sender, EventArgs e)
        {
            _calendarModel.Name = txtName.Text;
            _calendarModel.ParentCalendarId = _calendarModel.ParentItem.ID;

            var rslt = await _scheduleManager.SaveModel(_calendarModel.ParentItem, _calendarModel);
        }

        public async Task Initialize(ScheduleManager scheduleManager, CalendarModel treeItem)
        {
            _scheduleManager = scheduleManager;

            TreeItem = treeItem;
            _calendarModel = treeItem;

            this.txtName.Text = treeItem.DisplayName;
        }

        public async Task Initialize(ScheduleManager scheduleManager, object treeItem)
        {
            await Initialize(scheduleManager, treeItem as CalendarModel);

        }



        public ITreeItem? TreeItem { get; private set; }

        public List<TreeItemTypeEnum> AllowedChildTypes => [TreeItemTypeEnum.CalendarItem, TreeItemTypeEnum.EventItem];

        public bool CanClose()
        {
            return true;
        }

        public bool CanCreateChild(TreeItemTypeEnum itemType)
        {
            return true;
        }



        public async Task<ITreeItem?> CreateChild(TreeItemTypeEnum itemType)
        {
            MessageBox.Show("CreateChild");
            return null;
        }

    }
}
