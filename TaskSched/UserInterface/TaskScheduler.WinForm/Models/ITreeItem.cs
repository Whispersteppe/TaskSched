using System.ComponentModel;

namespace TaskScheduler.WinForm.Models
{


    /// <summary>
    /// Tree items surrounding the various data models
    /// </summary>
    public interface ITreeItem : INotifyPropertyChanged
    {

        /// <summary>
        /// text that will get displayed on the treeview
        /// </summary>
        string DisplayName { get; }
        Guid? ID { get; }

        ITreeItem? ParentItem { get; }

        TreeItemTypeEnum TreeItemType { get; }

        bool CanMoveItem(ITreeItem possibleNewParent);
        bool CanAddItem(ITreeItem possibleNewChild);
        List<TreeItemTypeEnum> AllowedMoveToParentTypes { get; }
        List<TreeItemTypeEnum> AllowedChildTypes { get; }
        bool CanHaveChildren();

        List<ITreeItem> Children { get; }

        bool CanAddCreateChild(TreeItemTypeEnum itemType);

        bool CanDeleteItem();

        bool CanEdit();

        bool ContainsText(string text);

    }


}
