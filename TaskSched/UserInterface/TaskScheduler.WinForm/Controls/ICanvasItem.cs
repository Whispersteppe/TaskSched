using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskScheduler.WinForm.Models;

namespace TaskScheduler.WinForm.Controls
{
    /// <summary>
    /// indicates a control that I will be using within the CanvasSelector
    /// </summary>
    internal interface ICanvasItem
    {
        List<ToolStripItem> ToolStripItems { get; }

        /// <summary>
        /// checks to see whether the current item can be closed or not
        /// </summary>
        /// <returns></returns>
        Task LeavingItem();

        ITreeItem? TreeItem { get; }

    }

    internal interface ICanvasItem<T> : ICanvasItem
    {
    }

}
