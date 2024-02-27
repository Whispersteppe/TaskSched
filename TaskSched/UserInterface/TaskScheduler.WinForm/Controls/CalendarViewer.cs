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
    public partial class CalendarViewer : UserControl, ICanvasItem<CalendarModel>, ICanvasItemCanDelete, ICanvasItemCanEdit, ICanvasItemHasChildren
    {
        ScheduleManager? _scheduleManager;
        CalendarModel _calendarModel;

        public CalendarViewer()
        {
            InitializeComponent();
        }


        public void SetScheduleManager(ScheduleManager scheduleManager)
        {
            _scheduleManager = scheduleManager;
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



        public void Delete()
        {
            MessageBox.Show("Delete");

        }

        public void Revert()
        {
            MessageBox.Show("Revert");

        }

        public void Save()
        {
            MessageBox.Show("Save");

        }

        public void ShowItem(object o)
        {
            ShowItem(o as CalendarModel);
        }

        public void ShowItem(CalendarModel o)
        {
            TreeItem = o;
            _calendarModel = o;

            this.txtName.Text = o.Name;

        }

        public async Task<ITreeItem?> CreateChild(TreeItemTypeEnum itemType)
        {
            MessageBox.Show("CreateChild");
            return null;
        }


    }
}
