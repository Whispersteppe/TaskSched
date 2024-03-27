using TaskSched.Common.DataModel;

namespace TaskSched.Common.Interfaces
{
    public interface IImportExport
    {
        Task<ExpandedResult<ExportData>> ExportData();
        Task<ExpandedResult> ImportData(ExportData data);
    }

    public class ExportData
    {
        public List<Event> Events { get; set; }
        public List<Activity> Activities { get; set; }
        public List<Folder> Folders { get; set; }
    }

}
