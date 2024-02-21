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
        ICalendarStore _calendarStore; 
        IActivityStore _activityStore;

        public ImportExport(IEventStore eventStore, ICalendarStore calendarStore, IActivityStore activityStore) 
        {
            _eventStore = eventStore;
            _calendarStore = calendarStore;
            _activityStore = activityStore;

        }

        public async Task<ExpandedResult<ExportData>> ExportData()
        {
            ExpandedResult<ExportData> result = new ExpandedResult<ExportData>()
            {
                Result = new ExportData()
                {
                    Activities = new List<Activity>(),
                    Calendars = new List<Calendar>(),
                    Events = new List<Event>()
                }
            };

            //  get activities
            var getActivitiesResult = await _activityStore.GetAll();
            result.Result.Activities.AddRange(getActivitiesResult.Result);

            //  get events
            var getEventResult = await _eventStore.GetAll();
            result.Result.Events.AddRange(getEventResult.Result);

            //  get calendars
            var getCalendarResult = await _calendarStore.GetAll(new CalendarRetrievalParameters() { AddChildEvents = false, AsTree = true});
            result.Result.Calendars.AddRange(getCalendarResult.Result);

            return result;


        }

        public async Task<ExpandedResult> ImportData(ExportData data)
        {

            ExpandedResult result = new ExpandedResult();

            foreach (var activity in data.Activities)
            {

            }

            foreach(var calendar in data.Calendars)
            {

            }

            foreach(var eventItem in data.Events)
            {

            }

            return result;
        }
    }
}
