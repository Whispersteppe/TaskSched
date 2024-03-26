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
        ScheduleManager? _scheduleManager;

        public BlankViewer()
        {
            InitializeComponent();
        }

        public List<ToolStripItem> ToolStripItems
        {
            get
            {

                ToolStripBuilder builder = new ToolStripBuilder();
                foreach(TreeItemTypeEnum type in TreeItem.AllowedChildTypes)
                {
                    var button = builder.AddButton("Add", TsAdd_Click);
                    button.Tag = type;
                }

                return builder.ToolStripItems;

                //builder.AddButton("Delete", TsDelete_Click);


                //List<ToolStripItem> toolItems = new List<ToolStripItem>();

                //foreach(var addItem in TreeItem.AllowedChildTypes)
                //{
                //    ToolStripButton tsAdd;

                //    tsAdd = new ToolStripButton();

                //    tsAdd.DisplayStyle = ToolStripItemDisplayStyle.Text;

                //    tsAdd.ImageTransparentColor = Color.Magenta;
                //    tsAdd.Name = "tsAdd";
                //    tsAdd.Size = new Size(42, 22);
                //    tsAdd.Text = "Add";
                //    tsAdd.Tag = addItem;

                //    toolItems.Add(tsAdd);

                //    tsAdd.Click += TsAdd_Click;

                //}


                //return toolItems;
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
        }


        public bool CanCreateChild(TreeItemTypeEnum itemType)
        {
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
