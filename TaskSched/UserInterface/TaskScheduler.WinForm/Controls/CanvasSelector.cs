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

            tsAdd.Enabled = false;
            tsDelete.Enabled = false;
            tsSave.Enabled = false;
            tsReset.Enabled = false;

            tsAdd.DropDownItems.Clear();

            _currentCanvas = null;
        }

        public async Task SetScheduleManager(ScheduleManager scheduleManager)
        {
            _scheduleManager = scheduleManager;
            _scheduleManager.OnTreeItemSelected += _scheduleManager_OnTreeItemSelected;

        }

        private async Task _scheduleManager_OnTreeItemSelected(ITreeItem treeItem)
        {
            this.ViewItem(treeItem); 
        }

        private void Ts_AddItem_Click(object? sender, EventArgs e)
        {
            if (sender is ToolStripButton tsAddButton)
            {
                MessageBox.Show($"Canvas Add {tsAddButton.Tag}");
                ICanvasItemHasChildren canvas = _currentCanvas as ICanvasItemHasChildren;
                if (canvas != null)
                {
                    if (canvas.CanCreateChild((TreeItemTypeEnum)tsAddButton.Tag) == true)
                    {
                        canvas.CreateChild((TreeItemTypeEnum)tsAddButton.Tag);
                    }
                }
            }
        }

        private void tsDelete_Click(object sender, EventArgs e)
        {
            ICanvasItemCanDelete canvas = _currentCanvas as ICanvasItemCanDelete;
            if (canvas != null)
            {
                canvas.Delete();
            }

        }

        private void tsSave_Click(object sender, EventArgs e)
        {
            ICanvasItemCanEdit canvas = _currentCanvas as ICanvasItemCanEdit;

            if (canvas != null)
            {
                canvas.Save();
            }

        }

        private void tsReset_Click(object sender, EventArgs e)
        {
            ICanvasItemCanEdit canvas = _currentCanvas as ICanvasItemCanEdit;

            if (canvas != null)
            {
                canvas.Revert();
            }
        }


        public void ViewItem(ITreeItem item)
        {
            if (item == null)
            {
                ViewItem(item, typeof(BlankViewer));
            }
            else
            {

                Type? viewerType = _canvasItems[item.GetType()];
                if (viewerType == null)
                {
                    ViewItem(item, typeof(BlankViewer));
                }
                else
                {
                    ViewItem(item, viewerType);
                }
            }

        }


        private void ViewItem(ITreeItem item, Type viewerType)
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
                    canvasItem.SetScheduleManager(_scheduleManager);

                    if (item != null)
                    {
                        canvasItem.ShowItem(item);
                        tsAdd.Enabled = canvasItem is ICanvasItemHasChildren;

                        tsDelete.Enabled = canvasItem is ICanvasItemCanDelete;
                        tsSave.Enabled = canvasItem is ICanvasItemCanEdit;
                        tsReset.Enabled = canvasItem is ICanvasItemCanEdit;

                        //  update the add list
                        tsAdd.DropDownItems.Clear();
                        if (canvasItem is ICanvasItemHasChildren canvasItemHasChildren)
                        {
                            if (canvasItemHasChildren.AllowedChildTypes.Count > 0)
                            {


                                foreach (var itemType in canvasItemHasChildren.AllowedChildTypes)
                                {
                                    ToolStripItem ts = new ToolStripButton(itemType.ToString());
                                    ts.Click += Ts_AddItem_Click;
                                    ts.Tag = itemType;
                                    ts.DisplayStyle = ToolStripItemDisplayStyle.Text;
                                    ts.AutoSize = true;

                                    tsAdd.DropDownItems.Add(ts);
                                }
                            }
                            else
                            {
                                tsAdd.Enabled = false;
                            }
                        }

                    }
                    else
                    {
                        tsAdd.Enabled = false;
                        tsDelete.Enabled = false;
                        tsSave.Enabled = false;
                        tsReset.Enabled = false;

                        tsAdd.DropDownItems.Clear();

                    }
                }
                else
                {
                    tsAdd.Enabled = false;
                    tsDelete.Enabled = false;
                    tsSave.Enabled = false;
                    tsReset.Enabled = false;

                    tsAdd.DropDownItems.Clear();
                }

                _currentCanvas.Show();

                ResumeLayout();
                PerformLayout();



            }
        }


    }


}
