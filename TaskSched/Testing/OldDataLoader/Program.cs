using OldDataLoader.OldDataModel;
using System.Diagnostics;
using TaskSched.Common.DataModel;
using TaskSched.Common.Interfaces;
using TaskSched.DataStore;
using Activity = TaskSched.Common.DataModel.Activity;

namespace OldDataLoader
{

    /// <summary>
    /// run time for the loader
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// application entry point
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            Console.WriteLine("start load");

            //  lets get the old config.  
            TaskRunnerConfig config = TaskRunnerConfig.GetConfig("TaskRunner.json");

            //  set up and create the database.  we'll wipe whatever is already there.
            TaskSchedDbContextConfiguration dbConfig = new TaskSchedDbContextConfiguration()
            {
                DataSource = "TaskSched.sqlite"
            };

            DataStoreMapper mapper = new DataStoreMapper();

            TaskSchedDbContext dbContext = new TaskSchedDbContext(dbConfig);
            if (File.Exists(dbConfig.DataSource))
            {
                await dbContext.Database.EnsureDeletedAsync();
            }
            await dbContext.Database.EnsureCreatedAsync();

            //  create my event stores
            IEventStore eventStore = new EventStore(dbContext, mapper);
            IActivityStore activityStore = new ActivityStore(dbContext, mapper);
            ICalendarStore calendarStore = new CalendarStore(dbContext, mapper);

            List<Activity> activities = await LoadActivities(config, activityStore);

            Debug.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(activities, Newtonsoft.Json.Formatting.Indented));

            List<Calendar> calendars = await LoadEvents(config, calendarStore, eventStore);
            Debug.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(calendars, Newtonsoft.Json.Formatting.Indented));



        }

        private static async Task<List<Calendar>> LoadEvents(TaskRunnerConfig config, ICalendarStore calendarStore, IEventStore eventStore)
        { 
            await LoadBaseTaskList(config, calendarStore, eventStore, config.Tasks);

            var rslt = await calendarStore.GetAll(new CalendarRetrievalParameters() { AddChildEvents = true, AsTree = true, AddChildFolders = true });

            return rslt.Result;
        }

        private static async Task LoadBaseTaskList(TaskRunnerConfig config, ICalendarStore calendarStore, IEventStore eventStore, List<TaskBaseConfig>? taskList, Guid? parentCalendarId = null)
        {
            if (taskList == null || taskList.Count == 0) return;

            foreach (var item in taskList)
            {
                if (item is TaskFolderConfig taskFolder)
                {
                    Calendar calendar = new Calendar()
                    {
                        Name = taskFolder.Name,
                        ParentCalendarId = parentCalendarId,
                    };

                    var rsltCreateCalendar = await calendarStore.Create(calendar);

                    //  now to check the children

                    await LoadBaseTaskList(config, calendarStore, eventStore, taskFolder.ChildItems, rsltCreateCalendar.Result);

                }
                else if (item is TaskConfig taskItem)
                {
                    Event eventItem = new Event()
                    {
                        Name = taskItem.Name,
                        IsActive = taskItem.IsActive,
                        CalendarId = parentCalendarId,
                        CatchUpOnStartup = taskItem.AllowLaunchOnStartup,
                        LastExecution = taskItem.LastExecution,
                        NextExecution = DateTime.Now,
                        Activities = new List<EventActivity>(),
                        Schedules = new List<EventSchedule>()
                    };
                    if (taskItem.Trigger is TriggerCronConfig triggerCronConfig)
                    {
                        EventSchedule schedule = new EventSchedule()
                        {
                            CRONData = triggerCronConfig.CronExpression,
                            Name = triggerCronConfig.CronExpression
                        };
                        eventItem.Schedules.Add(schedule);
                    }

                    var template = config.Templates.FirstOrDefault(x => x.ID == taskItem.TemplateID);

                    EventActivity activity = new EventActivity()
                    {
                        Name = taskItem.Name,
                        ActivityId = template.NewId,
                        Fields = new List<EventActivityField>()
                    };

                    eventItem.Activities.Add(activity);

                    foreach (var field in item.Properties)
                    {
                        //  find the matching template field
                        var templateField = template.Properties.FirstOrDefault(x => x.Name == field.Key);

                        var eventActivityField = new EventActivityField()
                        {
                            Name = field.Key,
                            Value = field.Value.ToString(),
                            ActivityFieldId = templateField.NewId
                        };

                        activity.Fields.Add(eventActivityField);

                    }

                    await eventStore.Create(eventItem);

                }
            }
        }


        private async static Task<List<Activity>> LoadActivities(TaskRunnerConfig config, IActivityStore activityStore)
        {
            foreach(var template in config.Templates)
            {
                Activity activity = new Activity()

                {
                    Name = template.Name,
                    ActivityType = ActivityTypeEnum.ExternalProgram,
                    DefaultFields = new List<ActivityField>()
                };

                foreach(var field in template.Properties)
                {
                    var defaultField = new ActivityField()
                    {
                        Name = field.Name,
                        IsReadOnly = false,
                        Value = field.DefaultValue
                        //  property type
                    };

                    activity.DefaultFields.Add(defaultField);
                }

                if (template is FileExecuteTemplateConfig executeTemplate)
                {
                    activity.DefaultFields.Add(new ActivityField()
                    {
                        Name = "ExecutablePath",
                        IsReadOnly = true,
                        Value = executeTemplate.ExecutablePath
                    });
                    
                    activity.DefaultFields.Add(new ActivityField()
                    {
                        Name = "CommandLine",
                        IsReadOnly = true,
                        Value = executeTemplate.CommandLine
                    });

                }
                else if (template is RSSWatcherTemplateConfig rssWatcherTemplate)
                {
                    // we're tossing these
                    //activity.DefaultFields.Add(new ActivityField()
                    //{
                    //    Name = "ExecutablePath",
                    //    IsReadOnly = true,
                    //    Value = rssWatcherTemplate.ExecutablePath
                    //});

                    //activity.DefaultFields.Add(new ActivityField()
                    //{
                    //    Name = "CommandLine",
                    //    IsReadOnly = true,
                    //    Value = rssWatcherTemplate.CommandLine
                    //});

                }

                var rsltCreate = await activityStore.Create(activity);
                template.NewId = rsltCreate.Result;

                var rsltGet = await activityStore.Get(rsltCreate.Result);
                foreach(var field in rsltGet.Result.DefaultFields)
                {
                    //  find the matching field
                    PropertyTemplate? foundProperty = template.Properties.FirstOrDefault(x=>x.Name == field.Name);
                    if (foundProperty != null)
                    {
                        foundProperty.NewId = field.Id;
                    }
                }

            }

            //  got them all loaded, now get them back for later use

            var rslt = await activityStore.GetAll();

            return rslt.Result;
        }
    }
}
