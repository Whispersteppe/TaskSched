using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.Interfaces;
using TaskSched.Test.XUnit;
using Xunit.Abstractions;
using TaskSched.Common.DataModel;

namespace TaskSched.Test
{
    public class ExecutionStoreTests : XUnitTestClassBase
    {
        public ExecutionStoreTests(XUnitClassFixture fixture, XUnitCollectionFixture collectionFixture, ITestOutputHelper output) : base(fixture, collectionFixture, output)
        {
        }

        private Activity CreateActivity(IExecutionHandler handler)
        {
            Activity activity = new Activity()
            {
                ActivityHandlerId = handler.HandlerInfo.HandlerId,
                Id = Guid.NewGuid(),
                Name = "test activity",
                DefaultFields = new List<ActivityField>()
                 {
                    new ActivityField() { Id = Guid.NewGuid(), Name = "ExecutablePath", IsReadOnly = true, Value = "C:\\Program Files\\Mozilla Firefox\\firefox.exe" },
                    new ActivityField() { Id = Guid.NewGuid(), Name = "CommandLine", IsReadOnly = true, Value = "-new-tab {0}" },
                    new ActivityField() { Id = Guid.NewGuid(), Name = "Url", IsReadOnly = false, Value = "https://www.google.com/rss" },

                }
            };

            return activity;
        }

        private EventActivity CreateEventActivity(Activity activity)
        {
            EventActivity eventActivity = new EventActivity()
            {
                ActivityId = activity.Id,
                Name = "glibnortz",
                Id = Guid.NewGuid(),
                Fields = new List<EventActivityField>()
            };

            foreach (var field in activity.DefaultFields)
            {
                if (field.IsReadOnly == false)
                {
                    eventActivity.Fields.Add(new EventActivityField()
                    {
                        Id = Guid.NewGuid(),
                        ActivityFieldId = field.Id,
                        Name = field.Name,
                        Value = "glibnortz"
                    });
                }
            }

            return eventActivity;

        }

        private Event CreateEvent(EventActivity eventActivity)
        {
            Event eventItem = new Event()
            {
                Activities = new List<EventActivity>() { eventActivity },
                CalendarId = Guid.NewGuid(),
                CatchUpOnStartup = true,
                Id = Guid.NewGuid(),
                IsActive = true,
                LastExecution = DateTime.Now.AddDays(-5),
                NextExecution = DateTime.Now,
                Name = "My Event",
                Schedules = new List<EventSchedule>()
                {
                    new EventSchedule()
                    {
                        Id = Guid.NewGuid(),
                        CRONData = "0 0 8 * * * ?",
                        Name = "Every Day at 8am"
                    }
                }
            };

            return eventItem;
        }


        [Fact]
        public async Task CheckExecutionStore()
        {
            ILogger logger = this.GetLogger(memberName: nameof(ExecutionStoreTests));

            IExecutionStore executionStore = new ExecutionStore.ExecutionStore(logger);

            var handlers = await executionStore.GetExecutionHandlers();
            Assert.NotEmpty(handlers);
            Assert.NotEmpty(handlers);

            var handler = await executionStore.GetExecutionHandler(handlers[0].HandlerInfo.HandlerId);
            Assert.NotNull(handler);

            Activity activity = CreateActivity(handler);


            var rsltValidateActivity = await handler.ValidateActivity(activity);
            Assert.NotNull(rsltValidateActivity);
            Assert.Equal(ResultMessageSeverity.OK, rsltValidateActivity.Status);

            EventActivity eventActivity = CreateEventActivity(activity);


            var rsltValidateEvent = await handler.ValidateEventActivity(activity, eventActivity);
            Assert.NotNull(rsltValidateEvent);
            Assert.Equal(ResultMessageSeverity.OK, rsltValidateEvent.Status);


            Event eventItem = CreateEvent(eventActivity);



            ActivityContext activityContext = new ActivityContext()
            {
                Activity = activity,
                EventActivity = eventActivity,
                EventItem = eventItem
            };

            var rsltExecute = await handler.HandleActivity(activityContext);
            Assert.NotNull(rsltExecute);

        }


        [Fact]
        public async Task CheckActivityValidation()
        {
            ILogger logger = this.GetLogger(memberName: nameof(ExecutionStoreTests));

            IExecutionStore executionStore = new ExecutionStore.ExecutionStore(logger);

            var handlers = await executionStore.GetExecutionHandlers();
            Assert.NotEmpty(handlers);
            Assert.NotEmpty(handlers);

            var handler = await executionStore.GetExecutionHandler(handlers[0].HandlerInfo.HandlerId);
            Assert.NotNull(handler);

            Activity activity = CreateActivity(handler);
            activity.ActivityHandlerId = Guid.NewGuid();
            activity.DefaultFields.RemoveAt(0);


            var rsltValidateActivity = await handler.ValidateActivity(activity);
            Assert.NotNull(rsltValidateActivity);
            Assert.Equal(ResultMessageSeverity.Error, rsltValidateActivity.Status);

            rsltValidateActivity = await handler.ValidateActivity(null);
            Assert.NotNull(rsltValidateActivity);
            Assert.Equal(ResultMessageSeverity.Error, rsltValidateActivity.Status);


        }

        [Fact]
        public async Task CheckEventActivityValidation()
        {
            ILogger logger = this.GetLogger(memberName: nameof(ExecutionStoreTests));

            IExecutionStore executionStore = new ExecutionStore.ExecutionStore(logger);

            var handlers = await executionStore.GetExecutionHandlers();
            Assert.NotEmpty(handlers);
            Assert.NotEmpty(handlers);

            var handler = await executionStore.GetExecutionHandler(handlers[0].HandlerInfo.HandlerId);
            Assert.NotNull(handler);

            Activity activity = CreateActivity(handler);


            EventActivity eventActivity = CreateEventActivity(activity);
            //eventActivity.ActivityId = Guid.NewGuid();
            eventActivity.Fields.RemoveAt(0);


            var rsltValidateEvent = await handler.ValidateEventActivity(activity, eventActivity);
            Assert.NotNull(rsltValidateEvent);
            Assert.Equal(ResultMessageSeverity.Error, rsltValidateEvent.Status);

            eventActivity.ActivityId = Guid.NewGuid();

            rsltValidateEvent = await handler.ValidateEventActivity(activity, eventActivity);
            Assert.NotNull(rsltValidateEvent);
            Assert.Equal(ResultMessageSeverity.Error, rsltValidateEvent.Status);

            rsltValidateEvent = await handler.ValidateEventActivity(null, null);
            Assert.NotNull(rsltValidateEvent);
            Assert.Equal(ResultMessageSeverity.Error, rsltValidateEvent.Status);

            rsltValidateEvent = await handler.ValidateEventActivity(activity, null);
            Assert.NotNull(rsltValidateEvent);
            Assert.Equal(ResultMessageSeverity.Error, rsltValidateEvent.Status);



        }
    }
}
