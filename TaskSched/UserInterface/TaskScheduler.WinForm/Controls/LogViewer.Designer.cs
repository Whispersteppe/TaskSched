namespace TaskScheduler.WinForm.Controls
{
    partial class LogViewer
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
            splitContainer1 = new SplitContainer();
            lvLogMessage = new ListView();
            colSeverity = new ColumnHeader();
            colTime = new ColumnHeader();
            colMessage = new ColumnHeader();
            logDetails = new PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(lvLogMessage);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(logDetails);
            splitContainer1.Size = new Size(469, 295);
            splitContainer1.SplitterDistance = 156;
            splitContainer1.TabIndex = 0;
            // 
            // lvLogMessage
            // 
            lvLogMessage.Columns.AddRange(new ColumnHeader[] { colSeverity, colTime, colMessage });
            lvLogMessage.Dock = DockStyle.Fill;
            lvLogMessage.FullRowSelect = true;
            lvLogMessage.Location = new Point(0, 0);
            lvLogMessage.Name = "lvLogMessage";
            lvLogMessage.Size = new Size(469, 156);
            lvLogMessage.TabIndex = 0;
            lvLogMessage.UseCompatibleStateImageBehavior = false;
            lvLogMessage.View = View.Details;
            lvLogMessage.SelectedIndexChanged += lvLogMessage_SelectedIndexChanged;
            // 
            // colSeverity
            // 
            colSeverity.Text = "Severity";
            // 
            // colTime
            // 
            colTime.Text = "Time";
            // 
            // colMessage
            // 
            colMessage.Text = "Message";
            // 
            // logDetails
            // 
            logDetails.Dock = DockStyle.Fill;
            logDetails.Location = new Point(0, 0);
            logDetails.Name = "logDetails";
            logDetails.Size = new Size(469, 135);
            logDetails.TabIndex = 0;
            // 
            // LogViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Name = "LogViewer";
            Size = new Size(469, 295);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private ListView lvLogMessage;
        private ColumnHeader colSeverity;
        private ColumnHeader colTime;
        private ColumnHeader colMessage;
        private PropertyGrid logDetails;
    }
}
