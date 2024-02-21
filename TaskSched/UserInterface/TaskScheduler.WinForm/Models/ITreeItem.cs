namespace TaskScheduler.WinForm.Models
{
    /// <summary>
    /// Tree items surrounding the various data models
    /// </summary>
    public interface ITreeItem
    {
        /// <summary>
        /// text that will get displayed on the treeview
        /// </summary>
        string Name { get; }

        /// <summary>
        /// any children on this tree item
        /// </summary>
        List<ITreeItem>? Children { get; }

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
