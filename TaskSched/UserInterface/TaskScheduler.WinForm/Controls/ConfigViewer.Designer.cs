namespace TaskScheduler.WinForm.Controls
{
    partial class ConfigViewer
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
            gridConfig = new PropertyGrid();
            SuspendLayout();
            // 
            // gridConfig
            // 
            gridConfig.Dock = DockStyle.Fill;
            gridConfig.Location = new Point(0, 0);
            gridConfig.Name = "gridConfig";
            gridConfig.Size = new Size(347, 244);
            gridConfig.TabIndex = 0;
            // 
            // ConfigViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridConfig);
            Name = "ConfigViewer";
            Size = new Size(347, 244);
            ResumeLayout(false);
        }

        #endregion

        private PropertyGrid gridConfig;
    }
}
