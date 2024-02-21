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
    public partial class ActivityViewer : UserControl, ICanvasItem<ActivityModel>
    {
        public ActivityViewer()
        {
            InitializeComponent();
        }

        public bool CanClose()
        {
            return true;
        }

        public void ShowItem(object o)
        {
            ShowItem((ActivityModel)o);
        }

        public void ShowItem(ActivityModel o)
        {
            this.txtName.Text = o.Name;
            //this.lvFields 
            //this.cmbActivityHandler
        }
    }
}
