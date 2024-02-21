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
    public partial class BlankViewer : UserControl, ICanvasItem<RootModel>
    {
        public BlankViewer()
        {
            InitializeComponent();
        }

        public bool CanClose()
        {
            return true;
        }

        public void ShowItem(object o)
        {
            ShowItem((RootModel)o);
        }

        public void ShowItem(RootModel o)
        {
            txtName.Text = o.Name;
        }
    }
}
