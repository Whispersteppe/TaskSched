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
    public partial class BlankViewer : UserControl, ICanvasItem, ICanvasItemHasChildren
    {
        ScheduleManager? _scheduleManager;

        public BlankViewer()
        {
            InitializeComponent();
        }

        public List<TreeItemTypeEnum> AllowedChildTypes { get; private set; } = [];

        public ITreeItem? TreeItem { get; private set; }



        public void SetScheduleManager(ScheduleManager scheduleManager)
        {
            _scheduleManager = scheduleManager;
        }


        public void ShowItem(object o)
        {
            if (o is ITreeItem treeItem)
            {
                TreeItem = treeItem;

                txtName.Text = treeItem.Name;

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
        public bool CanClose()
        {
            return false;
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



    }
}
