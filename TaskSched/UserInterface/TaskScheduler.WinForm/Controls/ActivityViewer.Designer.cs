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
            groupBox1 = new GroupBox();
            cmbFieldType = new ComboBox();
            btnSaveField = new Button();
            btnDeleteField = new Button();
            btnAddField = new Button();
            chkFieldRequiredByHandler = new CheckBox();
            txtFieldDefault = new TextBox();
            txtFieldName = new TextBox();
            label6 = new Label();
            label4 = new Label();
            label3 = new Label();
            lstFields = new ListBox();
            groupBox1.SuspendLayout();
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
            txtName.Size = new Size(255, 23);
            txtName.TabIndex = 2;
            // 
            // cmbActivityHandler
            // 
            cmbActivityHandler.FormattingEnabled = true;
            cmbActivityHandler.Location = new Point(97, 42);
            cmbActivityHandler.Name = "cmbActivityHandler";
            cmbActivityHandler.Size = new Size(182, 23);
            cmbActivityHandler.TabIndex = 3;
            cmbActivityHandler.SelectedValueChanged += cmbActivityHandler_SelectedValueChanged;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(cmbFieldType);
            groupBox1.Controls.Add(btnSaveField);
            groupBox1.Controls.Add(btnDeleteField);
            groupBox1.Controls.Add(btnAddField);
            groupBox1.Controls.Add(chkFieldRequiredByHandler);
            groupBox1.Controls.Add(txtFieldDefault);
            groupBox1.Controls.Add(txtFieldName);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(lstFields);
            groupBox1.Location = new Point(22, 82);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(371, 182);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupFields";
            // 
            // cmbFieldType
            // 
            cmbFieldType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFieldType.FormattingEnabled = true;
            cmbFieldType.Location = new Point(241, 56);
            cmbFieldType.Name = "cmbFieldType";
            cmbFieldType.Size = new Size(121, 23);
            cmbFieldType.TabIndex = 13;
            // 
            // btnSaveField
            // 
            btnSaveField.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSaveField.Location = new Point(180, 152);
            btnSaveField.Name = "btnSaveField";
            btnSaveField.Size = new Size(75, 23);
            btnSaveField.TabIndex = 12;
            btnSaveField.Text = "Save";
            btnSaveField.UseVisualStyleBackColor = true;
            btnSaveField.Click += btnSaveField_Click;
            // 
            // btnDeleteField
            // 
            btnDeleteField.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDeleteField.Location = new Point(99, 152);
            btnDeleteField.Name = "btnDeleteField";
            btnDeleteField.Size = new Size(75, 23);
            btnDeleteField.TabIndex = 11;
            btnDeleteField.Text = "Delete";
            btnDeleteField.UseVisualStyleBackColor = true;
            btnDeleteField.Click += btnDeleteField_Click;
            // 
            // btnAddField
            // 
            btnAddField.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAddField.Location = new Point(15, 152);
            btnAddField.Name = "btnAddField";
            btnAddField.Size = new Size(75, 23);
            btnAddField.TabIndex = 10;
            btnAddField.Text = "Add";
            btnAddField.UseVisualStyleBackColor = true;
            btnAddField.Click += btnAddField_Click;
            // 
            // chkFieldRequiredByHandler
            // 
            chkFieldRequiredByHandler.AutoSize = true;
            chkFieldRequiredByHandler.CheckAlign = ContentAlignment.MiddleRight;
            chkFieldRequiredByHandler.Enabled = false;
            chkFieldRequiredByHandler.Location = new Point(181, 84);
            chkFieldRequiredByHandler.Name = "chkFieldRequiredByHandler";
            chkFieldRequiredByHandler.Size = new Size(76, 19);
            chkFieldRequiredByHandler.TabIndex = 9;
            chkFieldRequiredByHandler.Text = "Required ";
            chkFieldRequiredByHandler.UseVisualStyleBackColor = true;
            // 
            // txtFieldDefault
            // 
            txtFieldDefault.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtFieldDefault.Location = new Point(245, 109);
            txtFieldDefault.Name = "txtFieldDefault";
            txtFieldDefault.Size = new Size(100, 23);
            txtFieldDefault.TabIndex = 8;
            // 
            // txtFieldName
            // 
            txtFieldName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtFieldName.Location = new Point(245, 26);
            txtFieldName.Name = "txtFieldName";
            txtFieldName.Size = new Size(100, 23);
            txtFieldName.TabIndex = 5;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(137, 112);
            label6.Name = "label6";
            label6.Size = new Size(76, 15);
            label6.TabIndex = 4;
            label6.Text = "Default Value";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(147, 58);
            label4.Name = "label4";
            label4.Size = new Size(31, 15);
            label4.TabIndex = 2;
            label4.Text = "Type";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(147, 29);
            label3.Name = "label3";
            label3.Size = new Size(39, 15);
            label3.TabIndex = 1;
            label3.Text = "Name";
            // 
            // lstFields
            // 
            lstFields.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lstFields.FormattingEnabled = true;
            lstFields.ItemHeight = 15;
            lstFields.Location = new Point(11, 24);
            lstFields.Name = "lstFields";
            lstFields.Size = new Size(120, 124);
            lstFields.TabIndex = 0;
            lstFields.SelectedIndexChanged += lstFields_SelectedIndexChanged;
            // 
            // ActivityViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox1);
            Controls.Add(cmbActivityHandler);
            Controls.Add(txtName);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "ActivityViewer";
            Size = new Size(413, 277);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtName;
        private ComboBox cmbActivityHandler;
        private GroupBox groupBox1;
        private Label label6;
        private Label label4;
        private Label label3;
        private ListBox lstFields;
        private TextBox txtFieldDefault;
        private TextBox txtFieldName;
        private Button btnDeleteField;
        private Button btnAddField;
        private CheckBox chkFieldRequiredByHandler;
        private Button btnSaveField;
        private ComboBox cmbFieldType;
    }
}
