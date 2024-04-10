using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm.Controls
{
    public partial class BlankViewer : UserControl, ICanvasItem
    {
        ScheduleManager _scheduleManager;

        public BlankViewer(ScheduleManager scheduleManager, ITreeItem item)
        {
            InitializeComponent();

            _scheduleManager = scheduleManager;

            if (item is ITreeItem treeItem)
            {
                TreeItem = treeItem;

                txtName.Text = treeItem.DisplayName;

                if (TreeItem.CanHaveChildren())
                {
                    AllowedChildTypes = TreeItem.AllowedChildTypes;
                }
                else
                {
                    AllowedChildTypes = [];
                }
            }
            else
            {
                TreeItem = null;
            }
        }

        public List<ToolStripItem> ToolStripItems
        {
            get
            {
                ToolStripBuilder builder = new ToolStripBuilder();
                if (TreeItem != null)
                {
                    foreach (TreeItemTypeEnum type in TreeItem.AllowedChildTypes)
                    {
                        var button = builder.AddButton($"Add {type}", TsAdd_Click);
                        button.Tag = type;
                    }
                }

                return builder.ToolStripItems;

            }
        }

        private async void TsAdd_Click(object? sender, EventArgs e)
        {
            if (sender is ToolStripItem toolStripItem)
            {
                if (toolStripItem.Tag is TreeItemTypeEnum addType)
                {
                    var treeItem = await _scheduleManager.CreateModel(TreeItem, addType);
                }

            }

        }

        public List<TreeItemTypeEnum> AllowedChildTypes { get; private set; } = [];

        public ITreeItem? TreeItem { get; private set; }


        public async Task LeavingItem()
        {
            await Task.Run(() => { });
        }


        public bool CanCreateChild(TreeItemTypeEnum itemType)
        {
            if (TreeItem == null) return false;

            if (TreeItem.CanHaveChildren())
            {
                return TreeItem.AllowedChildTypes.Contains(itemType);

            }
            else
            {
                return false;
            }

        }

        public async Task<ITreeItem?> CreateChild(TreeItemTypeEnum itemType)
        {
            if (CanCreateChild(itemType) == false) return null;
            if (TreeItem == null) return null;

            if (TreeItem.CanHaveChildren())
            {
                var treeItem = await _scheduleManager.CreateModel(TreeItem, TreeItemTypeEnum.ActivityItem);

                return treeItem;

            }
            else
            {
                return null;
            }
        }

        public async Task Initialize(ScheduleManager scheduleManager, object o)
        {
            await Task.Run(() => { });

            _scheduleManager = scheduleManager;

            if (o is ITreeItem treeItem)
            {
                TreeItem = treeItem;

                txtName.Text = treeItem.DisplayName;

                if (TreeItem.CanHaveChildren())
                {
                    AllowedChildTypes = TreeItem.AllowedChildTypes;
                }
                else
                {
                    AllowedChildTypes = [];
                }
            }
            else
            {
                TreeItem = null;
            }

        }
    }
}
