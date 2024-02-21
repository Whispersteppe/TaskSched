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
            activityViewer = new ActivityViewer();
            calendarViewer = new CalendarViewer();
            eventViewer = new EventViewer();
            processViewer = new ProcessViewer();
            blankViewer = new BlankViewer();
            SuspendLayout();
            // 
            // activityViewer
            // 
            activityViewer.Location = new Point(18, 150);
            activityViewer.Name = "activityViewer";
            activityViewer.Size = new Size(70, 80);
            activityViewer.TabIndex = 2;
            // 
            // calendarViewer
            // 
            calendarViewer.Location = new Point(177, 109);
            calendarViewer.Name = "calendarViewer";
            calendarViewer.Size = new Size(150, 150);
            calendarViewer.TabIndex = 4;
            // 
            // eventViewer
            // 
            eventViewer.Location = new Point(103, 160);
            eventViewer.Name = "eventViewer";
            eventViewer.Size = new Size(150, 150);
            eventViewer.TabIndex = 6;
            // 
            // processViewer
            // 
            processViewer.Location = new Point(127, 175);
            processViewer.Name = "processViewer";
            processViewer.Size = new Size(150, 150);
            processViewer.TabIndex = 7;
            // 
            // blankViewer
            // 
            blankViewer.AutoSize = true;
            blankViewer.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            blankViewer.Location = new Point(162, 31);
            blankViewer.Name = "blankViewer";
            blankViewer.Size = new Size(123, 30);
            blankViewer.TabIndex = 8;
            // 
            // CanvasSelector
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(blankViewer);
            Controls.Add(processViewer);
            Controls.Add(eventViewer);
            Controls.Add(calendarViewer);
            Controls.Add(activityViewer);
            Name = "CanvasSelector";
            Size = new Size(352, 312);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ActivityViewer activityViewer;
        private CalendarViewer calendarViewer;
        private EventViewer eventViewer;
        private ProcessViewer processViewer;
        private BlankViewer blankViewer;
    }
}
