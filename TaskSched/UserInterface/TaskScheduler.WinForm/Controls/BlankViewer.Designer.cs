﻿namespace TaskScheduler.WinForm.Controls
{
    partial class BlankViewer
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
            txtName = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 12);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 0;
            label1.Text = "Name";
            // 
            // txtName
            // 
            txtName.AutoSize = true;
            txtName.Location = new Point(82, 15);
            txtName.Name = "txtName";
            txtName.Size = new Size(38, 15);
            txtName.TabIndex = 1;
            txtName.Text = "label2";
            // 
            // BlankViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(txtName);
            Controls.Add(label1);
            Name = "BlankViewer";
            Size = new Size(177, 150);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label txtName;
    }
}
