using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms
{
    public class ClearingTextBox : TextBox
    {
        private readonly Label lblTheClose;

        public bool ButtonTextClear { get; set; } = true;

        public ClearingTextBox()
        {
            Resize += PositionX;

            TextChanged += ShowHideX;

            lblTheClose = new Label()
            {
                Location = new Point(100, 0),
                AutoSize = true,
                Text = "x",
                ForeColor = Color.Gray,
                Visible = false,
                Font = new Font("Tahoma", 8.25F),
                Cursor = Cursors.Arrow
            };

            Controls.Add(lblTheClose);
            lblTheClose.Click += (ss, ee) => { ((Label)ss).Visible = false; Text = string.Empty; };
            lblTheClose.BringToFront();
        }

        private void ShowHideX(object sender, EventArgs e) => lblTheClose.Visible = ButtonTextClear && !string.IsNullOrEmpty(Text);
        private void PositionX(object sender, EventArgs e) => lblTheClose.Location = new Point(Width - 15, ((Height - lblTheClose.Height) / 2) - 3);
    }
}

