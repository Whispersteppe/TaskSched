namespace TaskScheduler.WinForm.Controls
{
    partial class FolderViewer
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
            gridFolderProperties = new PropertyGrid();
            SuspendLayout();
            // 
            // gridFolderProperties
            // 
            gridFolderProperties.Dock = DockStyle.Fill;
            gridFolderProperties.Location = new Point(0, 0);
            gridFolderProperties.Name = "gridFolderProperties";
            gridFolderProperties.Size = new Size(313, 189);
            gridFolderProperties.TabIndex = 2;
            gridFolderProperties.PropertyValueChanged += GridFolderProperties_PropertyValueChanged;
            // 
            // FolderViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridFolderProperties);
            Name = "FolderViewer";
            Size = new Size(313, 189);
            ResumeLayout(false);
        }

        #endregion

        private PropertyGrid gridFolderProperties;
    }
}
