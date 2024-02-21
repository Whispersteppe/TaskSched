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
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm.Controls
{
    public partial class CanvasSelector : UserControl
    {
        Control _currentCanvas;

        public CanvasSelector()
        {
            InitializeComponent();

            activityViewer.Visible = false;
            eventViewer.Visible = false;
            calendarViewer.Visible = false;
            processViewer.Visible = false;

            _currentCanvas = blankViewer;
            _currentCanvas.Dock = DockStyle.Fill;
            _currentCanvas.Visible = true;

        }

        public void ViewItem(ITreeItem item)
        {
            if (item == null)
            {
                ViewItem(item, blankViewer);
            }
            else if (item is ActivityModel activity)
            {
                ViewItem(activity, activityViewer);
            }
            else if (item is EventModel eventItem)
            {
                ViewItem(eventItem, eventViewer);
            }
            else if (item is CalendarModel calendar)
            {
                ViewItem(calendar, calendarViewer);
            }
            else
            {
                ViewItem(item, blankViewer);
            }
        }

        private void ViewItem<T>(T item, Control viewer)
        {
            SuspendLayout();
            _currentCanvas.Dock = DockStyle.None;

            _currentCanvas.Hide();

            _currentCanvas = viewer;
            _currentCanvas.Dock = DockStyle.Fill;

            if (_currentCanvas is ICanvasItem canvasItem)
            {
                canvasItem.ShowItem(item);
            }

            _currentCanvas.Show();

            ResumeLayout();
            PerformLayout();
        }

    }


}
