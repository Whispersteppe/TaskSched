namespace TaskScheduler.WinForm.Controls
{
    partial class ActivityViewer
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
            grdActivityProperties = new PropertyGrid();
            SuspendLayout();
            // 
            // grdActivityProperties
            // 
            grdActivityProperties.Dock = DockStyle.Fill;
            grdActivityProperties.Location = new Point(0, 0);
            grdActivityProperties.Name = "grdActivityProperties";
            grdActivityProperties.Size = new Size(413, 277);
            grdActivityProperties.TabIndex = 0;
            // 
            // ActivityViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(grdActivityProperties);
            Name = "ActivityViewer";
            Size = new Size(413, 277);
            ResumeLayout(false);
        }

        #endregion

        private PropertyGrid grdActivityProperties;
    }
}
