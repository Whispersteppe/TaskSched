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
using TaskSched.Common.FieldValidator;

namespace TaskSched.DataStore
{
    public class EventStore : IEventStore
    {
        IDataStoreMapper _mapper;
        IFieldValidatorSet _fieldValidatorSet;
        TaskSchedDbContextFactory _contextFactory;

        public EventStore(
            TaskSchedDbContextFactory contextFactory, 
            IDataStoreMapper mapper, 
            IFieldValidatorSet fieldValidators) 
        { 
            _contextFactory = contextFactory;
            _mapper = mapper;
            _fieldValidatorSet = fieldValidators;
        }

        private ExpandedResult ValidateEvent(Event eventItem) 
        { 

            ExpandedResult result = new ExpandedResult();

            if (eventItem.Activities != null)
            {
                foreach (var activity in eventItem.Activities)
                {
                    if (activity.Fields != null)
                    {
                        foreach (var field in activity.Fields)
                        {
                            if (_fieldValidatorSet.ValidateField(field.Value, field.FieldType) == false)
                            {
                                result.Messages.Add(new ResultMessage() { Severity = ResultMessageSeverity.Error, Message = $"Activity {activity.Name} Field {field.Name} has invalid data for type {field.FieldType} - {field.Value}" });
                            }
                        }
                    }
                }
            }

            return result;
        }

        public async Task<ExpandedResult<Guid>> Create(model.Event eventItem)
        {
            ExpandedResult<Guid> rslt = new ExpandedResult<Guid>();

            rslt.Messages.AddRange(ValidateEvent(eventItem).Messages);

            if (rslt.Status > ResultMessageSeverity.OK)
            {
                return rslt;
            }

            db.Event item = _mapper.Map<db.Event>(eventItem);

            item.LastExecution = DateTime.MinValue;
            item.NextExecution = DateTime.MinValue;
            item.Id = Guid.Empty;

            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                _dbContext.Events.Add(item);

                await _dbContext.SaveChangesAsync();


                rslt.Result = item.Id;


                rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.OK, Message = "Event created" });

                return rslt;
            }
        }

        public async Task<model.ExpandedResult> Delete(Guid eventId)
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
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
        }

        public async Task<model.ExpandedResult<model.Event?>> Get(Guid eventId)
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                var entity = await _dbContext
                .Events
                .Include(x => x.Schedules)
                .Include(x => x.Activities).ThenInclude(x=>x.Fields)
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
        }

        public async Task<model.ExpandedResult<List<model.Event>>> GetAll()
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                var entities = await _dbContext
                .Events
                .Include(x => x.Schedules)
                .Include(x => x.Activities).ThenInclude(x => x.Fields)
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
        }

        public async Task<model.ExpandedResult> Update(model.Event eventItem)
        {

            ExpandedResult rslt = new ExpandedResult();

            rslt.Messages.AddRange(ValidateEvent(eventItem).Messages);

            if (rslt.Status > ResultMessageSeverity.OK)
            {
                return rslt;
            }

            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                var dbEntity = await _dbContext
                .Events
                .Include(x => x.Schedules)
                .Include(x => x.Activities).ThenInclude(x=>x.Fields)
                .FirstOrDefaultAsync(x => x.Id == eventItem.Id)
                ;

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

        }

        public async Task<ExpandedResult> MoveEvent(Guid eventId, Guid? newParentCalendarId)
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
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
}
