namespace TaskScheduler.WinForm.Controls
{
    partial class SchedulerStatusViewer
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
            lvEvents = new ListView();
            colName = new ColumnHeader();
            colNextExecution = new ColumnHeader();
            colEnabled = new ColumnHeader();
            colLastExecution = new ColumnHeader();
            SuspendLayout();
            // 
            // lvEvents
            // 
            lvEvents.Columns.AddRange(new ColumnHeader[] { colName, colNextExecution, colLastExecution, colEnabled });
            lvEvents.Dock = DockStyle.Fill;
            lvEvents.FullRowSelect = true;
            lvEvents.Location = new Point(0, 0);
            lvEvents.Name = "lvEvents";
            lvEvents.Size = new Size(316, 206);
            lvEvents.TabIndex = 0;
            lvEvents.UseCompatibleStateImageBehavior = false;
            lvEvents.View = View.Details;
            lvEvents.ColumnClick += lvEvents_ColumnClick;
            // 
            // colName
            // 
            colName.Text = "Name";
            // 
            // colNextExecution
            // 
            colNextExecution.Text = "Next Execution";
            // 
            // colEnabled
            // 
            colEnabled.DisplayIndex = 2;
            colEnabled.Text = "Enabled";
            // 
            // colLastExecution
            // 
            colLastExecution.DisplayIndex = 3;
            colLastExecution.Text = "Last Execution";
            // 
            // SchedulerStatusViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lvEvents);
            Name = "SchedulerStatusViewer";
            Size = new Size(316, 206);
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private ListView lvEvents;
        private ColumnHeader colName;
        private ColumnHeader colNextExecution;
        private ColumnHeader colEnabled;
        private ColumnHeader colLastExecution;
    }
}
