using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.DataModel;
using TaskSched.Common.Interfaces;

namespace TaskSched.DataStore
{
    public class ImportExport : IImportExport
    {
        IEventStore _eventStore; 
        IFolderStore _folderStore; 
        IActivityStore _activityStore;

        public ImportExport(IEventStore eventStore, IFolderStore folderStore, IActivityStore activityStore) 
        {
            _eventStore = eventStore;
            _folderStore = folderStore;
            _activityStore = activityStore;

        }

        public async Task<ExpandedResult<ExportData>> ExportData()
        {
            ExpandedResult<ExportData> result = new ExpandedResult<ExportData>()
            {
                Result = new ExportData()
                {
                    Activities = new List<Activity>(),
                    Folders = new List<Folder>(),
                    Events = new List<Event>()
                }
            };

            //  get activities
            var getActivitiesResult = await _activityStore.GetAll();
            result.Result.Activities.AddRange(getActivitiesResult.Result);

            //  get events
            var getEventResult = await _eventStore.GetAll();
            result.Result.Events.AddRange(getEventResult.Result);

            //  get folders
            var getFolderResult = await _folderStore.GetAll(new FolderRetrievalParameters() { AddChildEvents = false, AsTree = true});
            result.Result.Folders.AddRange(getFolderResult.Result);

            return result;


        }

        public async Task<ExpandedResult> ImportData(ExportData data)
        {

            ExpandedResult result = new ExpandedResult();

            foreach (var activity in data.Activities)
            {

            }

            foreach(var folder in data.Folders)
            {

            }

            foreach(var eventItem in data.Events)
            {

            }

            return result;
        }
    }
}
