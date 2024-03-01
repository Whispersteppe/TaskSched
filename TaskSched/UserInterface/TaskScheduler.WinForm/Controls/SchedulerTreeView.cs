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

    public partial class SchedulerTreeView : UserControl
    {
        List<ITreeItem> _list = [];

        ScheduleManager? _scheduleManager;

        public SchedulerTreeView()
        {
            InitializeComponent();
        }

        public async Task SetScheduleManager(ScheduleManager scheduleManager)
        {
            _scheduleManager = scheduleManager;

            _scheduleManager.OnTreeItemCreated += _scheduleManager_OnTreeItemCreated;
            _scheduleManager.OnTreeItemUpdated += _scheduleManager_OnTreeItemUpdated;
            _scheduleManager.OnTreeItemRemoved += _scheduleManager_OnTreeItemRemoved;


            var tree = await _scheduleManager.GetAllRoots();

            SetTreeviewCollection(tree);
        }

        private async Task _scheduleManager_OnTreeItemRemoved(ITreeItem treeItem)
        {
            TreeNode? foundNode = FindNode(treeItem);
            if (foundNode != null)
            {
                treeScheduler.Nodes.Remove(foundNode);
            }
        }

        #region Find node by tagitem ITreeItem

        private TreeNode? FindNode(ITreeItem treeItem)
        { 
            foreach(TreeNode node in treeScheduler.Nodes)
            {
                if (treeItem.Equals(node.Tag))
                { 
                    return node; 
                }
                else
                {
                    TreeNode? childNode = FindNode(node, treeItem);
                    if (childNode != null)
                    {
                        return childNode;
                    }
                }

            }

            return null;

        }

        private TreeNode? FindNode(TreeNode parentNode, ITreeItem treeItem)
        {
            foreach (TreeNode node in parentNode.Nodes)
            {
                if (treeItem.Equals(node.Tag))
                {
                    return node;
                }
                else
                {
                    TreeNode? childNode = FindNode(node, treeItem);
                    if (childNode != null)
                    {
                        return childNode;
                    }
                }

            }

            return null;

        }

        #endregion


        private async Task _scheduleManager_OnTreeItemUpdated(ITreeItem treeItem)
        {
            var node = FindNode(treeItem);

            if (node != null)
            {
                node.Text = treeItem.DisplayName;
            }
        }

        private async Task _scheduleManager_OnTreeItemCreated(ITreeItem? parentItem, ITreeItem childItem)
        {
            if (parentItem != null)
            {
                var parentNode = FindNode(parentItem);
                if (parentNode != null)
                {

                    TreeNode node = new TreeNode(childItem.DisplayName)
                    {

                    };

                    node.Tag = childItem;
                    parentNode.Nodes.Add(node);
                }
            }
        }

        private void SetTreeviewCollection(List<ITreeItem> list)
        {

            treeScheduler.Nodes.Clear();

            _list = list;
            foreach (ITreeItem item in _list)
            {
                TreeNode node = new TreeNode(item.DisplayName)
                {

                };

                node.Tag = item;

                treeScheduler.Nodes.Add(node);
                AddChildren(node, item);


            }
        }

        private void AddChildren(TreeNode parentNode, ITreeItem parentTreeItem)
        {
            if (parentTreeItem.CanHaveChildren() == true)
            {
                if (parentTreeItem.Children == null) return;

                foreach (ITreeItem item in parentTreeItem.Children)
                {
                    TreeNode node = new TreeNode(item.DisplayName);

                    node.Tag = item;

                    parentNode.Nodes.Add(node);

                    AddChildren(node, item);

                }
            }
        }

        private async void treeScheduler_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null) return;

            ITreeItem? item = e.Node.Tag as ITreeItem;
            if (item != null)
            {
                if (_scheduleManager != null)
                {
                    await _scheduleManager.SelectTreeViewItem(item);
                }
            }
        }
    }
}
