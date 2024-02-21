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
    public class ActivityStore : IActivityStore
    {
        TaskSchedDbContextFactory _contextFactory;
        IDataStoreMapper _mapper;
        IFieldValidatorSet _fieldValidatorSet;

        public ActivityStore(TaskSchedDbContextFactory contextFactory, IDataStoreMapper mapper, IFieldValidatorSet fieldValidatorSet) 
        { 
            _contextFactory = contextFactory;
            _mapper = mapper;
            _fieldValidatorSet = fieldValidatorSet;
        }


        private ExpandedResult ValidateActivity(model.Activity activity)
        {
            ExpandedResult result = new ExpandedResult();

            if (activity.DefaultFields != null)
            {
                foreach (var field in activity.DefaultFields)
                {
                    if (_fieldValidatorSet.ValidateField(field.Value, field.FieldType) == false)
                    {
                        result.Messages.Add(new ResultMessage() { Severity = ResultMessageSeverity.Error, Message = $"Activity {activity.Name} Field {field.Name} has invalid data for type {field.FieldType} - {field.Value}" });
                    }

                }
            }


            return result;
        }


        public async Task<model.ExpandedResult<Guid>> Create(model.Activity activity)
        {

            ExpandedResult<Guid> rslt = new ExpandedResult<Guid>();

            rslt.Messages.AddRange(ValidateActivity(activity).Messages);

            if (rslt.Status > ResultMessageSeverity.OK)
            {
                return rslt;
            }


            db.Activity item = _mapper.Map<db.Activity>(activity);

            item.Id = Guid.Empty;
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                _dbContext.Activities.Add(item);

                await _dbContext.SaveChangesAsync();

                rslt.Result = item.Id;

                rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.OK, Message = "Activity created" });

                return rslt;
            }
        }

        public async Task<model.ExpandedResult> Delete(Guid activityId)
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                var entity = await _dbContext.Activities.FirstOrDefaultAsync(x => x.Id == activityId);

                model.ExpandedResult rslt = new model.ExpandedResult();


                if (entity != null)
                {
                    _dbContext.Activities.Remove(entity);

                    await _dbContext.SaveChangesAsync();
                    rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.OK, Message = "Activity deleted" });

                }
                else
                {
                    rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.Warning, Message = "Activity not found.  no deletion occurred" });
                }

                return rslt;
            }
        }

        public async Task<model.ExpandedResult<model.Activity?>> Get(Guid eventId)
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                var entity = await _dbContext
                .Activities
                .Include(x => x.DefaultFields)
                .FirstOrDefaultAsync(x => x.Id == eventId)
                ;

                model.ExpandedResult<model.Activity?> rslt = new model.ExpandedResult<model.Activity>();

                if (entity != null)
                {

                    rslt.Result = _mapper.Map<model.Activity>(entity);
                    rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.OK, Message = "Activity retrieved" });

                }
                else
                {
                    rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.Warning, Message = "Activity not found." });
                }

                return rslt;
            }
        }

        public async Task<model.ExpandedResult<List<model.Activity>>> GetAll()
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                var entities = await _dbContext
                .Activities
                .Include(x => x.DefaultFields)
                .ToListAsync();
                ;

                model.ExpandedResult<List<model.Activity>> rslt = new model.ExpandedResult<List<model.Activity>>();

                if (entities != null)
                {

                    rslt.Result = _mapper.Map<List<model.Activity>>(entities);
                    rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.OK, Message = "Activities retrieved" });

                }
                else
                {
                    rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.Warning, Message = "Activities not found." });
                }

                return rslt;
            }
        }

        public async Task<model.ExpandedResult> Update(model.Activity activity)
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                ExpandedResult rslt = new ExpandedResult();

                rslt.Messages.AddRange(ValidateActivity(activity).Messages);

                if (rslt.Status > ResultMessageSeverity.OK)
                {
                    return rslt;
                }


                var dbActivity = await _dbContext
                    .Activities
                    .Include(x => x.DefaultFields)
                    .FirstOrDefaultAsync(x => x.Id == activity.Id)
                    ;

                if (dbActivity != null)
                {
                    _dbContext.Entry(dbActivity).CurrentValues.SetValues(activity);

                    _dbContext.Update(dbActivity);

                    await _dbContext.SaveChangesAsync();

                    rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.OK, Message = "Activity updated" });

                }
                else
                {
                    rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.Error, Message = "Activity not found.  no update occurred" });
                }

                return rslt;
            }
        }
    }
}
