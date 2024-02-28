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
            toolStrip1 = new ToolStrip();
            panelCanvasArea = new Panel();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(632, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "tsContainerStrip";
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
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ToolStrip toolStrip1;
        private Panel panelCanvasArea;
    }
}
