using OldDataLoader.OldDataModel;
using System.Diagnostics;
using TaskSched.Common.DataModel;
using TaskSched.Common.FieldValidator;
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
                DataSource = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "TaskSched.sqlite")
            };

            Debug.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(dbConfig, Newtonsoft.Json.Formatting.Indented));


            DataStoreMapper mapper = new DataStoreMapper();
            TaskSchedDbContextFactory contextFactory = new TaskSchedDbContextFactory(dbConfig);
            TaskSchedDbContext dbContext = contextFactory.GetConnection();

            if (File.Exists(dbConfig.DataSource))
            {
                await dbContext.Database.EnsureDeletedAsync();
            }
            await dbContext.Database.EnsureCreatedAsync();

            //  create my event stores
            IFieldValidatorSet fieldValidatorSet = new FieldValidatorSet();
            IEventStore eventStore = new EventStore(contextFactory, mapper, fieldValidatorSet);
            IActivityStore activityStore = new ActivityStore(contextFactory, mapper, fieldValidatorSet);
            IFolderStore folderStore = new FolderStore(contextFactory, mapper);

            List<Activity> activities = await LoadActivities(config, activityStore);

            Debug.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(activities, Newtonsoft.Json.Formatting.Indented));

            List<Folder> folders = await LoadEvents(config, folderStore, eventStore);
            Debug.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(folders, Newtonsoft.Json.Formatting.Indented));



        }

        private static async Task<List<Folder>> LoadEvents(TaskRunnerConfig config, IFolderStore folderStore, IEventStore eventStore)
        { 
            await LoadBaseTaskList(config, folderStore, eventStore, config.Tasks);

            var rslt = await folderStore.GetAll(new FolderRetrievalParameters() { AddChildEvents = true, AsTree = true, AddChildFolders = true });

            return rslt.Result;
        }

        private static async Task LoadBaseTaskList(TaskRunnerConfig config, 
            IFolderStore folderStore, 
            IEventStore eventStore, 
            List<TaskBaseConfig>? taskList, 
            Guid? parentFolderId = null)
        {
            if (taskList == null || taskList.Count == 0) return;

            foreach (var item in taskList)
            {
                if (item is TaskFolderConfig taskFolder)
                {
                    Folder folder = new Folder()
                    {
                        Name = taskFolder.Name,
                        ParentFolderId = parentFolderId,
                    };

                    var rsltCreateFolder = await folderStore.Create(folder);

                    //  now to check the children

                    await LoadBaseTaskList(config, folderStore, eventStore, taskFolder.ChildItems, rsltCreateFolder.Result);

                }
                else if (item is TaskConfig taskItem)
                {
                    Event eventItem = new Event()
                    {
                        Name = taskItem.Name,
                        IsActive = taskItem.IsActive,
                        FolderId = parentFolderId,
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
                    if (template != null && template.NewId != Guid.Empty)
                    {

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
                                ActivityFieldId = templateField.NewId,
                                FieldType = field.Key == "Url" ? FieldTypeEnum.Url : FieldTypeEnum.String
                            };

                            activity.Fields.Add(eventActivityField);

                        }

                        var rslt = await eventStore.Create(eventItem);
                        var rsltGet = await eventStore.Get(rslt.Result);
                    }
                    else
                    {
                        Console.WriteLine($"can't get activity - {eventItem.Name}");
                    }


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
                    ActivityHandlerId = new Guid("00000000-0000-0000-0000-000000000001"), //  hard-wired from 
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
                        Value = executeTemplate.ExecutablePath, 
                        FieldType= FieldTypeEnum.ExecutablePath,
                    });
                    
                    activity.DefaultFields.Add(new ActivityField()
                    {
                        Name = "CommandLine",
                        IsReadOnly = true,
                        Value = executeTemplate.CommandLine.Replace("{0}", "{Url}")
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
                if (rsltCreate.Status == ResultMessageSeverity.OK)
                {
                    template.NewId = rsltCreate.Result;

                    var rsltGet = await activityStore.Get(rsltCreate.Result);
                    foreach (var field in rsltGet.Result.DefaultFields)
                    {
                        //  find the matching field
                        PropertyTemplate? foundProperty = template.Properties.FirstOrDefault(x => x.Name == field.Name);
                        if (foundProperty != null)
                        {
                            foundProperty.NewId = field.Id;
                        }
                    }
                }
                else
                {
                    foreach(var msg in rsltCreate.Messages)
                    {
                        Console.WriteLine(msg.Message);
                    }
                }

            }

            //  got them all loaded, now get them back for later use

            var rslt = await activityStore.GetAll();

            return rslt.Result;
        }
    }
}
