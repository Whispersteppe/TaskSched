using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.FieldValidator;
using TaskSched.Common.Interfaces;
using TaskSched.DataStore;
using TaskSched.ExecutionEngine;
using TaskSched.Test.DummyComponents;
using TaskSched.Test.XUnit;
using Xunit.Abstractions;

namespace TaskSched.Test
{
    public class ScheduleEngineTests : XUnitTestClassBase
    {
        public ScheduleEngineTests(XUnitClassFixture fixture, XUnitCollectionFixture collectionFixture, ITestOutputHelper output) : base(fixture, collectionFixture, output)
        {
        }

        [Fact]
        public async Task ScheduleEngineTest()
        {
            await Task.Run(() => { });


            ILogger logger = this.GetLogger(memberName: nameof(ExecutionEngineTests));
            IExecutionStore executionStore = new ExecutionStore.ExecutionStore(logger);
            IExecutionEngine executionEngine = new ActivityEngine(logger, executionStore);

            //var db = this.CollectionFixture.Repository;
            var factory = CollectionFixture.RepositoryFactory;

            IFieldValidatorSet fieldValidatorSet = new FieldValidatorSet();
            
            IDataStoreMapper mapper = new TaskSched.DataStore.DataStoreMapper();
            IEventStore eventStore = new TaskSched.DataStore.EventStore(factory, mapper, fieldValidatorSet, this.GetLogger<EventStore>());
            IActivityStore activityStore = new TaskSched.DataStore.ActivityStore(factory, mapper, fieldValidatorSet, this.GetLogger<ActivityStore>());


            ISchedulerEngine scheduleEngine = new SchedulerEngine.SchedulerEngine(executionEngine, eventStore, activityStore, logger);

        }

        [Fact]
        public async Task RunEngine()
        {
            // builder.Services.
            IEventStore eventStore = new InMemoryEventStore();
            IActivityStore activityStore = new InMemoryActivityStore();
            ILogger logger = new DebugLogger();
            IExecutionStore executionStore = new TaskSched.ExecutionStore.ExecutionStore(logger);

            var activity = new Common.DataModel.Activity()
            {
                ActivityHandlerId = new Guid("00000000-0000-0000-0000-000000000001"),
                Name = "testName",
                DefaultFields = new List<Common.DataModel.ActivityField>()
                {
                     new Common.DataModel.ActivityField()
                     {
                         Name = "ExecutablePath",
                         Value= @"C:\Program Files\Mozilla Firefox\firefox.exe",
                     },
                     new Common.DataModel.ActivityField()
                     {
                         Name = "CommandLine",
                         Value= "-new-tab {Url}",
                     }

                }

            };

            var activityCreate = await activityStore.Create(activity);

            IExecutionEngine executionEngine = new ActivityEngine(logger, executionStore);

            ISchedulerEngine schedulerEngine = new TaskSched.SchedulerEngine.SchedulerEngine(executionEngine, eventStore, activityStore, logger);

            await schedulerEngine.Start();
            await executionEngine.Start();

            await schedulerEngine.CreateEvent(new Common.DataModel.Event()
            {
                CatchUpOnStartup = true,
                IsActive = true,
                LastExecution = DateTime.Now,
                Name = Guid.NewGuid().ToString(),
                Schedules = new List<Common.DataModel.EventSchedule>()
                 {
                     new Common.DataModel.EventSchedule
                     {
                          Name= "first schedule", CRONData = "0/2 * * * * ?"
                     }
                 },
                Activities = new List<Common.DataModel.EventActivity>
                {
                    new Common.DataModel.EventActivity()
                    {
                        ActivityId = activityCreate.Result,
                        Name = "Activity 1",
                        Fields = new List<Common.DataModel.EventActivityField>()
                        {
                            new Common.DataModel.EventActivityField()
                            {
                                 Name = "Url", Value = @"https://www.google.com"
                            }
                        }
                    },
                    new Common.DataModel.EventActivity()
                    {
                        ActivityId = activityCreate.Result,
                        Name = "Activity 2",
                        Fields = new List<Common.DataModel.EventActivityField>()
                        {
                            new Common.DataModel.EventActivityField()
                            {
                                 Name = "Url", Value = @"https://www.google.com"
                            }
                        }
                    }

                },
            });

            DateTime endTime = DateTime.Now.AddSeconds(10);

            while (DateTime.Now < endTime)
            {
                await Task.Delay(1000);
            }

            await executionEngine.Stop();
            await schedulerEngine.Stop();
        }
    }
}
