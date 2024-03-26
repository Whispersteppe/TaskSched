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
        /// <summary>
        /// show the given item
        /// </summary>
        /// <param name="o"></param>
        Task Initialize(ScheduleManager scheduleManager, object treeItem);

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
        Task Initialize(ScheduleManager scheduleManager, T o);
    }

}
