namespace TaskSched.Common.Interfaces
{
    /// <summary>
    /// retrieval parameters for get all folders
    /// </summary>
    public class FolderRetrievalParameters
    {
        public bool AsTree { get; set; }
        public bool AddChildFolders { get; set; }
        public bool AddChildEvents { get; set; }
    }
}
