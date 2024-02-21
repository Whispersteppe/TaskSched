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
            label1 = new Label();
            label2 = new Label();
            txtName = new TextBox();
            cmbActivityHandler = new ComboBox();
            label3 = new Label();
            lvFields = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(7, 12);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 0;
            label1.Text = "Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(7, 45);
            label2.Name = "label2";
            label2.Size = new Size(68, 15);
            label2.TabIndex = 1;
            label2.Text = "Handled By";
            // 
            // txtName
            // 
            txtName.Location = new Point(97, 13);
            txtName.Name = "txtName";
            txtName.Size = new Size(154, 23);
            txtName.TabIndex = 2;
            // 
            // cmbActivityHandler
            // 
            cmbActivityHandler.FormattingEnabled = true;
            cmbActivityHandler.Location = new Point(97, 42);
            cmbActivityHandler.Name = "cmbActivityHandler";
            cmbActivityHandler.Size = new Size(121, 23);
            cmbActivityHandler.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(10, 83);
            label3.Name = "label3";
            label3.Size = new Size(37, 15);
            label3.TabIndex = 4;
            label3.Text = "Fields";
            // 
            // lvFields
            // 
            lvFields.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4 });
            lvFields.Location = new Point(15, 105);
            lvFields.Name = "lvFields";
            lvFields.Size = new Size(301, 77);
            lvFields.TabIndex = 5;
            lvFields.UseCompatibleStateImageBehavior = false;
            lvFields.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Name";
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Field Type";
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Read Only";
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Value";
            // 
            // ActivityViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lvFields);
            Controls.Add(label3);
            Controls.Add(cmbActivityHandler);
            Controls.Add(txtName);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "ActivityViewer";
            Size = new Size(358, 198);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtName;
        private ComboBox cmbActivityHandler;
        private Label label3;
        private ListView lvFields;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
    }
}
