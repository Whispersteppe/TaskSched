using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.DataModel;

namespace TaskSched.Common.Interfaces
{
    /// <summary>
    /// repository interface for events
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        /// Get a single event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        Task<ExpandedResult<Event?>> Get(Guid eventId);

        /// <summary>
        /// get all events
        /// </summary>
        /// <returns></returns>
        Task<ExpandedResult<List<Event>>> GetAll();

        /// <summary>
        /// update an event
        /// </summary>
        /// <param name="eventItem"></param>
        /// <returns></returns>
        Task<ExpandedResult> Update(Event eventItem);

        /// <summary>
        /// remove an event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        Task<ExpandedResult> Delete(Guid eventId);

        /// <summary>
        /// create an event
        /// </summary>
        /// <param name="eventItem"></param>
        /// <returns></returns>
        Task<ExpandedResult<Guid>> Create(Event eventItem);

        /// <summary>
        /// move an event to a new parent
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="newParentCalendarId"></param>
        /// <returns></returns>
        Task<ExpandedResult> MoveEvent(Guid eventId, Guid? newParentCalendarId);
    }

}
