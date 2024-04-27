namespace TaskScheduler.WinForm.Controls
{
    partial class SchedulerTreeView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchedulerTreeView));
            txtSearch = new TextBox();
            treeScheduler = new TreeView();
            treeviewImages = new ImageList(components);
            SuspendLayout();
            // 
            // txtSearch
            // 
            txtSearch.Dock = DockStyle.Top;
            txtSearch.Location = new Point(0, 0);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Search";
            txtSearch.Size = new Size(150, 23);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // treeScheduler
            // 
            treeScheduler.AllowDrop = true;
            treeScheduler.Dock = DockStyle.Fill;
            treeScheduler.HideSelection = false;
            treeScheduler.ImageIndex = 0;
            treeScheduler.ImageList = treeviewImages;
            treeScheduler.Location = new Point(0, 23);
            treeScheduler.Name = "treeScheduler";
            treeScheduler.SelectedImageIndex = 0;
            treeScheduler.Size = new Size(150, 166);
            treeScheduler.TabIndex = 1;
            treeScheduler.AfterCollapse += treeScheduler_AfterCollapse;
            treeScheduler.AfterExpand += treeScheduler_AfterExpand;
            treeScheduler.ItemDrag += treeScheduler_ItemDrag;
            treeScheduler.AfterSelect += treeScheduler_AfterSelect;
            treeScheduler.NodeMouseClick += treeScheduler_NodeMouseClick;
            treeScheduler.DragDrop += treeScheduler_DragDrop;
            treeScheduler.DragEnter += treeScheduler_DragEnter;
            treeScheduler.DragOver += treeScheduler_DragOver;
            // 
            // treeviewImages
            // 
            treeviewImages.ColorDepth = ColorDepth.Depth32Bit;
            treeviewImages.ImageStream = (ImageListStreamer)resources.GetObject("treeviewImages.ImageStream");
            treeviewImages.TransparentColor = Color.Transparent;
            treeviewImages.Images.SetKeyName(0, "Unknown.ico");
            treeviewImages.Images.SetKeyName(1, "ClosedFolder.ico");
            treeviewImages.Images.SetKeyName(2, "OpenFolder.ico");
            treeviewImages.Images.SetKeyName(3, "Checkbox.ico");
            // 
            // SchedulerTreeView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(treeScheduler);
            Controls.Add(txtSearch);
            Name = "SchedulerTreeView";
            Size = new Size(150, 189);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtSearch;
        private TreeView treeScheduler;
        private ImageList treeviewImages;
    }
}
