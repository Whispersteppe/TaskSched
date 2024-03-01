﻿namespace TaskScheduler.WinForm.Controls
{
    partial class EventViewer
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
            tabControl1 = new TabControl();
            tabOverview = new TabPage();
            cbCatchUpOnStartup = new CheckBox();
            cbIsActive = new CheckBox();
            lbNextExecution = new Label();
            lblLastExecution = new Label();
            label5 = new Label();
            label4 = new Label();
            txtName = new TextBox();
            label1 = new Label();
            tabSchedule = new TabPage();
            lstScheduleItems = new ListBox();
            tabActivities = new TabPage();
            lstActivityFields = new ListBox();
            txtFieldValue = new TextBox();
            txtFieldName = new Label();
            label3 = new Label();
            label2 = new Label();
            lstActivities = new ListBox();
            splitContainer1 = new SplitContainer();
            label10 = new Label();
            txtScheduleName = new Label();
            btnScheduleAdd = new Button();
            btnScheduleDelete = new Button();
            txtScheduleCRON = new TextBox();
            tabControl2 = new TabControl();
            tabScheduleTime = new TabPage();
            tabScheduleDays = new TabPage();
            btnScheduleSave = new Button();
            tabScheduleMonth = new TabPage();
            tabScheduleYear = new TabPage();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            label12 = new Label();
            label14 = new Label();
            label15 = new Label();
            label16 = new Label();
            label17 = new Label();
            label18 = new Label();
            label19 = new Label();
            label20 = new Label();
            label21 = new Label();
            label22 = new Label();
            label23 = new Label();
            label24 = new Label();
            label25 = new Label();
            label26 = new Label();
            label27 = new Label();
            label28 = new Label();
            splitContainer2 = new SplitContainer();
            splitContainer3 = new SplitContainer();
            btnActivityAdd = new Button();
            btnActivityDelete = new Button();
            btnActivitySave = new Button();
            tabControl1.SuspendLayout();
            tabOverview.SuspendLayout();
            tabSchedule.SuspendLayout();
            tabActivities.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabControl2.SuspendLayout();
            tabScheduleTime.SuspendLayout();
            tabScheduleDays.SuspendLayout();
            tabScheduleMonth.SuspendLayout();
            tabScheduleYear.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabOverview);
            tabControl1.Controls.Add(tabSchedule);
            tabControl1.Controls.Add(tabActivities);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(582, 447);
            tabControl1.TabIndex = 2;
            // 
            // tabOverview
            // 
            tabOverview.Controls.Add(cbCatchUpOnStartup);
            tabOverview.Controls.Add(cbIsActive);
            tabOverview.Controls.Add(lbNextExecution);
            tabOverview.Controls.Add(lblLastExecution);
            tabOverview.Controls.Add(label5);
            tabOverview.Controls.Add(label4);
            tabOverview.Controls.Add(txtName);
            tabOverview.Controls.Add(label1);
            tabOverview.Location = new Point(4, 24);
            tabOverview.Name = "tabOverview";
            tabOverview.Padding = new Padding(3);
            tabOverview.Size = new Size(574, 419);
            tabOverview.TabIndex = 0;
            tabOverview.Text = "Overview";
            tabOverview.UseVisualStyleBackColor = true;
            // 
            // cbCatchUpOnStartup
            // 
            cbCatchUpOnStartup.AutoSize = true;
            cbCatchUpOnStartup.CheckAlign = ContentAlignment.MiddleRight;
            cbCatchUpOnStartup.Location = new Point(20, 63);
            cbCatchUpOnStartup.Name = "cbCatchUpOnStartup";
            cbCatchUpOnStartup.Size = new Size(131, 19);
            cbCatchUpOnStartup.TabIndex = 11;
            cbCatchUpOnStartup.Text = "Catch up on startup";
            cbCatchUpOnStartup.UseVisualStyleBackColor = true;
            // 
            // cbIsActive
            // 
            cbIsActive.AutoSize = true;
            cbIsActive.CheckAlign = ContentAlignment.MiddleRight;
            cbIsActive.Location = new Point(81, 38);
            cbIsActive.Name = "cbIsActive";
            cbIsActive.Size = new Size(70, 19);
            cbIsActive.TabIndex = 10;
            cbIsActive.Text = "Is Active";
            cbIsActive.UseVisualStyleBackColor = true;
            // 
            // lbNextExecution
            // 
            lbNextExecution.AutoSize = true;
            lbNextExecution.Location = new Point(143, 120);
            lbNextExecution.Name = "lbNextExecution";
            lbNextExecution.Size = new Size(38, 15);
            lbNextExecution.TabIndex = 9;
            lbNextExecution.Text = "label7";
            // 
            // lblLastExecution
            // 
            lblLastExecution.AutoSize = true;
            lblLastExecution.Location = new Point(140, 94);
            lblLastExecution.Name = "lblLastExecution";
            lblLastExecution.Size = new Size(38, 15);
            lblLastExecution.TabIndex = 8;
            lblLastExecution.Text = "label6";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(20, 122);
            label5.Name = "label5";
            label5.Size = new Size(87, 15);
            label5.TabIndex = 7;
            label5.Text = "Next Execution";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(17, 92);
            label4.Name = "label4";
            label4.Size = new Size(83, 15);
            label4.TabIndex = 6;
            label4.Text = "Last Execution";
            // 
            // txtName
            // 
            txtName.Location = new Point(79, 6);
            txtName.Name = "txtName";
            txtName.Size = new Size(181, 23);
            txtName.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 2;
            label1.Text = "Name";
            // 
            // tabSchedule
            // 
            tabSchedule.Controls.Add(splitContainer1);
            tabSchedule.Location = new Point(4, 24);
            tabSchedule.Name = "tabSchedule";
            tabSchedule.Padding = new Padding(3);
            tabSchedule.Size = new Size(574, 419);
            tabSchedule.TabIndex = 1;
            tabSchedule.Text = "Schedule";
            tabSchedule.UseVisualStyleBackColor = true;
            // 
            // lstScheduleItems
            // 
            lstScheduleItems.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstScheduleItems.FormattingEnabled = true;
            lstScheduleItems.ItemHeight = 15;
            lstScheduleItems.Location = new Point(3, 3);
            lstScheduleItems.Name = "lstScheduleItems";
            lstScheduleItems.Size = new Size(183, 379);
            lstScheduleItems.TabIndex = 1;
            // 
            // tabActivities
            // 
            tabActivities.Controls.Add(splitContainer2);
            tabActivities.Location = new Point(4, 24);
            tabActivities.Name = "tabActivities";
            tabActivities.Size = new Size(574, 419);
            tabActivities.TabIndex = 2;
            tabActivities.Text = "Activities";
            tabActivities.UseVisualStyleBackColor = true;
            // 
            // lstActivityFields
            // 
            lstActivityFields.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstActivityFields.FormattingEnabled = true;
            lstActivityFields.ItemHeight = 15;
            lstActivityFields.Location = new Point(3, 3);
            lstActivityFields.Name = "lstActivityFields";
            lstActivityFields.Size = new Size(120, 379);
            lstActivityFields.TabIndex = 8;
            // 
            // txtFieldValue
            // 
            txtFieldValue.Location = new Point(94, 58);
            txtFieldValue.Name = "txtFieldValue";
            txtFieldValue.Size = new Size(100, 23);
            txtFieldValue.TabIndex = 7;
            // 
            // txtFieldName
            // 
            txtFieldName.AutoSize = true;
            txtFieldName.Location = new Point(100, 20);
            txtFieldName.Name = "txtFieldName";
            txtFieldName.Size = new Size(37, 15);
            txtFieldName.TabIndex = 6;
            txtFieldName.Text = "name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 57);
            label3.Name = "label3";
            label3.Size = new Size(35, 15);
            label3.TabIndex = 5;
            label3.Text = "Value";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(19, 21);
            label2.Name = "label2";
            label2.Size = new Size(39, 15);
            label2.TabIndex = 4;
            label2.Text = "Name";
            // 
            // lstActivities
            // 
            lstActivities.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstActivities.FormattingEnabled = true;
            lstActivities.ItemHeight = 15;
            lstActivities.Location = new Point(0, 3);
            lstActivities.Name = "lstActivities";
            lstActivities.Size = new Size(189, 379);
            lstActivities.TabIndex = 0;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(3, 3);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(btnScheduleDelete);
            splitContainer1.Panel1.Controls.Add(btnScheduleAdd);
            splitContainer1.Panel1.Controls.Add(lstScheduleItems);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(btnScheduleSave);
            splitContainer1.Panel2.Controls.Add(tabControl2);
            splitContainer1.Panel2.Controls.Add(txtScheduleCRON);
            splitContainer1.Panel2.Controls.Add(label10);
            splitContainer1.Panel2.Controls.Add(txtScheduleName);
            splitContainer1.Size = new Size(568, 413);
            splitContainer1.SplitterDistance = 189;
            splitContainer1.TabIndex = 5;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(10, 39);
            label10.Name = "label10";
            label10.Size = new Size(40, 15);
            label10.TabIndex = 6;
            label10.Text = "CRON";
            // 
            // txtScheduleName
            // 
            txtScheduleName.AutoSize = true;
            txtScheduleName.Location = new Point(7, 10);
            txtScheduleName.Name = "txtScheduleName";
            txtScheduleName.Size = new Size(39, 15);
            txtScheduleName.TabIndex = 5;
            txtScheduleName.Text = "Name";
            // 
            // btnScheduleAdd
            // 
            btnScheduleAdd.Location = new Point(12, 249);
            btnScheduleAdd.Name = "btnScheduleAdd";
            btnScheduleAdd.Size = new Size(46, 23);
            btnScheduleAdd.TabIndex = 2;
            btnScheduleAdd.Text = "Add";
            btnScheduleAdd.UseVisualStyleBackColor = true;
            // 
            // btnScheduleDelete
            // 
            btnScheduleDelete.Location = new Point(64, 249);
            btnScheduleDelete.Name = "btnScheduleDelete";
            btnScheduleDelete.Size = new Size(54, 23);
            btnScheduleDelete.TabIndex = 3;
            btnScheduleDelete.Text = "Delete";
            btnScheduleDelete.UseVisualStyleBackColor = true;
            // 
            // txtScheduleCRON
            // 
            txtScheduleCRON.Location = new Point(100, 39);
            txtScheduleCRON.Name = "txtScheduleCRON";
            txtScheduleCRON.Size = new Size(100, 23);
            txtScheduleCRON.TabIndex = 8;
            txtScheduleCRON.Text = "Sec Min Hour DayOfMonth Month DayOfWeek Year";
            txtScheduleCRON.TextChanged += txtScheduleCRON_TextChanged;
            // 
            // tabControl2
            // 
            tabControl2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl2.Controls.Add(tabScheduleTime);
            tabControl2.Controls.Add(tabScheduleDays);
            tabControl2.Controls.Add(tabScheduleMonth);
            tabControl2.Controls.Add(tabScheduleYear);
            tabControl2.Location = new Point(7, 68);
            tabControl2.Name = "tabControl2";
            tabControl2.SelectedIndex = 0;
            tabControl2.Size = new Size(365, 317);
            tabControl2.TabIndex = 9;
            // 
            // tabScheduleTime
            // 
            tabScheduleTime.Controls.Add(label17);
            tabScheduleTime.Controls.Add(label16);
            tabScheduleTime.Controls.Add(label15);
            tabScheduleTime.Controls.Add(label14);
            tabScheduleTime.Controls.Add(label12);
            tabScheduleTime.Controls.Add(label9);
            tabScheduleTime.Controls.Add(label8);
            tabScheduleTime.Controls.Add(label7);
            tabScheduleTime.Controls.Add(label6);
            tabScheduleTime.Location = new Point(4, 24);
            tabScheduleTime.Name = "tabScheduleTime";
            tabScheduleTime.Padding = new Padding(3);
            tabScheduleTime.Size = new Size(257, 151);
            tabScheduleTime.TabIndex = 0;
            tabScheduleTime.Text = "Time";
            tabScheduleTime.UseVisualStyleBackColor = true;
            // 
            // tabScheduleDays
            // 
            tabScheduleDays.Controls.Add(label22);
            tabScheduleDays.Controls.Add(label21);
            tabScheduleDays.Controls.Add(label20);
            tabScheduleDays.Controls.Add(label19);
            tabScheduleDays.Controls.Add(label18);
            tabScheduleDays.Location = new Point(4, 24);
            tabScheduleDays.Name = "tabScheduleDays";
            tabScheduleDays.Padding = new Padding(3);
            tabScheduleDays.Size = new Size(257, 151);
            tabScheduleDays.TabIndex = 1;
            tabScheduleDays.Text = "Days";
            tabScheduleDays.UseVisualStyleBackColor = true;
            // 
            // btnScheduleSave
            // 
            btnScheduleSave.Location = new Point(10, 249);
            btnScheduleSave.Name = "btnScheduleSave";
            btnScheduleSave.Size = new Size(75, 23);
            btnScheduleSave.TabIndex = 10;
            btnScheduleSave.Text = "Save";
            btnScheduleSave.UseVisualStyleBackColor = true;
            // 
            // tabScheduleMonth
            // 
            tabScheduleMonth.Controls.Add(label25);
            tabScheduleMonth.Controls.Add(label24);
            tabScheduleMonth.Controls.Add(label23);
            tabScheduleMonth.Location = new Point(4, 24);
            tabScheduleMonth.Name = "tabScheduleMonth";
            tabScheduleMonth.Size = new Size(257, 151);
            tabScheduleMonth.TabIndex = 2;
            tabScheduleMonth.Text = "Months";
            tabScheduleMonth.UseVisualStyleBackColor = true;
            // 
            // tabScheduleYear
            // 
            tabScheduleYear.Controls.Add(label28);
            tabScheduleYear.Controls.Add(label27);
            tabScheduleYear.Controls.Add(label26);
            tabScheduleYear.Location = new Point(4, 24);
            tabScheduleYear.Name = "tabScheduleYear";
            tabScheduleYear.Size = new Size(357, 289);
            tabScheduleYear.TabIndex = 3;
            tabScheduleYear.Text = "Years";
            tabScheduleYear.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(10, 7);
            label6.Name = "label6";
            label6.Size = new Size(75, 15);
            label6.TabIndex = 0;
            label6.Text = "Any Seconds";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(14, 32);
            label7.Name = "label7";
            label7.Size = new Size(74, 15);
            label7.TabIndex = 1;
            label7.Text = "Any Minutes";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(9, 59);
            label8.Name = "label8";
            label8.Size = new Size(63, 15);
            label8.TabIndex = 2;
            label8.Text = "Any Hours";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(14, 84);
            label9.Name = "label9";
            label9.Size = new Size(136, 15);
            label9.TabIndex = 3;
            label9.Text = "Every N hours starting at";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(17, 112);
            label12.Name = "label12";
            label12.Size = new Size(149, 15);
            label12.TabIndex = 4;
            label12.Text = "Every N minutes starting at";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(9, 132);
            label14.Name = "label14";
            label14.Size = new Size(149, 15);
            label14.TabIndex = 5;
            label14.Text = "Every N seconds starting at";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(139, 12);
            label15.Name = "label15";
            label15.Size = new Size(98, 15);
            label15.TabIndex = 6;
            label15.Text = "Selected Seconds";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(149, 34);
            label16.Name = "label16";
            label16.Size = new Size(97, 15);
            label16.TabIndex = 7;
            label16.Text = "Selected Minutes";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(156, 60);
            label17.Name = "label17";
            label17.Size = new Size(86, 15);
            label17.TabIndex = 8;
            label17.Text = "Selected Hours";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(16, 10);
            label18.Name = "label18";
            label18.Size = new Size(56, 15);
            label18.TabIndex = 0;
            label18.Text = "Any Days";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(13, 34);
            label19.Name = "label19";
            label19.Size = new Size(136, 15);
            label19.TabIndex = 1;
            label19.Text = "Certain days of the week";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(14, 62);
            label20.Name = "label20";
            label20.Size = new Size(134, 15);
            label20.TabIndex = 2;
            label20.Text = "Every N days starting on";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(13, 89);
            label21.Name = "label21";
            label21.Size = new Size(105, 15);
            label21.TabIndex = 3;
            label21.Text = "Days of the month";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(15, 114);
            label22.Name = "label22";
            label22.Size = new Size(129, 15);
            label22.TabIndex = 4;
            label22.Text = "from last day of month";
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(11, 8);
            label23.Name = "label23";
            label23.Size = new Size(65, 15);
            label23.TabIndex = 0;
            label23.Text = "All Months";
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(15, 35);
            label24.Name = "label24";
            label24.Size = new Size(95, 15);
            label24.TabIndex = 1;
            label24.Text = "Selected Months";
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(15, 68);
            label25.Name = "label25";
            label25.Size = new Size(151, 15);
            label25.TabIndex = 2;
            label25.Text = "Every N months starting on";
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(14, 8);
            label26.Name = "label26";
            label26.Size = new Size(53, 15);
            label26.TabIndex = 0;
            label26.Text = "Any Year";
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(13, 33);
            label27.Name = "label27";
            label27.Size = new Size(81, 15);
            label27.TabIndex = 1;
            label27.Text = "Selected Years";
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(15, 65);
            label28.Name = "label28";
            label28.Size = new Size(133, 15);
            label28.TabIndex = 2;
            label28.Text = "Every N years starting at";
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(btnActivitySave);
            splitContainer2.Panel1.Controls.Add(btnActivityDelete);
            splitContainer2.Panel1.Controls.Add(btnActivityAdd);
            splitContainer2.Panel1.Controls.Add(lstActivities);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(splitContainer3);
            splitContainer2.Size = new Size(574, 419);
            splitContainer2.SplitterDistance = 191;
            splitContainer2.TabIndex = 9;
            // 
            // splitContainer3
            // 
            splitContainer3.Dock = DockStyle.Fill;
            splitContainer3.Location = new Point(0, 0);
            splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(lstActivityFields);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(label2);
            splitContainer3.Panel2.Controls.Add(txtFieldValue);
            splitContainer3.Panel2.Controls.Add(label3);
            splitContainer3.Panel2.Controls.Add(txtFieldName);
            splitContainer3.Size = new Size(379, 419);
            splitContainer3.SplitterDistance = 126;
            splitContainer3.TabIndex = 0;
            // 
            // btnActivityAdd
            // 
            btnActivityAdd.Location = new Point(27, 526);
            btnActivityAdd.Name = "btnActivityAdd";
            btnActivityAdd.Size = new Size(50, 23);
            btnActivityAdd.TabIndex = 1;
            btnActivityAdd.Text = "Add";
            btnActivityAdd.UseVisualStyleBackColor = true;
            // 
            // btnActivityDelete
            // 
            btnActivityDelete.Location = new Point(83, 526);
            btnActivityDelete.Name = "btnActivityDelete";
            btnActivityDelete.Size = new Size(61, 23);
            btnActivityDelete.TabIndex = 2;
            btnActivityDelete.Text = "Delete";
            btnActivityDelete.UseVisualStyleBackColor = true;
            // 
            // btnActivitySave
            // 
            btnActivitySave.Location = new Point(150, 526);
            btnActivitySave.Name = "btnActivitySave";
            btnActivitySave.Size = new Size(61, 23);
            btnActivitySave.TabIndex = 3;
            btnActivitySave.Text = "Save";
            btnActivitySave.UseVisualStyleBackColor = true;
            // 
            // EventViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabControl1);
            Name = "EventViewer";
            Size = new Size(582, 447);
            tabControl1.ResumeLayout(false);
            tabOverview.ResumeLayout(false);
            tabOverview.PerformLayout();
            tabSchedule.ResumeLayout(false);
            tabActivities.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabControl2.ResumeLayout(false);
            tabScheduleTime.ResumeLayout(false);
            tabScheduleTime.PerformLayout();
            tabScheduleDays.ResumeLayout(false);
            tabScheduleDays.PerformLayout();
            tabScheduleMonth.ResumeLayout(false);
            tabScheduleMonth.PerformLayout();
            tabScheduleYear.ResumeLayout(false);
            tabScheduleYear.PerformLayout();
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel2.ResumeLayout(false);
            splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabOverview;
        private TabPage tabSchedule;
        private TabPage tabActivities;
        private Label lbNextExecution;
        private Label lblLastExecution;
        private Label label5;
        private Label label4;
        private TextBox txtName;
        private Label label1;
        private ListBox lstScheduleItems;
        private ListBox lstActivities;
        private CheckBox cbCatchUpOnStartup;
        private CheckBox cbIsActive;
        private TextBox txtFieldValue;
        private Label txtFieldName;
        private Label label3;
        private Label label2;
        private ListBox lstActivityFields;
        private SplitContainer splitContainer1;
        private Label label10;
        private Label txtScheduleName;
        private Button btnScheduleDelete;
        private Button btnScheduleAdd;
        private TabControl tabControl2;
        private TabPage tabScheduleTime;
        private TabPage tabScheduleDays;
        private TextBox txtScheduleCRON;
        private Button btnScheduleSave;
        private TabPage tabScheduleMonth;
        private TabPage tabScheduleYear;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label17;
        private Label label16;
        private Label label15;
        private Label label14;
        private Label label12;
        private Label label22;
        private Label label21;
        private Label label20;
        private Label label19;
        private Label label18;
        private Label label25;
        private Label label24;
        private Label label23;
        private Label label28;
        private Label label27;
        private Label label26;
        private SplitContainer splitContainer2;
        private Button btnActivityDelete;
        private Button btnActivityAdd;
        private SplitContainer splitContainer3;
        private Button btnActivitySave;
    }
}
