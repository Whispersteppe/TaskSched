namespace TaskScheduler.WinForm.Controls
{
    partial class CanvasSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CanvasSelector));
            toolStrip1 = new ToolStrip();
            tsAdd = new ToolStripDropDownButton();
            tsDelete = new ToolStripButton();
            tsSave = new ToolStripButton();
            tsReset = new ToolStripButton();
            panelCanvasArea = new Panel();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { tsAdd, tsDelete, tsSave, tsReset });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(632, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "tsContainerStrip";
            // 
            // tsAdd
            // 
            tsAdd.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsAdd.Image = (Image)resources.GetObject("tsAdd.Image");
            tsAdd.ImageTransparentColor = Color.Magenta;
            tsAdd.Name = "tsAdd";
            tsAdd.Size = new Size(42, 22);
            tsAdd.Text = "Add";
            // 
            // tsDelete
            // 
            tsDelete.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsDelete.Image = (Image)resources.GetObject("tsDelete.Image");
            tsDelete.ImageTransparentColor = Color.Magenta;
            tsDelete.Name = "tsDelete";
            tsDelete.Size = new Size(44, 22);
            tsDelete.Text = "Delete";
            tsDelete.Click += tsDelete_Click;
            // 
            // tsSave
            // 
            tsSave.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsSave.Image = (Image)resources.GetObject("tsSave.Image");
            tsSave.ImageTransparentColor = Color.Magenta;
            tsSave.Name = "tsSave";
            tsSave.Size = new Size(35, 22);
            tsSave.Text = "Save";
            tsSave.Click += tsSave_Click;
            // 
            // tsReset
            // 
            tsReset.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsReset.Image = (Image)resources.GetObject("tsReset.Image");
            tsReset.ImageTransparentColor = Color.Magenta;
            tsReset.Name = "tsReset";
            tsReset.Size = new Size(39, 22);
            tsReset.Text = "Reset";
            tsReset.Click += tsReset_Click;
            // 
            // panelCanvasArea
            // 
            panelCanvasArea.Dock = DockStyle.Fill;
            panelCanvasArea.Location = new Point(0, 25);
            panelCanvasArea.Name = "panelCanvasArea";
            panelCanvasArea.Size = new Size(632, 482);
            panelCanvasArea.TabIndex = 2;
            // 
            // CanvasSelector
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panelCanvasArea);
            Controls.Add(toolStrip1);
            Name = "CanvasSelector";
            Size = new Size(632, 507);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ToolStrip toolStrip1;
        private ToolStripButton tsDelete;
        private ToolStripButton tsSave;
        private ToolStripButton tsReset;
        private ToolStripDropDownButton tsAdd;
        private Panel panelCanvasArea;
    }
}
