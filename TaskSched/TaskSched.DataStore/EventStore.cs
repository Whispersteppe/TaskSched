using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using model=TaskSched.Common.DataModel;
using db = TaskSched.DataStore.DataModel;
using TaskSched.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskSched.Common.DataModel;

namespace TaskSched.DataStore
{
    public class EventStore : IEventStore
    {
        TaskSchedDbContext _dbContext;
        IDataStoreMapper _mapper;

        public EventStore(TaskSchedDbContext dbContext, IDataStoreMapper mapper) 
        { 
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<model.ExpandedResult<Guid>> Create(model.Event eventItem)
        {
             db.Event item = _mapper.Map<db.Event>(eventItem);

            item.LastExecution = DateTime.MinValue;
            item.NextExecution = DateTime.MinValue;
            item.Id = Guid.Empty;

            _dbContext.Events.Add(item);

            await _dbContext.SaveChangesAsync();

            model.ExpandedResult<Guid> rslt = new model.ExpandedResult<Guid>()
            {
                Result = item.Id
            };

            rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.OK, Message = "Event created" });

            return rslt;
        }

        public async Task<model.ExpandedResult> Delete(Guid eventId)
        {
            var entity = await _dbContext.Events.FirstOrDefaultAsync(x => x.Id == eventId);

            model.ExpandedResult rslt = new model.ExpandedResult();


            if (entity != null)
            {
                _dbContext.Events.Remove(entity);

                await _dbContext.SaveChangesAsync();
                rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.OK, Message = "Event deleted" });

            }
            else
            {
                rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.Warning, Message = "Event not found.  no deletion occurred" });
            }

            return rslt;
        }

        public async Task<model.ExpandedResult<model.Event?>> Get(Guid eventId)
        {
            var entity = await _dbContext
                .Events
                .Include(x=>x.Schedules)
                .Include(x=>x.Activities)
                .FirstOrDefaultAsync(x => x.Id == eventId)
                ;

            model.ExpandedResult<model.Event?> rslt = new model.ExpandedResult<model.Event>();

            if (entity != null)
            {

                rslt.Result = _mapper.Map<model.Event>(entity);
                rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.OK, Message = "Event retrieved" });

            }
            else
            {
                rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.Warning, Message = "Event not found." });
            }

            return rslt;
        }

        public async Task<model.ExpandedResult<List<model.Event>>> GetAll()
        {
            var entities = await _dbContext
                .Events
                .Include(x => x.Schedules)
                .Include(x => x.Activities)
                .ToListAsync();
                ;

            model.ExpandedResult<List<model.Event>> rslt = new model.ExpandedResult<List<model.Event>>();

            if (entities != null)
            {

                rslt.Result = _mapper.Map<List<model.Event>>(entities);
                rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.OK, Message = "Events retrieved" });

            }
            else
            {
                rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.Warning, Message = "Events not found." });
            }

            return rslt;
        }

        public async Task<model.ExpandedResult> Update(model.Event eventItem)
        {
            var dbEntity = await _dbContext
                .Events
                .Include(x => x.Schedules)
                .Include(x => x.Activities)
                .FirstOrDefaultAsync(x => x.Id == eventItem.Id)
                ;

            model.ExpandedResult rslt = new model.ExpandedResult();


            if (dbEntity != null)
            {
                _dbContext.Entry(dbEntity).CurrentValues.SetValues(eventItem);

                _dbContext.Update(dbEntity);

                await _dbContext.SaveChangesAsync();

                rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.OK, Message = "Event updated" });

            }
            else
            {
                rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.Error, Message = "Event not found.  no update occurred" });
            }

            return rslt;

        }

        public async Task<ExpandedResult> MoveEvent(Guid eventId, Guid? newParentCalendarId)
        {
            var eventItem = _dbContext.Events.FirstOrDefault(x => x.Id == eventId);

            if (eventItem != null)
            {
                eventItem.CalendarId = newParentCalendarId;
                _dbContext.Events.Update(eventItem);
                await _dbContext.SaveChangesAsync();

                return new ExpandedResult() { Messages = new List<ResultMessage>() { new ResultMessage() { Message = "Event moved", Severity = ResultMessageSeverity.OK } } };

            }
            else
            {
                return new ExpandedResult() { Messages = new List<ResultMessage>() { new ResultMessage() { Message = "Event was not found", Severity = ResultMessageSeverity.Error } } };
            }


        }


    }
}
