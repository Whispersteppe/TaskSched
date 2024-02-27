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
        void ShowItem(object o);


        void SetScheduleManager(ScheduleManager scheduleManager);

        /// <summary>
        /// checks to see whether the current item can be closed or not
        /// </summary>
        /// <returns></returns>
        bool CanClose();

        ITreeItem? TreeItem { get; }

    }

    internal interface ICanvasItem<T> : ICanvasItem
    {
        void ShowItem(T o);
    }

    public interface ICanvasItemHasChildren
    {
        List<TreeItemTypeEnum> AllowedChildTypes { get; }
        Task<ITreeItem?> CreateChild(TreeItemTypeEnum itemType);
        bool CanCreateChild(TreeItemTypeEnum itemType);

    }

    public interface ICanvasItemCanDelete
    {
        void Delete();
    }

    public interface ICanvasItemCanEdit
    {
        /// <summary>
        /// save this item
        /// </summary>
        void Save();

        /// <summary>
        /// revert tjos ote, back to the last saved version
        /// </summary>
        void Revert();
    }
}
