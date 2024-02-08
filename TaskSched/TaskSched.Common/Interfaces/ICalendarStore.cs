using System.Dynamic;
using System.Globalization;
using TaskSched.Common.DataModel;
using Calendar = TaskSched.Common.DataModel.Calendar;

namespace TaskSched.Common.Interfaces
{
    /// <summary>
    /// repository interface for calendars
    /// </summary>
    public interface ICalendarStore
    {
        /// <summary>
        /// Get a single calendar
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        Task<ExpandedResult<Calendar?>> Get(Guid calendarId);

        /// <summary>
        /// Get all calendars using the parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ExpandedResult<List<Calendar>>> GetAll(CalendarRetrievalParameters parameters);

        /// <summary>
        /// update a single calendar
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns></returns>
        Task<ExpandedResult> Update(Calendar calendar);

        /// <summary>
        /// remove a calendar
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        Task<ExpandedResult> Delete(Guid calendarId);

        /// <summary>
        /// create a calendar
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns></returns>
        Task<ExpandedResult<Guid>> Create(Calendar calendar);

        /// <summary>
        /// move a calendar to a new parent
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="newParentCalendarId"></param>
        /// <returns></returns>
        Task<ExpandedResult> MoveCalendar(Guid calendarId, Guid? newParentCalendarId);

    }
}
