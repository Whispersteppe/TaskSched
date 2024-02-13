using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskSched.Common.Interfaces;
using TaskSched.ExecutionEngine;
using TaskSched.ExecutionStore;

namespace TaskSched.Test.Console
{
    /// <summary>
    /// test console app
    /// </summary>
    public class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.WriteLine("Hello, World!");

            //  we're not doing DI yet.
            // HostApplicationBuilder builder = Host.CreateApplicationBuilder();

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
                         Name = "fieldName",
                         Value= "fieldValue",
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
                          Name= "first schedule", CRONData = "0 * * * * ?"
                     }
                 },
                Activities = new List<Common.DataModel.EventActivity>
                {
                    new Common.DataModel.EventActivity()
                    {
                        ActivityId = activityCreate.Result, 
                        Name = "Activity 1",
                    },
                    new Common.DataModel.EventActivity()
                    {
                        ActivityId = activityCreate.Result,
                        Name = "Activity 1",
                    }

                },
            });



            while (System.Console.KeyAvailable == false)
            {
                await Task.Delay(1000);
            }

            await executionEngine.Stop();
            await schedulerEngine.Stop();

        }
    }
}
