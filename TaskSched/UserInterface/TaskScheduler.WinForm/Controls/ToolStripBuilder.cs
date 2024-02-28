using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler.WinForm.Controls
{
    internal class ToolStripBuilder
    {
        public List<ToolStripItem> ToolStripItems { get; private set; } = new List<ToolStripItem>();

        public ToolStripBuilder() 
        {
        }


        public ToolStripButton AddButton(string text, EventHandler buttonClickFunction )
        {

            ToolStripButton tsButton = new ToolStripButton();

            tsButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsButton.ImageTransparentColor = Color.Magenta;
            tsButton.Name = text;
            tsButton.Size = new Size(42, 22);
            tsButton.Text = text;

            tsButton.Click += buttonClickFunction;

            ToolStripItems.Add(tsButton);

            return tsButton;
        }

    }
}
