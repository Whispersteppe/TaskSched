using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.Interfaces;
using TaskSched.DataStore.DataModel;
using Model = TaskSched.Common.DataModel;
using Db = TaskSched.DataStore.DataModel;
using TaskSched.Common.Extensions;

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

        public async Task<Model.ExpandedResult<ExportData>> ExportData()
        {
            Model.ExpandedResult<ExportData> result = new Model.ExpandedResult<ExportData>()
            {
                Result = new ExportData()
                {
                    Activities = new List<Model.Activity>(),
                    Folders = new List<Model.Folder>(),
                    Events = new List<Model.Event>()
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

        public async Task<Model.ExpandedResult> ImportData(ExportData data)
        {

            Dictionary<Guid, Guid> activityConvert = new Dictionary<Guid, Guid>();
            Dictionary<Guid, Guid> folderConvert = new Dictionary<Guid, Guid>();
            Dictionary<Guid, Guid> eventConvert = new Dictionary<Guid, Guid>();

            Model.ExpandedResult result = new Model.ExpandedResult();

            foreach (var activity in data.Activities)
            {
                Guid oldId = activity.Id;
                activity.SetForCreate();
                var rslt = await _activityStore.Create(activity);
                activityConvert.Add(oldId, rslt.Result);
            }

            foreach(var folder in data.Folders)
            {
                Guid oldId = folder.Id;
                folder.SetForCreate();

                if (folder.ParentFolderId != null)
                {
                    if (folderConvert.ContainsKey(folder.ParentFolderId.Value))
                    {
                        folder.ParentFolderId = folderConvert[folder.ParentFolderId.Value];
                    }
                }
                var rslt = await _folderStore.Create(folder);
                folderConvert.Add(oldId, rslt.Result);
            }

            foreach(var eventItem in data.Events)
            {
                Guid oldId = eventItem.Id;
                eventItem.SetForCreate();

                var rslt = await _eventStore.Create(eventItem);
            }

            return result;
        }
    }
}
