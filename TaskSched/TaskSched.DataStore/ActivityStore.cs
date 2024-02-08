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

namespace TaskSched.DataStore
{
    public class ActivityStore : IActivityStore
    {
        TaskSchedDbContext _dbContext;
        IDataStoreMapper _mapper;

        public ActivityStore(TaskSchedDbContext dbContext, IDataStoreMapper mapper) 
        { 
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<model.ExpandedResult<Guid>> Create(model.Activity activity)
        {
             db.Activity item = _mapper.Map<db.Activity>(activity);

            item.Id = Guid.Empty;

            _dbContext.Activities.Add(item);

            await _dbContext.SaveChangesAsync();

            model.ExpandedResult<Guid> rslt = new model.ExpandedResult<Guid>()
            {
                Result = item.Id
            };

            rslt.Messages.Add(new model.ResultMessage() { Severity = model.ResultMessageSeverity.OK, Message = "Activity created" });

            return rslt;
        }

        public async Task<model.ExpandedResult> Delete(Guid activityId)
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

        public async Task<model.ExpandedResult<model.Activity?>> Get(Guid eventId)
        {
            var entity = await _dbContext
                .Activities
                .Include(x=>x.DefaultFields)
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

        public async Task<model.ExpandedResult<List<model.Activity>>> GetAll()
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

        public async Task<model.ExpandedResult> Update(model.Activity activity)
        {
            var dbActivity = await _dbContext
                .Activities
                .Include(x => x.DefaultFields)
                .FirstOrDefaultAsync(x => x.Id == activity.Id)
                ;

            model.ExpandedResult rslt = new model.ExpandedResult();


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
