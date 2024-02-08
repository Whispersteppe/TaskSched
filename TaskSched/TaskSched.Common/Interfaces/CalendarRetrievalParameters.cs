namespace TaskSched.Common.Interfaces
{
    /// <summary>
    /// retrieval parameters for get all calendars
    /// </summary>
    public class CalendarRetrievalParameters
    {
        public bool AsTree { get; set; }
        public bool AddChildFolders { get; set; }
        public bool AddChildEvents { get; set; }
    }
}
