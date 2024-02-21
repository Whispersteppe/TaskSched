using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        void ShowItem(object o);

        /// <summary>
        /// checks to see whether the current item can be closed or not
        /// </summary>
        /// <returns></returns>
        bool CanClose();
    }

    internal interface ICanvasItem<T> : ICanvasItem
    {
        void ShowItem(T o);
    }
}
