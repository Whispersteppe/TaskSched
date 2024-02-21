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
        public EventViewer()
        {
            InitializeComponent();
        }

        public bool CanClose()
        {
            return true;
        }

        public void ShowItem(EventModel o)
        {
            this.txtName.Text = o.Name;
            this.lblLastExecution.Text = o.LastExecutionDate.ToString();
            this.lbNextExecution.Text = o.NextExecutionDate.ToString();
        }

        public void ShowItem(object o)
        {
            ShowItem(o as EventModel);
        }
    }
}
