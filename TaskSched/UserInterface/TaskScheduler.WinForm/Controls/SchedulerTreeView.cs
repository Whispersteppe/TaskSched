using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using TaskScheduler.WinForm.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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


            _list = await _scheduleManager.GetAllRoots();

            SetTreeviewCollection(_list, "");
        }

        private async Task _scheduleManager_OnTreeItemRemoved(ITreeItem treeItem)
        {
            TreeNode? foundNode = FindNode(treeItem);
            if (foundNode != null)
            {
                treeScheduler.Nodes.Remove(foundNode);
            }



            ITreeItem parentItem = FindItemByID(treeItem.ParentItem.ID);
            if (parentItem != null)
            {
                parentItem.Children.Remove(treeItem);
            }
        }

        #region Find node by tagitem ITreeItem

        private TreeNode? FindNode(ITreeItem treeItem)
        {
            foreach (TreeNode node in treeScheduler.Nodes)
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

        

        private ITreeItem? FindItemByID(Guid? ID)
        {
            if (ID == null) return null;

            foreach (ITreeItem item in _list)
            {
                if (item.ID == ID)
                {
                    return item;
                }
                else
                {
                    ITreeItem? childItem = FindItemByID(item, ID);
                    if (childItem != null)
                    {
                        return childItem;
                    }
                }

            }

            return null;

        }

        private ITreeItem? FindItemByID(ITreeItem parentItem, Guid? ID)
        {
            if (ID == null) return null;

            foreach (ITreeItem item in parentItem.Children)
            {
                if (ID == item.ID)
                {
                    return item;
                }
                else
                {
                    ITreeItem? childItem = FindItemByID(item, ID);
                    if (childItem != null)
                    {
                        return childItem;
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
                node.Tag = treeItem;
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

                ITreeItem pItem = FindItemByID(parentItem.ID);
                if (pItem != null)
                {
                    pItem.Children.Add(childItem);
                }
            }
        }

        private void SetTreeviewCollection(List<ITreeItem> list, string searchText)
        {

            treeScheduler.Nodes.Clear();

            //_list = list;
            foreach (ITreeItem item in list)
            {
                TreeNode node = new TreeNode(item.DisplayName)
                {

                };

                node.Tag = item;

                AddChildren(node, item, searchText);

                if (string.IsNullOrEmpty(searchText) || item.ContainsText(searchText) || node.Nodes.Count > 0)
                {
                    treeScheduler.Nodes.Add(node);
                }
            }

            if (string.IsNullOrEmpty(searchText) == false) 
            {
                EnsureNodeTreeVisible(treeScheduler.Nodes);
            }
        }

        private void EnsureNodeTreeVisible(TreeNodeCollection nodes)
        {
            foreach(TreeNode node in nodes)
            {
                node.EnsureVisible();
                EnsureNodeTreeVisible(node.Nodes);
            }

        }

        private void AddChildren(TreeNode parentNode, ITreeItem parentTreeItem, string searchText)
        {
            if (parentTreeItem.CanHaveChildren() == true)
            {
                if (parentTreeItem.Children == null) return;

                foreach (ITreeItem item in parentTreeItem.Children)
                {
                    TreeNode node = new TreeNode(item.DisplayName);

                    node.Tag = item;

                    AddChildren(node, item, searchText);

                    if (string.IsNullOrEmpty(searchText) || item.ContainsText(searchText) || node.Nodes.Count > 0)
                    {
                        parentNode.Nodes.Add(node);
                    }


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
                    ITreeItem refreshedModel = await _scheduleManager.RefreshModel(item);
                    e.Node.Tag = refreshedModel;

                    await _scheduleManager.SelectTreeViewItem(refreshedModel);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text.Length == 0)
            {
                SetTreeviewCollection(_list, "");
            }
            else
            {
                SetTreeviewCollection(_list, txtSearch.Text);
            }
        }

        //private void txtSearch_TextChanged(object sender, EventArgs e)
        //{
        //    if (txtSearch.Text.Length == 0)
        //    {
        //        ClearIsVisible();
        //    }
        //    else
        //    {
        //        SearchAndMark(treeScheduler.Nodes, txtSearch.Text);
        //    }
        //}


        //private void SearchAndMark(TreeNodeCollection items, string text)
        //{
        //    if (items == null) return;


        //    foreach (TreeNode item in items)
        //    {
        //        item.Collapse(false);
        //        if (TreeNodeMatch(item, text))
        //        {
        //            item.BackColor = Color.Blue;
        //            item.EnsureVisible();
        //        }
        //        else
        //        {
        //            item.BackColor = Color.White;
        //        }

        //        SearchAndMark(item.Nodes, text);
        //    }

        //}

        //private bool TreeNodeMatch(TreeNode node, string text)
        //{
        //    if (node == null) return false;
        //    if (node.Text.Contains(text)) return true;

        //    if (node.Tag is ITreeItem treeItem)
        //    {
        //        if (treeItem.ContainsText(text) == true) return true;

        //    }

        //    return false;
        //}


        //private void ClearIsVisible()
        //{
        //    foreach (TreeNode item in this.treeScheduler.Nodes)
        //    {
        //        item.EnsureVisible();
        //        item.BackColor = Color.White;
        //        ClearIsVisible(item.Nodes);
        //    }
        //}
        //private void ClearIsVisible(TreeNodeCollection childItems)
        //{
        //    if (childItems == null) return;

        //    foreach (TreeNode item in childItems)
        //    {
        //        item.EnsureVisible();
        //        item.BackColor = Color.White;
        //        ClearIsVisible(item.Nodes);
        //    }
        //}


        #region Drag and Drop

        private void treeScheduler_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        private void treeScheduler_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void treeScheduler_DragOver(object sender, DragEventArgs e)
        {
            Point targetPoint = treeScheduler.PointToClient(new Point(e.X, e.Y));
            TreeNode movingNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            if (movingNode.Tag is ITreeItem movingItem)
            {
                // Select the node at the mouse position.
                TreeNode landingNode = treeScheduler.GetNodeAt(targetPoint);

                if (landingNode != null)
                {
                    if (landingNode.Tag is ITreeItem landingItem)
                    {

                        if (movingItem.CanMoveItem(landingItem) == true && ContainsNode(movingNode, landingNode) == false)
                        {
                            e.Effect = DragDropEffects.Move;
                        }
                        else
                        {
                            e.Effect = DragDropEffects.None;
                        }
                    }
                }
            }
        }



        private void treeScheduler_DragDrop(object sender, DragEventArgs e)
        {
            Point targetPoint = treeScheduler.PointToClient(new Point(e.X, e.Y));
            TreeNode movingNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            if (movingNode.Tag is ITreeItem movingItem)
            {
                // Select the node at the mouse position.
                TreeNode landingNode = treeScheduler.GetNodeAt(targetPoint);

                if (landingNode != null)
                {
                    //  make sure we're not trying to land on a parent of ourselves
                    if (ContainsNode(movingNode, landingNode) == true)
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }

                    if (landingNode.Tag is ITreeItem landingItem)
                    {
                        if (movingItem.CanMoveItem(landingItem) == true)
                        {
                            _scheduleManager.MoveItem(landingItem, movingItem);
                            movingNode.Remove();
                            landingNode.Nodes.Add(movingNode);
                        }
                        else
                        {
                            //MessageBox.Show("Cannot drop here.  ignoring");
                        }
                    }
                }
            }

        }

        private bool ContainsNode(TreeNode selectedNode, TreeNode landingNode)
        {
            // Check the parent node of the second node.
            if (landingNode.Parent == null) return false;
            if (landingNode.Parent.Equals(selectedNode)) return true;

            // If the parent node is not null or equal to the first node, 
            // call the ContainsNode method recursively using the parent of 
            // the second node.
            return ContainsNode(selectedNode, landingNode.Parent);
        }

        #endregion

    }
}
