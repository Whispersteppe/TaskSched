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
    public partial class CalendarViewer : UserControl, ICanvasItem<CalendarModel>
    {
        public CalendarViewer()
        {
            InitializeComponent();
        }

        public bool CanClose()
        {
            return true;
        }

        public void ShowItem(CalendarModel o)
        {
            this.txtName.Text = o.Name;

        }

        public void ShowItem(object o)
        {
            ShowItem(o as CalendarModel);
        }
    }
}
