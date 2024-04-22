namespace TaskScheduler.WinForm.Controls
{
    partial class AboutViewer
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
            gridAbout = new PropertyGrid();
            SuspendLayout();
            // 
            // gridAbout
            // 
            gridAbout.Dock = DockStyle.Fill;
            gridAbout.Location = new Point(0, 0);
            gridAbout.Name = "gridAbout";
            gridAbout.Size = new Size(285, 227);
            gridAbout.TabIndex = 0;
            // 
            // AboutViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridAbout);
            Name = "AboutViewer";
            Size = new Size(285, 227);
            ResumeLayout(false);
        }

        #endregion

        private PropertyGrid gridAbout;
    }
}
