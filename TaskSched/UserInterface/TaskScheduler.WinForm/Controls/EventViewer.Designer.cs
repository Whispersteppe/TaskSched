namespace TaskScheduler.WinForm.Controls
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
            label12 = new Label();
            label10 = new Label();
            label9 = new Label();
            listBox1 = new ListBox();
            label8 = new Label();
            tabActivities = new TabPage();
            label13 = new Label();
            label11 = new Label();
            listBox2 = new ListBox();
            label2 = new Label();
            label3 = new Label();
            txtFieldName = new Label();
            txtFieldValue = new TextBox();
            lstFields = new ListBox();
            tabControl1.SuspendLayout();
            tabOverview.SuspendLayout();
            tabSchedule.SuspendLayout();
            tabActivities.SuspendLayout();
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
            tabControl1.Size = new Size(432, 309);
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
            tabOverview.Size = new Size(424, 281);
            tabOverview.TabIndex = 0;
            tabOverview.Text = "Overview";
            tabOverview.UseVisualStyleBackColor = true;
            // 
            // cbCatchUpOnStartup
            // 
            cbCatchUpOnStartup.AutoSize = true;
            cbCatchUpOnStartup.Location = new Point(64, 63);
            cbCatchUpOnStartup.Name = "cbCatchUpOnStartup";
            cbCatchUpOnStartup.Size = new Size(131, 19);
            cbCatchUpOnStartup.TabIndex = 11;
            cbCatchUpOnStartup.Text = "Catch up on startup";
            cbCatchUpOnStartup.UseVisualStyleBackColor = true;
            // 
            // cbIsActive
            // 
            cbIsActive.AutoSize = true;
            cbIsActive.Location = new Point(64, 38);
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
            tabSchedule.Controls.Add(label12);
            tabSchedule.Controls.Add(label10);
            tabSchedule.Controls.Add(label9);
            tabSchedule.Controls.Add(listBox1);
            tabSchedule.Controls.Add(label8);
            tabSchedule.Location = new Point(4, 24);
            tabSchedule.Name = "tabSchedule";
            tabSchedule.Padding = new Padding(3);
            tabSchedule.Size = new Size(424, 281);
            tabSchedule.TabIndex = 1;
            tabSchedule.Text = "Schedule";
            tabSchedule.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(144, 105);
            label12.Name = "label12";
            label12.Size = new Size(80, 15);
            label12.TabIndex = 4;
            label12.Text = "CRON Builder";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(128, 40);
            label10.Name = "label10";
            label10.Size = new Size(40, 15);
            label10.TabIndex = 3;
            label10.Text = "CRON";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(125, 11);
            label9.Name = "label9";
            label9.Size = new Size(39, 15);
            label9.TabIndex = 2;
            label9.Text = "Name";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(10, 30);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(85, 94);
            listBox1.TabIndex = 1;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(6, 9);
            label8.Name = "label8";
            label8.Size = new Size(60, 15);
            label8.TabIndex = 0;
            label8.Text = "Schedules";
            // 
            // tabActivities
            // 
            tabActivities.Controls.Add(lstFields);
            tabActivities.Controls.Add(txtFieldValue);
            tabActivities.Controls.Add(txtFieldName);
            tabActivities.Controls.Add(label3);
            tabActivities.Controls.Add(label2);
            tabActivities.Controls.Add(label13);
            tabActivities.Controls.Add(label11);
            tabActivities.Controls.Add(listBox2);
            tabActivities.Location = new Point(4, 24);
            tabActivities.Name = "tabActivities";
            tabActivities.Size = new Size(424, 281);
            tabActivities.TabIndex = 2;
            tabActivities.Text = "Activities";
            tabActivities.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(182, 5);
            label13.Name = "label13";
            label13.Size = new Size(80, 15);
            label13.TabIndex = 3;
            label13.Text = "Activity Fields";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(23, 6);
            label11.Name = "label11";
            label11.Size = new Size(55, 15);
            label11.TabIndex = 1;
            label11.Text = "Activities";
            // 
            // listBox2
            // 
            listBox2.FormattingEnabled = true;
            listBox2.ItemHeight = 15;
            listBox2.Location = new Point(12, 29);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(120, 244);
            listBox2.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(230, 66);
            label2.Name = "label2";
            label2.Size = new Size(39, 15);
            label2.TabIndex = 4;
            label2.Text = "Name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(227, 102);
            label3.Name = "label3";
            label3.Size = new Size(35, 15);
            label3.TabIndex = 5;
            label3.Text = "Value";
            // 
            // txtFieldName
            // 
            txtFieldName.AutoSize = true;
            txtFieldName.Location = new Point(311, 65);
            txtFieldName.Name = "txtFieldName";
            txtFieldName.Size = new Size(37, 15);
            txtFieldName.TabIndex = 6;
            txtFieldName.Text = "name";
            // 
            // txtFieldValue
            // 
            txtFieldValue.Location = new Point(305, 103);
            txtFieldValue.Name = "txtFieldValue";
            txtFieldValue.Size = new Size(100, 23);
            txtFieldValue.TabIndex = 7;
            // 
            // lstFields
            // 
            lstFields.FormattingEnabled = true;
            lstFields.ItemHeight = 15;
            lstFields.Location = new Point(142, 102);
            lstFields.Name = "lstFields";
            lstFields.Size = new Size(71, 94);
            lstFields.TabIndex = 8;
            // 
            // EventViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabControl1);
            Name = "EventViewer";
            Size = new Size(432, 309);
            tabControl1.ResumeLayout(false);
            tabOverview.ResumeLayout(false);
            tabOverview.PerformLayout();
            tabSchedule.ResumeLayout(false);
            tabSchedule.PerformLayout();
            tabActivities.ResumeLayout(false);
            tabActivities.PerformLayout();
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
        private Label label12;
        private Label label10;
        private Label label9;
        private ListBox listBox1;
        private Label label8;
        private Label label13;
        private Label label11;
        private ListBox listBox2;
        private CheckBox cbCatchUpOnStartup;
        private CheckBox cbIsActive;
        private TextBox txtFieldValue;
        private Label txtFieldName;
        private Label label3;
        private Label label2;
        private ListBox lstFields;
    }
}
