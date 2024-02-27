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
    public partial class EventViewer : UserControl, ICanvasItem<EventModel>, ICanvasItemCanDelete, ICanvasItemCanEdit
    {
        ScheduleManager? _scheduleManager;

        public EventViewer()
        {
            InitializeComponent();
        }


        public void SetScheduleManager(ScheduleManager scheduleManager)
        {
            _scheduleManager = scheduleManager;
        }

        public ITreeItem? TreeItem { get; private set; }

        public bool CanClose()
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

        public void ShowItem(EventModel o)
        {
            TreeItem = o;

            this.txtName.Text = o.Name;
            this.lblLastExecution.Text = o.LastExecutionDate.ToString();
            this.lbNextExecution.Text = o.NextExecutionDate.ToString();
        }

        public void ShowItem(object o)
        {
            if (o == null) return;
            if (o is EventModel eventModel) 
            {
                ShowItem(eventModel);
            }
        }
    }
}
