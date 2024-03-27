using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model=TaskSched.Common.DataModel;
using Db = TaskSched.DataStore.DataModel;
using TaskSched.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskSched.Common.DataModel;

namespace TaskSched.DataStore
{
    public class CalendarStore : ICalendarStore
    {
        TaskSchedDbContextFactory _contextFactory;
        IDataStoreMapper _mapper;

        public CalendarStore(TaskSchedDbContextFactory contextFactory, IDataStoreMapper mapper) 
        { 
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<Model.ExpandedResult<Guid>> Create(Model.Calendar calendar)
        {
            Db.Calendar item = _mapper.Map<Db.Calendar>(calendar);

            item.Id = Guid.Empty;

            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {
                _dbContext.Calendars.Add(item);

                await _dbContext.SaveChangesAsync();

                Model.ExpandedResult<Guid> rslt = new Model.ExpandedResult<Guid>()
                {
                    Result = item.Id
                };

                rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.OK, Message = "Calendar created" });

                return rslt;
            }
        }



        public async Task<Model.ExpandedResult> Delete(Guid calendarId)
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                var entity = await _dbContext.Calendars.FirstOrDefaultAsync(x => x.Id == calendarId);

                Model.ExpandedResult rslt = new Model.ExpandedResult();


                if (entity != null)
                {
                    _dbContext.Calendars.Remove(entity);

                    await _dbContext.SaveChangesAsync();
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.OK, Message = "Calendar deleted" });

                }
                else
                {
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.Warning, Message = "Calendar not found.  no deletion occurred" });
                }

                return rslt;
            }
        }

        public async Task<Model.ExpandedResult<Model.Calendar?>> Get(Guid calendarId)
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                var entity = await _dbContext
                .Calendars
                .FirstOrDefaultAsync(x => x.Id == calendarId)
                ;

                Model.ExpandedResult<Model.Calendar?> rslt = new Model.ExpandedResult<Model.Calendar?>();

                if (entity != null)
                {

                    rslt.Result = _mapper.Map<Model.Calendar>(entity);  
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.OK, Message = "Calendar retrieved" });

                }
                else
                {
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.Warning, Message = "Calendar not found." });
                }

                return rslt;
            }
        }


        public async Task<Model.ExpandedResult<List<Model.Calendar>>> GetAll(CalendarRetrievalParameters parameters)
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                var query = _dbContext
                .Calendars.AsQueryable()
                ;

                //  this seems to get all the calendars already built into a tree.  cool
                var calendars = await query.ToListAsync();


                if (parameters.AddChildEvents == true)
                {
                    //  add the unassigned calendar
                    calendars.Add(new Db.Calendar()
                    {
                        Id = Guid.Empty,
                        Name = "Unassigned Events",
                        ParentCalendarId = null,
                        Events = new List<Db.Event>(),
                    });

                    //  this drops all the events into the calendars directly.  also cool.
                    var events = await _dbContext
                        .Events
                        .Include(x => x.Schedules)
                        .Include(x => x.Activities).ThenInclude(x=>x.Fields)
                        .ToListAsync();
                    ;


                    foreach (var eventItem in events)
                    {
                        Db.Calendar? associatedCalendar;
                        if (eventItem.CalendarId != null)
                        {
                            associatedCalendar = calendars.FirstOrDefault(x => x.Id == eventItem.CalendarId);
                            if (associatedCalendar == null)
                            {
                                associatedCalendar = calendars.FirstOrDefault(x => x.Id == Guid.Empty);
                            }
                        }
                        else
                        {
                            associatedCalendar = calendars.FirstOrDefault(x => x.Id == Guid.Empty);
                        }

                        if (associatedCalendar != null)
                        {
                            //  lets not add them again.  it gets clumsy
                            if (associatedCalendar.Events.Any(x=>x.Id == eventItem.Id) == false)
                            {
                                associatedCalendar.Events.Add(eventItem);
                            }
                        }
                    }
                }

                if (parameters.AsTree == true)
                {
                    //  build out the tree

                    var calendarTree = new List<Db.Calendar>();

                    foreach (var calendar in calendars)
                    {
                        if (calendar.ParentCalendarId == null)
                        {
                            calendarTree.Add(calendar);
                        }
                        else
                        {
                            var parentCalendar = calendars.FirstOrDefault(x => x.Id == calendar.ParentCalendarId);
                            if (parentCalendar != null)
                            {
                                if (parentCalendar.ChildCalendars == null)
                                {
                                    parentCalendar.ChildCalendars = new List<Db.Calendar>();
                                }
                                //  again, don't duplicate things
                                if (parentCalendar.ChildCalendars.Any(x => x.Id == calendar.Id) == false)
                                {
                                    parentCalendar.ChildCalendars.Add(calendar);
                                }
                            }
                        }
                    }

                    calendars = calendarTree;
                }


                Model.ExpandedResult<List<Model.Calendar>> rslt = new Model.ExpandedResult<List<Model.Calendar>>();

                if (calendars != null)
                {

                    rslt.Result = _mapper.Map<List<Model.Calendar>>(calendars);
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.OK, Message = "Calendars retrieved" });

                }
                else
                {
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.Warning, Message = "Calendars not found." });
                }

                return rslt;
            }
        }

        public async Task<Model.ExpandedResult> Update(Model.Calendar calendar)
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                var dbEntity = await _dbContext
                .Calendars
                .FirstOrDefaultAsync(x => x.Id == calendar.Id)
                ;

                Model.ExpandedResult rslt = new Model.ExpandedResult();


                if (dbEntity != null)
                {

                    //  we're not doing a mapping since we've only got one field, and i don't want to screw up child events and calendars
                    //_dbContext.Entry(dbEntity).CurrentValues.SetValues(calendar); 
                    dbEntity.Name = calendar.Name;

                    _dbContext.Update(dbEntity);

                    await _dbContext.SaveChangesAsync();

                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.OK, Message = "Calendar updated" });

                }
                else
                {
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.Error, Message = "Calendar not found.  no update occurred" });
                }

                return rslt;
            }

        }


        public async Task<ExpandedResult> MoveCalendar(Guid calendarId, Guid? newParentCalendarId)
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                var calendar = _dbContext.Calendars.FirstOrDefault(x => x.Id == calendarId);

                if (calendar != null)
                {
                    calendar.ParentCalendarId = newParentCalendarId;
                    _dbContext.Calendars.Update(calendar);
                    await _dbContext.SaveChangesAsync();

                    return new ExpandedResult() { Messages = new List<Model.ResultMessage>() { new ResultMessage() { Message = "Calendar moved", Severity = ResultMessageSeverity.OK } } };

                }
                else
                {
                    return new ExpandedResult() { Messages = new List<Model.ResultMessage>() { new ResultMessage() { Message = "Calendar was not found", Severity = ResultMessageSeverity.Error } } };
                }
            }

        }





    }
}
