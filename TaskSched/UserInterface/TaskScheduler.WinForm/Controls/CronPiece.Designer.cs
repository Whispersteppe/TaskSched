namespace TaskScheduler.WinForm.Controls
{
    partial class CronPiece
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
            lstComponentType = new ComboBox();
            pnlAllowAny = new Panel();
            label15 = new Label();
            pnlIgnore = new Panel();
            label14 = new Label();
            pnlRange = new Panel();
            listBox2 = new ListBox();
            label10 = new Label();
            pnlRepeating = new Panel();
            comboBox13 = new ComboBox();
            comboBox9 = new ComboBox();
            label5 = new Label();
            label9 = new Label();
            pnlWeekday = new Panel();
            comboBox11 = new ComboBox();
            label12 = new Label();
            pnlFromEndOfMonth = new Panel();
            label11 = new Label();
            comboBox10 = new ComboBox();
            pnlWeekNthOccurance = new Panel();
            comboBox14 = new ComboBox();
            comboBox12 = new ComboBox();
            label13 = new Label();
            pnlCanvas = new Panel();
            pnlAllowAny.SuspendLayout();
            pnlIgnore.SuspendLayout();
            pnlRange.SuspendLayout();
            pnlRepeating.SuspendLayout();
            pnlWeekday.SuspendLayout();
            pnlFromEndOfMonth.SuspendLayout();
            pnlWeekNthOccurance.SuspendLayout();
            pnlCanvas.SuspendLayout();
            SuspendLayout();
            // 
            // lstComponentType
            // 
            lstComponentType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lstComponentType.DropDownStyle = ComboBoxStyle.DropDownList;
            lstComponentType.FormattingEnabled = true;
            lstComponentType.Items.AddRange(new object[] { "Allow Any", "Repeating", "Range", "Last Day of Month", "Last Weekday of Month", "Nth Weekday", "Ignored" });
            lstComponentType.Location = new Point(3, 3);
            lstComponentType.Name = "lstComponentType";
            lstComponentType.Size = new Size(439, 23);
            lstComponentType.TabIndex = 8;
            lstComponentType.SelectionChangeCommitted += lstComponentType_SelectionChangeCommitted;
            // 
            // pnlAllowAny
            // 
            pnlAllowAny.Controls.Add(label15);
            pnlAllowAny.Controls.Add(pnlIgnore);
            pnlAllowAny.Location = new Point(8, 20);
            pnlAllowAny.Name = "pnlAllowAny";
            pnlAllowAny.Size = new Size(173, 84);
            pnlAllowAny.TabIndex = 9;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(62, 35);
            label15.Name = "label15";
            label15.Size = new Size(61, 15);
            label15.TabIndex = 1;
            label15.Text = "Allow Any";
            // 
            // pnlIgnore
            // 
            pnlIgnore.Controls.Add(label14);
            pnlIgnore.Location = new Point(3, 16);
            pnlIgnore.Name = "pnlIgnore";
            pnlIgnore.Size = new Size(191, 79);
            pnlIgnore.TabIndex = 16;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(71, 32);
            label14.Name = "label14";
            label14.Size = new Size(48, 15);
            label14.TabIndex = 1;
            label14.Text = "Ignored";
            // 
            // pnlRange
            // 
            pnlRange.Controls.Add(listBox2);
            pnlRange.Controls.Add(label10);
            pnlRange.Location = new Point(8, 197);
            pnlRange.Name = "pnlRange";
            pnlRange.Size = new Size(173, 79);
            pnlRange.TabIndex = 10;
            // 
            // listBox2
            // 
            listBox2.FormattingEnabled = true;
            listBox2.ItemHeight = 15;
            listBox2.Location = new Point(35, 13);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(120, 94);
            listBox2.TabIndex = 3;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(18, -28);
            label10.Name = "label10";
            label10.Size = new Size(83, 15);
            label10.TabIndex = 2;
            label10.Text = "Selected items";
            // 
            // pnlRepeating
            // 
            pnlRepeating.Controls.Add(comboBox13);
            pnlRepeating.Controls.Add(comboBox9);
            pnlRepeating.Controls.Add(label5);
            pnlRepeating.Controls.Add(label9);
            pnlRepeating.Location = new Point(8, 110);
            pnlRepeating.Name = "pnlRepeating";
            pnlRepeating.Size = new Size(173, 81);
            pnlRepeating.TabIndex = 11;
            // 
            // comboBox13
            // 
            comboBox13.FormattingEnabled = true;
            comboBox13.Location = new Point(100, 30);
            comboBox13.Name = "comboBox13";
            comboBox13.Size = new Size(45, 23);
            comboBox13.TabIndex = 4;
            // 
            // comboBox9
            // 
            comboBox9.FormattingEnabled = true;
            comboBox9.Location = new Point(88, 7);
            comboBox9.Name = "comboBox9";
            comboBox9.Size = new Size(31, 23);
            comboBox9.TabIndex = 6;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(15, 33);
            label5.Name = "label5";
            label5.Size = new Size(93, 15);
            label5.TabIndex = 5;
            label5.Text = "Units starting at ";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(47, 8);
            label9.Name = "label9";
            label9.Size = new Size(35, 15);
            label9.TabIndex = 4;
            label9.Text = "Every";
            // 
            // pnlWeekday
            // 
            pnlWeekday.Controls.Add(comboBox11);
            pnlWeekday.Controls.Add(label12);
            pnlWeekday.Location = new Point(218, 20);
            pnlWeekday.Name = "pnlWeekday";
            pnlWeekday.Size = new Size(169, 85);
            pnlWeekday.TabIndex = 12;
            // 
            // comboBox11
            // 
            comboBox11.FormattingEnabled = true;
            comboBox11.Location = new Point(8, 30);
            comboBox11.Name = "comboBox11";
            comboBox11.Size = new Size(121, 23);
            comboBox11.TabIndex = 3;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(13, 8);
            label12.Name = "label12";
            label12.Size = new Size(55, 15);
            label12.TabIndex = 2;
            label12.Text = "Weekday";
            // 
            // pnlFromEndOfMonth
            // 
            pnlFromEndOfMonth.Controls.Add(label11);
            pnlFromEndOfMonth.Controls.Add(comboBox10);
            pnlFromEndOfMonth.Location = new Point(202, 199);
            pnlFromEndOfMonth.Name = "pnlFromEndOfMonth";
            pnlFromEndOfMonth.Size = new Size(173, 77);
            pnlFromEndOfMonth.TabIndex = 13;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(15, 38);
            label11.Name = "label11";
            label11.Size = new Size(111, 15);
            label11.TabIndex = 3;
            label11.Text = "From end of month";
            // 
            // comboBox10
            // 
            comboBox10.FormattingEnabled = true;
            comboBox10.Location = new Point(17, 12);
            comboBox10.Name = "comboBox10";
            comboBox10.Size = new Size(91, 23);
            comboBox10.TabIndex = 2;
            // 
            // pnlWeekNthOccurance
            // 
            pnlWeekNthOccurance.Controls.Add(comboBox14);
            pnlWeekNthOccurance.Controls.Add(comboBox12);
            pnlWeekNthOccurance.Controls.Add(label13);
            pnlWeekNthOccurance.Location = new Point(216, 126);
            pnlWeekNthOccurance.Name = "pnlWeekNthOccurance";
            pnlWeekNthOccurance.Size = new Size(191, 79);
            pnlWeekNthOccurance.TabIndex = 17;
            // 
            // comboBox14
            // 
            comboBox14.FormattingEnabled = true;
            comboBox14.Location = new Point(13, 38);
            comboBox14.Name = "comboBox14";
            comboBox14.Size = new Size(121, 23);
            comboBox14.TabIndex = 4;
            // 
            // comboBox12
            // 
            comboBox12.FormattingEnabled = true;
            comboBox12.Location = new Point(3, 9);
            comboBox12.Name = "comboBox12";
            comboBox12.Size = new Size(45, 23);
            comboBox12.TabIndex = 3;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(54, 11);
            label13.Name = "label13";
            label13.Size = new Size(79, 15);
            label13.TabIndex = 2;
            label13.Text = "occurance of ";
            // 
            // pnlCanvas
            // 
            pnlCanvas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlCanvas.Controls.Add(pnlAllowAny);
            pnlCanvas.Controls.Add(pnlWeekNthOccurance);
            pnlCanvas.Controls.Add(pnlRange);
            pnlCanvas.Controls.Add(pnlWeekday);
            pnlCanvas.Controls.Add(pnlRepeating);
            pnlCanvas.Controls.Add(pnlFromEndOfMonth);
            pnlCanvas.Location = new Point(0, 32);
            pnlCanvas.Name = "pnlCanvas";
            pnlCanvas.Size = new Size(445, 328);
            pnlCanvas.TabIndex = 18;
            // 
            // CronPiece
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlCanvas);
            Controls.Add(lstComponentType);
            Name = "CronPiece";
            Size = new Size(445, 366);
            Load += CronPiece_Load;
            pnlAllowAny.ResumeLayout(false);
            pnlAllowAny.PerformLayout();
            pnlIgnore.ResumeLayout(false);
            pnlIgnore.PerformLayout();
            pnlRange.ResumeLayout(false);
            pnlRange.PerformLayout();
            pnlRepeating.ResumeLayout(false);
            pnlRepeating.PerformLayout();
            pnlWeekday.ResumeLayout(false);
            pnlWeekday.PerformLayout();
            pnlFromEndOfMonth.ResumeLayout(false);
            pnlFromEndOfMonth.PerformLayout();
            pnlWeekNthOccurance.ResumeLayout(false);
            pnlWeekNthOccurance.PerformLayout();
            pnlCanvas.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private RadioButton btnAllowAny;
        private RadioButton btnRepeating;
        private RadioButton btnRange;
        private RadioButton btnLastDayOfMonth;
        private RadioButton btnLastWeekdayOfMonth;
        private RadioButton btnNthWeekday;
        private RadioButton btnIgnored;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TabPage tabPage6;
        private TabPage tabPage7;
        private Label label2;
        private Label label1;
        private Label label3;
        private ComboBox comboBox5;
        private ComboBox comboBox4;
        private ListBox listBox1;
        private Label label4;
        private ComboBox comboBox6;
        private ComboBox comboBox1;
        private Label label6;
        private ComboBox comboBox3;
        private ComboBox comboBox2;
        private Label label7;
        private Label label8;
        private ComboBox lstComponentType;
        private Panel pnlAllowAny;
        private Label label15;
        private Panel pnlRange;
        private ListBox listBox2;
        private Label label10;
        private Panel pnlRepeating;
        private ComboBox comboBox13;
        private ComboBox comboBox9;
        private Label label5;
        private Label label9;
        private Panel pnlWeekday;
        private ComboBox comboBox11;
        private Label label12;
        private Panel pnlFromEndOfMonth;
        private Label label11;
        private ComboBox comboBox10;
        private Panel pnlIgnore;
        private Label label14;
        private Panel pnlWeekNthOccurance;
        private ComboBox comboBox14;
        private ComboBox comboBox12;
        private Label label13;
        private Panel pnlCanvas;
    }
}
