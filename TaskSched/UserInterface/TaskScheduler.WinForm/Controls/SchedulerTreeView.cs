using System.Collections.Generic;
using System.Xml.Linq;
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

            //            Icon icon = new Icon()
            //            treeScheduler.ImageList.Images.Add()
        }

        public async Task SetScheduleManager(ScheduleManager scheduleManager)
        {
            _scheduleManager = scheduleManager;

            _scheduleManager.OnTreeItemCreated += _scheduleManager_OnTreeItemCreated;
            _scheduleManager.OnTreeItemUpdated += _scheduleManager_OnTreeItemUpdated;
            _scheduleManager.OnTreeItemRemoved += _scheduleManager_OnTreeItemRemoved;
            _scheduleManager.OnItemSelected += _scheduleManager_OnItemSelected;


            _list = await _scheduleManager.GetAllRoots();

            SetTreeviewCollection(_list, "");
        }

        private async Task _scheduleManager_OnItemSelected(Guid itemId)
        {
            var find = FindItemByID(itemId);
            if (find != null)
            {
                var node = FindNode(find);
                if (node != null)
                {
                    treeScheduler.SelectedNode = node;
                    node.EnsureVisible();
                }
            }
        }

        private async Task _scheduleManager_OnTreeItemRemoved(ITreeItem treeItem)
        {
            await Task.Run(() => { });

            TreeNode? foundNode = FindNode(treeItem);
            if (foundNode != null)
            {
                treeScheduler.Nodes.Remove(foundNode);
            }


            ITreeItem? parentItem = FindItemByID(treeItem.ParentId);
            parentItem?.Children.Remove(treeItem);
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
            await Task.Run(() => { });

            var node = FindNode(treeItem);

            if (node != null)
            {
                node.Text = treeItem.DisplayName;
                node.Tag = treeItem;
            }
        }

        private async Task _scheduleManager_OnTreeItemCreated(ITreeItem? parentItem, ITreeItem childItem)
        {
            await Task.Run(() => { });

            if (parentItem != null)
            {
                var parentNode = FindNode(parentItem);
                if (parentNode != null)
                {

                    TreeNode node = new TreeNode(childItem.DisplayName)
                    {

                    };

                    node.Tag = childItem;
                    SetImageForNode(node);
                    parentNode.Nodes.Add(node);

                    //  select the new node
                    treeScheduler.SelectedNode = node;
                }

                ITreeItem? pItem = FindItemByID(parentItem.ID);
                pItem?.Children.Add(childItem);
            }
        }

        class TreeItemComparer : IComparer<ITreeItem>
        {
            public int Compare(ITreeItem? x, ITreeItem? y)
            {
                int compare = x.DisplayName.CompareTo(y.DisplayName);
                return compare;
            }
        }


        private void SetTreeviewCollection(List<ITreeItem> list, string searchText)
        {

            treeScheduler.Nodes.Clear();

            // don't sort the top ones.  just the children.
            //list.Sort(new TreeItemComparer());

            foreach (ITreeItem item in list)
            {
                TreeNode node = new TreeNode(item.DisplayName)
                {

                };

                node.Tag = item;

                SetImageForNode(node);

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

        private static void EnsureNodeTreeVisible(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
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

                List<ITreeItem> children = new List<ITreeItem>(parentTreeItem.Children);

                children.Sort(new TreeItemComparer());

                foreach (ITreeItem item in children)
                {
                    TreeNode node = new TreeNode(item.DisplayName);

                    node.Tag = item;
                    SetImageForNode(node);

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

            if (e.Node.Tag is ITreeItem item)
            {
                if (_scheduleManager != null)
                {
                    ITreeItem refreshedModel = await _scheduleManager.RefreshModel(item);
                    e.Node.Tag = refreshedModel;

                    await _scheduleManager.SelectTreeViewItem(refreshedModel);
                }
            }
        }

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text.Length == 0)
            {
                //  lets reset our list.  it's about due
                _list = await _scheduleManager.GetAllRoots();
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

            if (e.Data != null)
            {
                TreeNode? movingNode = (TreeNode?)e.Data.GetData(typeof(TreeNode));
                if (movingNode != null && movingNode.Tag is ITreeItem movingItem)
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
        }



        private void treeScheduler_DragDrop(object sender, DragEventArgs e)
        {
            Point targetPoint = treeScheduler.PointToClient(new Point(e.X, e.Y));
            if (e.Data != null)
            {
                TreeNode? movingNode = (TreeNode?)e.Data.GetData(typeof(TreeNode));

                if (movingNode != null && movingNode.Tag is ITreeItem movingItem)
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
                                _scheduleManager?.MoveItem(landingItem, movingItem);
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

        }

        private static bool ContainsNode(TreeNode selectedNode, TreeNode landingNode)
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

        private void treeScheduler_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left) return; //  we only want the off mouse button

            treeScheduler.SelectedNode = e.Node;

            if (e.Node.Tag is ITreeItem treeItem)
            {

                ContextMenuStrip? menu = treeItem.GetContextMenu();
                if (menu != null)
                {

                    menu.Show(treeScheduler, new Point(e.X, e.Y));
                }
            }
        }

        private void treeScheduler_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            SetImageForNode(e.Node);
        }

        private void treeScheduler_AfterExpand(object sender, TreeViewEventArgs e)
        {
            SetImageForNode(e.Node);

        }

        private void SetImageForNode(TreeNode selectedNode)
        {
            if (selectedNode.Tag != null && selectedNode.Tag is ITreeItem treeItem)
            {

                switch (treeItem.TreeItemType)
                {
                    case TreeItemTypeEnum.FolderItem:
                        if (selectedNode.IsExpanded == true)
                        {
                            selectedNode.ImageIndex = 2;
                            selectedNode.SelectedImageIndex = 2;
                        }
                        else
                        {
                            selectedNode.ImageIndex = 1;
                            selectedNode.SelectedImageIndex = 1;
                        }
                        break;
                    case TreeItemTypeEnum.EventItem:
                        selectedNode.ImageIndex = 3;
                        selectedNode.SelectedImageIndex = 3;
                        break;
                    case TreeItemTypeEnum.ActivityItem:
                        selectedNode.ImageIndex = 8;
                        selectedNode.SelectedImageIndex = 8;
                        break;
                    case TreeItemTypeEnum.LogRootItem:
                        selectedNode.ImageIndex = 4;
                        selectedNode.SelectedImageIndex = 4;
                        break;
                    case TreeItemTypeEnum.Unknown:
                        selectedNode.ImageIndex = 0;
                        selectedNode.SelectedImageIndex = 0;
                        break;
                    case TreeItemTypeEnum.RootItem:
                        selectedNode.ImageIndex = 0;
                        selectedNode.SelectedImageIndex = 0;
                        break;
                    case TreeItemTypeEnum.ActivityRootItem:
                        selectedNode.ImageIndex = 8;
                        selectedNode.SelectedImageIndex = 8;
                        break;
                    case TreeItemTypeEnum.FolderRootItem:
                        if (selectedNode.IsExpanded == true)
                        {
                            selectedNode.ImageIndex = 2;
                            selectedNode.SelectedImageIndex = 2;
                        }
                        else
                        {
                            selectedNode.ImageIndex = 1;
                            selectedNode.SelectedImageIndex = 1;
                        }
                        break;
                    case TreeItemTypeEnum.StatusRootItem:
                        selectedNode.ImageIndex = 5;
                        selectedNode.SelectedImageIndex = 5;
                        break;
                    case TreeItemTypeEnum.LogViewItem:
                        selectedNode.ImageIndex = 4;
                        selectedNode.SelectedImageIndex = 4;
                        break;
                    case TreeItemTypeEnum.SchedulerStatusItem:
                        selectedNode.ImageIndex = 5;
                        selectedNode.SelectedImageIndex = 5;
                        break;
                    case TreeItemTypeEnum.ExecutionEngineStatusItem:
                        selectedNode.ImageIndex = 5;
                        selectedNode.SelectedImageIndex = 5;
                        break;
                    case TreeItemTypeEnum.ConfigItem:
                        selectedNode.ImageIndex = 6;
                        selectedNode.SelectedImageIndex = 6;
                        break;
                    case TreeItemTypeEnum.AboutItem:
                        selectedNode.ImageIndex = 7;
                        selectedNode.SelectedImageIndex = 7;
                        break;
                    default:
                        selectedNode.ImageIndex = 0;
                        selectedNode.SelectedImageIndex = 0;
                        break;
                }
            }
        }

        private void treeScheduler_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == null) return;
            if (_scheduleManager == null) return;

            if (e.Node.Tag is EventModel eventModel)
            {
                Task.Run(async () =>
                {
                    await _scheduleManager.LaunchEvent(eventModel);
                }).Wait();
            }
        }
    }
}
