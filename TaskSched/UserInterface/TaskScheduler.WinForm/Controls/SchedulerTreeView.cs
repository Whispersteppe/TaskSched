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

    public delegate void TreeItemHandler(ITreeItem item);
    public partial class SchedulerTreeView : UserControl
    {

        public event TreeItemHandler OnItemSelected;
        public SchedulerTreeView()
        {
            InitializeComponent();
        }

        List<ITreeItem> _list;

        public void SetTreeviewCollection(List<ITreeItem> list)
        {
            _list = list;
            foreach (ITreeItem item in _list)
            {
                TreeNode node = new TreeNode(item.Name)
                {

                };

                node.Tag = item;

                treeScheduler.Nodes.Add(node);
                AddChildren(node, item);


            }
        }

        private void AddChildren(TreeNode parentNode, ITreeItem parentTreeItem)
        {
            if (parentTreeItem.Children == null) return;

            foreach (ITreeItem item in parentTreeItem.Children)
            {
                TreeNode node = new TreeNode(item.Name)
                {

                };

                node.Tag = item;

                parentNode.Nodes.Add(node);

                AddChildren(node, item);

            }
        }

        private void treeScheduler_AfterSelect(object sender, TreeViewEventArgs e)
        {

            ITreeItem? item = e.Node.Tag as ITreeItem;
            if (item != null)
            {
                OnItemSelected(item);
            }
        }
    }
}
