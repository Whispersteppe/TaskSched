using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskScheduler.WinForm.Models;
using TaskScheduler.WinForm.NLogCustom;

namespace TaskScheduler.WinForm.Controls
{
    public partial class SchedulerStatusViewer : UserControl
    {
        ScheduleManager _manager;
        SchedulerStatusModel _model;

        public SchedulerStatusViewer(ScheduleManager scheduleManager, SchedulerStatusModel model)
        {
            InitializeComponent();

            _manager = scheduleManager;
            _model = model;

            FillGrid();
        }

        private void FillGrid()
        {
            lvEvents.BeginUpdate();
            lvEvents.Items.Clear();

            var items = _model.StatusItems().Result;
            foreach (var item in items)
            {
                ListViewItem lvItem = new ListViewItem(item.Name);
                lvItem.SubItems.Add(item.NextExecution.ToString());
                lvItem.SubItems.Add(item.PreviousExecution.ToString());
                lvItem.SubItems.Add(item.IsActive.ToString());
                lvItem.Tag = item;

                lvEvents.Items.Add(lvItem);

                foreach (ColumnHeader col in lvEvents.Columns)
                {
                    Size t = TextRenderer.MeasureText(lvItem.SubItems[col.Index].Text, lvItem.SubItems[col.Index].Font);
                    if (t.Width + 8 > col.Width)
                    {
                        col.Width = t.Width + 8;
                    }
                }
            }

            lvEvents.ListViewItemSorter = new EventSorter(1);
            lvEvents.Sort();

            lvEvents.EndUpdate();
        }

        private void lvEvents_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            lvEvents.ListViewItemSorter = new EventSorter(e.Column);
            lvEvents.Sort();
        }

        private async void lvEvents_DoubleClick(object sender, EventArgs e)
        {
            if (lvEvents.SelectedItems.Count > 0)
            {
                var selectedItem = lvEvents.SelectedItems[0];
                if (selectedItem.Tag is ScheduleStatusItem item)
                {
                    await _manager.SelectItem(item.ID);
                }
            }
        }
    }

    public class EventSorter : IComparer<ListViewItem>, IComparer
    {
        int _sortColumn;

        public EventSorter(int sortColumn) 
        { 
            _sortColumn = sortColumn;
        }

        public int Compare(ListViewItem? x, ListViewItem? y)
        {
            var scheduleStatusItemX = x.Tag as ScheduleStatusItem;
            var scheduleStatusItemY = y.Tag as ScheduleStatusItem;

            switch(_sortColumn)
            {
                case 0:
                    return  scheduleStatusItemX.Name.CompareTo(scheduleStatusItemY.Name);
                case 1:
                    return scheduleStatusItemX.NextExecution.CompareTo(scheduleStatusItemY.NextExecution);
                case 2:
                    return scheduleStatusItemX.PreviousExecution.CompareTo(scheduleStatusItemY.PreviousExecution);
                case 3:
                    return scheduleStatusItemX.IsActive.CompareTo(scheduleStatusItemY.IsActive);
                default:
                    return 0;
            }

        }

        public int Compare(object? x, object? y)
        {
            return Compare(x as ListViewItem, y as ListViewItem);
        }
    }
}
