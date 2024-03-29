﻿namespace TaskScheduler.WinForm.Controls
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
            txtSearch = new TextBox();
            treeScheduler = new TreeView();
            SuspendLayout();
            // 
            // txtSearch
            // 
            txtSearch.Dock = DockStyle.Top;
            txtSearch.Location = new Point(0, 0);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(150, 23);
            txtSearch.TabIndex = 0;
            // 
            // treeScheduler
            // 
            treeScheduler.Dock = DockStyle.Fill;
            treeScheduler.Location = new Point(0, 23);
            treeScheduler.Name = "treeScheduler";
            treeScheduler.Size = new Size(150, 166);
            treeScheduler.TabIndex = 1;
            treeScheduler.AfterSelect += treeScheduler_AfterSelect;
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
    }
}
