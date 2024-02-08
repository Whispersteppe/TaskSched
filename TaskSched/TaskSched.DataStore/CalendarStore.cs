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
        TaskSchedDbContext _dbContext;
        IDataStoreMapper _mapper;

        public CalendarStore(TaskSchedDbContext dbContext, IDataStoreMapper mapper) 
        { 
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Model.ExpandedResult<Guid>> Create(Model.Calendar calendar)
        {
             Db.Calendar item = _mapper.Map<Db.Calendar>(calendar);

            item.Id = Guid.Empty;

            _dbContext.Calendars.Add(item);

            await _dbContext.SaveChangesAsync();

            Model.ExpandedResult<Guid> rslt = new Model.ExpandedResult<Guid>()
            {
                Result = item.Id
            };

            rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.OK, Message = "Calendar created" });

            return rslt;
        }



        public async Task<Model.ExpandedResult> Delete(Guid calendarId)
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

        public async Task<Model.ExpandedResult<Model.Calendar>> Get(Guid calendarId)
        {
            var entity = await _dbContext
                .Calendars
                .FirstOrDefaultAsync(x => x.Id == calendarId)
                ;

            Model.ExpandedResult<Model.Calendar?> rslt = new Model.ExpandedResult<Model.Calendar>();

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


        public async Task<Model.ExpandedResult<List<Model.Calendar>>> GetAll(CalendarRetrievalParameters parameters)
        {
            var query = _dbContext
                .Calendars.AsQueryable()
                ;

            if (parameters.AddChildEvents == true)
            {
                query = query
                    .Include(x => x.Events)
                    ;
            }

            var calendars = await query.ToListAsync();


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
                            parentCalendar.ChildCalendars.Add(calendar);
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

        public async Task<Model.ExpandedResult> Update(Model.Calendar calendar)
        {
            var dbEntity = await _dbContext
                .Calendars
                .FirstOrDefaultAsync(x => x.Id == calendar.Id)
                ;

            Model.ExpandedResult rslt = new Model.ExpandedResult();


            if (dbEntity != null)
            {
                _dbContext.Entry(dbEntity).CurrentValues.SetValues(calendar);

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


        public async Task<ExpandedResult> MoveCalendar(Guid calendarId, Guid? newParentCalendarId)
        {
            var calendar = _dbContext.Calendars.FirstOrDefault(x=>x.Id == calendarId);

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
