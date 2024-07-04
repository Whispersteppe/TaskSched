using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model=TaskSched.Common.DataModel;
using DB = TaskSched.DataStore.DataModel;
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
        ILogger _logger;

        public ActivityStore(TaskSchedDbContextFactory contextFactory, IDataStoreMapper mapper, IFieldValidatorSet fieldValidatorSet, ILogger<ActivityStore> logger) 
        { 
            _contextFactory = contextFactory;
            _mapper = mapper;
            _fieldValidatorSet = fieldValidatorSet;
            _logger = logger;
        }


        private ExpandedResult ValidateActivity(Model.Activity activity)
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


        public async Task<Model.ExpandedResult<Guid>> Create(Model.Activity activity)
        {

            ExpandedResult<Guid> rslt = new ExpandedResult<Guid>();

            rslt.Messages.AddRange(ValidateActivity(activity).Messages);

            if (rslt.Status > ResultMessageSeverity.OK)
            {
                return rslt;
            }


            DB.Activity item = _mapper.Map<DB.Activity>(activity);

            item.Id = Guid.Empty;
            foreach(var field in item.DefaultFields)
            {
                field.ActivityId = Guid.Empty;
                field.Id = Guid.Empty;
            }

            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                _dbContext.Activities.Add(item);

                await _dbContext.SaveChangesAsync();

                rslt.Result = item.Id;

                rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.OK, Message = "Activity created" });

                _logger.LogInformation($"Creating Activity {activity.Name}, ID={item.Id}");

                return rslt;
            }
        }

        public async Task<Model.ExpandedResult> Delete(Guid activityId)
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                var entity = await _dbContext.Activities.FirstOrDefaultAsync(x => x.Id == activityId);

                Model.ExpandedResult rslt = new Model.ExpandedResult();


                if (entity != null)
                {
                    _dbContext.Activities.Remove(entity);

                    await _dbContext.SaveChangesAsync();
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.OK, Message = "Activity deleted" });

                    _logger.LogInformation($"Deleting Activity {entity.Name}, ID={activityId}");

                }
                else
                {
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.Warning, Message = "Activity not found.  no deletion occurred" });
                    _logger.LogWarning($"Activity not found - ID={activityId}");
                }

                return rslt;
            }
        }

        public async Task<Model.ExpandedResult<Model.Activity?>> Get(Guid eventId)
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                var entity = await _dbContext
                .Activities
                .Include(x => x.DefaultFields)
                .FirstOrDefaultAsync(x => x.Id == eventId)
                ;

                Model.ExpandedResult<Model.Activity?> rslt = new Model.ExpandedResult<Model.Activity?>();

                if (entity != null)
                {

                    rslt.Result = _mapper.Map<Model.Activity>(entity);
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.OK, Message = "Activity retrieved" });

                }
                else
                {
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.Warning, Message = "Activity not found." });
                    _logger.LogWarning($"Activity not found - ID={eventId}");
                }

                return rslt;
            }
        }

        public async Task<ExpandedResult<Activity>> GetDefault()
        {

            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {
                var entities = await _dbContext
                .Activities
                .Include(x => x.DefaultFields)
                .Include(x=>x.TaskActions)
                .ToListAsync();
                ;

                //  now lets get the one with the most tasks associated with it

                var selectedEntity = entities.FirstOrDefault();
                foreach(var entity in entities)
                {
                    if (entity.TaskActions.Count > selectedEntity.TaskActions.Count)
                    {
                        selectedEntity = entity;
                    }
                }


                Model.ExpandedResult<Model.Activity> rslt = new Model.ExpandedResult<Model.Activity>()
                {
                    Result = _mapper.Map<Model.Activity>(selectedEntity), 
                    Messages= new List<Model.ResultMessage>()
                };

                return rslt;

            }

        }

        public async Task<Model.ExpandedResult<List<Model.Activity>>> GetAll()
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                var entities = await _dbContext
                .Activities
                .Include(x => x.DefaultFields)
                .ToListAsync();
                ;

                Model.ExpandedResult<List<Model.Activity>> rslt = new Model.ExpandedResult<List<Model.Activity>>();

                if (entities != null)
                {

                    rslt.Result = _mapper.Map<List<Model.Activity>>(entities);
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.OK, Message = "Activities retrieved" });

                }
                else
                {
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.Warning, Message = "Activities not found." });
                    _logger.LogWarning($"Activities not found");
                }

                return rslt;
            }
        }

        public async Task<Model.ExpandedResult> Update(Model.Activity activity)
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

                    //  do the fields
                    foreach (var field in activity.DefaultFields)
                    {
                        if (field.Id == Guid.Empty)
                        {
                            var dbField = new DB.ActivityField()
                            {
                                Name = field.Name,
                                Value = field.Value, 
                                FieldType = field.FieldType, 
                                IsReadOnly = field.IsReadOnly,
                            };
                            dbActivity.DefaultFields.Add(dbField);
                        }
                        else
                        {
                            var dbField = dbActivity.DefaultFields.FirstOrDefault(x => x.Id == field.Id);
                            _dbContext.Entry(dbField).CurrentValues.SetValues(field);
                        }
                    }

                    List<DB.ActivityField> deletedFields = dbActivity.DefaultFields.Where(x => activity.DefaultFields.Any(y => y.Id == x.Id) == false).ToList();
                    deletedFields.ForEach(x => dbActivity.DefaultFields.Remove(x));

                    _dbContext.Update(dbActivity);

                    await _dbContext.SaveChangesAsync();

                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.OK, Message = "Activity updated" });

                    _logger.LogInformation($"Activity updated {activity.Name} - ID={activity.Id}");

                }
                else
                {
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.Error, Message = "Activity not found.  no update occurred" });
                    _logger.LogWarning($"Activity not found {activity.Name} - ID={activity.Id}");
                }

                return rslt;
            }
        }
    }
}
