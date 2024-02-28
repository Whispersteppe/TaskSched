using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        Control? _currentCanvas;

        Dictionary<Type, Type> _canvasItems = new Dictionary<Type, Type>();

        ScheduleManager _scheduleManager;

        public CanvasSelector()
        {
            InitializeComponent();

            _canvasItems.Clear();

            _canvasItems.Add(typeof(ActivityModel), typeof(ActivityViewer));
            _canvasItems.Add(typeof(EventModel), typeof(EventViewer));
            _canvasItems.Add(typeof(CalendarModel), typeof(CalendarViewer));
            _canvasItems.Add(typeof(LogRootModel), typeof(BlankViewer));
            _canvasItems.Add(typeof(StatusRootModel), typeof(BlankViewer));
            _canvasItems.Add(typeof(CalendarRootModel), typeof(BlankViewer));
            _canvasItems.Add(typeof(ActivityRootModel), typeof(BlankViewer));

            _currentCanvas = null;
        }

        public async Task SetScheduleManager(ScheduleManager scheduleManager)
        {
            _scheduleManager = scheduleManager;
            _scheduleManager.OnTreeItemSelected += _scheduleManager_OnTreeItemSelected;

        }

        private async Task _scheduleManager_OnTreeItemSelected(ITreeItem treeItem)
        {
            await this.ViewItem(treeItem); 
        }

        public async Task ViewItem(ITreeItem item)
        {
            if (item == null)
            {
                await ViewItem(item, typeof(BlankViewer));
            }
            else
            {

                Type? viewerType = _canvasItems[item.GetType()];
                if (viewerType == null)
                {
                    await ViewItem(item, typeof(BlankViewer));
                }
                else
                {
                    await ViewItem(item, viewerType);
                }
            }

        }


        private async Task ViewItem(ITreeItem item, Type viewerType)
        {
            object? o = viewerType.Assembly.CreateInstance(viewerType.FullName);
            if (o != null)
            {
                SuspendLayout();
                if (_currentCanvas != null)
                {
                    panelCanvasArea.Controls.Remove(_currentCanvas);

                    _currentCanvas.Dispose();
                }

                _currentCanvas = o as Control;

                _currentCanvas.AutoSize = true;
                _currentCanvas.Dock = DockStyle.Fill;
                _currentCanvas.Visible = true;

                panelCanvasArea.Controls.Add(_currentCanvas);

                if (_currentCanvas is ICanvasItem canvasItem)
                {
                    await canvasItem.Initialize(_scheduleManager, item);

                    // get the tool strip items from the canvas viewer
                    //foreach(ToolStripItem toolStripItem in toolStrip1.Items)
                    //{
                    //    toolStripItem.Dispose();
                    //}
                    toolStrip1.Items.Clear();

                    var toolStripItems = canvasItem.ToolStripItems;
                    foreach(var toolStripItem in toolStripItems)
                    {
                        toolStrip1.Items.Add(toolStripItem);
                    }
                }
                else
                {
                }

                _currentCanvas.Show();

                ResumeLayout();
                PerformLayout();



            }
        }


    }


}
