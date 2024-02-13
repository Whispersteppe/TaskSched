using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.DataModel;
using TaskSched.Common.Interfaces;

namespace TaskSched.Test.DummyComponents
{
    internal class InMemoryEventStore : IEventStore
    {

        Dictionary<Guid, Event> _events;

        public InMemoryEventStore()
        {
            _events = new Dictionary<Guid, Event>();
        }
        public async Task<ExpandedResult<Guid>> Create(Event eventItem)
        {
            eventItem.Id = Guid.NewGuid();
            eventItem.Schedules.ForEach(schedule => { schedule.Id = Guid.NewGuid(); });

            _events.Add(eventItem.Id, eventItem);

            return new ExpandedResult<Guid>()
            {
                Result = eventItem.Id,
            };
        }

        public async Task<ExpandedResult> Delete(Guid eventId)
        {
            if (_events.ContainsKey(eventId))
            {
                _events.Remove(eventId);
            }
            return new ExpandedResult() { };
        }

        public async Task<ExpandedResult<Event?>> Get(Guid eventId)
        {
            return new ExpandedResult<Event?>()
            {
                Result = _events.ContainsKey(eventId) ? _events[eventId] : null
            };
        }

        public async Task<ExpandedResult<List<Event>>> GetAll()
        {
            var rslt = new ExpandedResult<List<Event>>()
            {
                Result = new List<Event>()
            };

            foreach (var item in _events)
            {
                rslt.Result.Add(item.Value);
            }

            return rslt;
        }

        public async Task<ExpandedResult> MoveEvent(Guid eventId, Guid? newParentCalendarId)
        {
            if (_events.ContainsKey(eventId))
            {
                _events[eventId].CalendarId = newParentCalendarId;
            }

            return new ExpandedResult() { };
        }



        public async Task<ExpandedResult> Update(Event eventItem)
        {
            _events[eventItem.Id] = eventItem;
            return new ExpandedResult() { };
        }
    }
}
