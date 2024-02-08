using Bogus;
using TaskSched.Common.Interfaces;
using TaskSched.DataStore;
using TaskSched.DataStore.DataModel;
using TaskSched.Test.Fakes;
using TaskSched.Test.XUnit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TaskSched.Test
{
    //    [Collection("Context collection")]

    public class EventStoreTests : XUnitTestClassBase
    {

        XUnitCollectionFixture _collectionFixture;
        public EventStoreTests(XUnitClassFixture fixture, XUnitCollectionFixture collectionFixture, ITestOutputHelper output) 
            : base(fixture, collectionFixture, output)
        {
        }



        [Fact]
        public async Task EventActivityFieldCRUD()
        {
            var db = this.CollectionFixture.Repository;

            var activity = await CreateActivity();
            var activityField = await CreateActivityField(activity.Id);

            IDataStoreMapper mapper = new TaskSched.DataStore.DataStoreMapper();
            IEventStore eventStore = new TaskSched.DataStore.EventStore(db, mapper);


            var eventItem = Fakes.TaskSchedFaker.Model.Events.Create(3, 3);

            eventItem.Activities.ForEach(x => x.ActivityId = activity.Id);


            var rsltCreate = await eventStore.Create(eventItem);
            Assert.NotNull(rsltCreate);

            var rsltGet = await eventStore.Get(rsltCreate.Result);
            Assert.NotNull(rsltGet);

            var rsltGetAll = await eventStore.GetAll();
            Assert.NotNull(rsltGetAll);



            var rsltDelete = eventStore.Delete(rsltCreate.Result);
            Assert.NotNull(rsltDelete);



            await DeleteActivityField(activityField);
            await DeleteActivity(activity);

        }


        [Fact]
        public async Task ActivityFieldCRUD()
        {
            var  db = this.CollectionFixture.Repository;




            IDataStoreMapper mapper = new TaskSched.DataStore.DataStoreMapper();
            IActivityStore activityStore = new TaskSched.DataStore.ActivityStore(db, mapper);


            var activity = Fakes.TaskSchedFaker.Model.Activities.Create(3);


            var rsltCreate = await activityStore.Create(activity);
            Assert.NotNull(rsltCreate);

            var rsltGet = await activityStore.Get(rsltCreate.Result);
            Assert.NotNull(rsltGet);

            var rsltGetAll = await activityStore.GetAll();
            Assert.NotNull(rsltGetAll);



            var rsltDelete = activityStore.Delete(rsltCreate.Result);
            Assert.NotNull(rsltDelete);

        }


        [Fact]
        public async Task CalendarCRUD()
        {
            var db = this.CollectionFixture.Repository;

            IDataStoreMapper mapper = new TaskSched.DataStore.DataStoreMapper();
            ICalendarStore calendarStore = new TaskSched.DataStore.CalendarStore(db, mapper);


            var calendar = Fakes.TaskSchedFaker.Model.Calendars.Create();


            var rsltCreate = await calendarStore.Create(calendar);
            Assert.NotNull(rsltCreate);

            var rsltGet = await calendarStore.Get(rsltCreate.Result);
            Assert.NotNull(rsltGet);

            CalendarRetrievalParameters retrievalParameters = new CalendarRetrievalParameters()
            {
                AddChildEvents = true,
                AsTree = true,
                AddChildFolders = true
            };

            var rsltGetAll = await calendarStore.GetAll(retrievalParameters);

            Assert.NotNull(rsltGetAll);


            var rsltDelete = calendarStore.Delete(rsltCreate.Result);
            Assert.NotNull(rsltDelete);

        }


        #region helpers

        internal async Task<Event> CreateEvent()
        {
            var db = this.CollectionFixture.Repository;

            var task = TaskSchedFaker.Database.Events.Create();
            db.Events.Add(task);
            await db.SaveChangesAsync();

            var taskGet = db.Events.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            return taskGet;

        }

        internal async Task DeleteEvent(Event task)
        {
            var db = this.CollectionFixture.Repository;

            var taskGet = db.Events.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            db.Events.Remove(taskGet);
            await db.SaveChangesAsync();

            var taskAfterDelete = db.Events.FirstOrDefault(x => x.Id == task.Id);
            Assert.Null(taskAfterDelete);
        }

        internal async Task<EventSchedule> CreateEventSchedule(Guid itemId)
        {
            var db = this.CollectionFixture.Repository;

            var task = TaskSchedFaker.Database.EventSchedules.Create(itemId);
            db.EventSchedules.Add(task);
            await db.SaveChangesAsync();

            var taskGet = db.EventSchedules.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            return taskGet;

        }

        internal async Task DeleteEventSchedule(EventSchedule task)
        {
            var db = this.CollectionFixture.Repository;

            var taskGet = db.EventSchedules.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            db.EventSchedules.Remove(taskGet);
            await db.SaveChangesAsync();

            var taskAfterDelete = db.EventSchedules.FirstOrDefault(x => x.Id == task.Id);
            Assert.Null(taskAfterDelete);
        }

        internal async Task<Activity> CreateActivity()
        {
            var db = this.CollectionFixture.Repository;

            var task = TaskSchedFaker.Database.Activities.Create();
            db.Activities.Add(task);
            await db.SaveChangesAsync();

            var taskGet = db.Activities.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            return taskGet;

        }

        internal async Task DeleteActivity(Activity task)
        {
            var db = this.CollectionFixture.Repository;

            var taskGet = db.Activities.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            db.Activities.Remove(taskGet);
            await db.SaveChangesAsync();

            var taskAfterDelete = db.Activities.FirstOrDefault(x => x.Id == task.Id);
            Assert.Null(taskAfterDelete);
        }


        internal async Task<ActivityField> CreateActivityField(Guid itemId)
        {
            var db = this.CollectionFixture.Repository;

            var task = TaskSchedFaker.Database.ActivityFields.Create(itemId);
            db.ActivityFields.Add(task);
            await db.SaveChangesAsync();

            var taskGet = db.ActivityFields.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            return taskGet;

        }

        internal async Task DeleteActivityField(ActivityField task)
        {
            var db = this.CollectionFixture.Repository;

            var taskGet = db.ActivityFields.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            db.ActivityFields.Remove(taskGet);
            await db.SaveChangesAsync();

            var taskAfterDelete = db.ActivityFields.FirstOrDefault(x => x.Id == task.Id);
            Assert.Null(taskAfterDelete);
        }


        internal async Task<EventActivity> CreateEventActivity(Guid itemId, Guid actionId)
        {
            var db = this.CollectionFixture.Repository;

            var task = TaskSchedFaker.Database.EventActivities.Create(itemId, actionId);
            db.EventActivities.Add(task);
            await db.SaveChangesAsync();

            var taskGet = db.EventActivities.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            return taskGet;

        }

        internal async Task DeleteEventActivity(EventActivity task)
        {
            var db = this.CollectionFixture.Repository;

            var taskGet = db.EventActivities.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            db.EventActivities.Remove(taskGet);
            await db.SaveChangesAsync();

            var taskAfterDelete = db.EventActivities.FirstOrDefault(x => x.Id == task.Id);
            Assert.Null(taskAfterDelete);
        }


        internal async Task<EventActivityField> CreateEventActivityField(Guid actionId, Guid actionFieldId)
        {
            var db = this.CollectionFixture.Repository;

            var task = TaskSchedFaker.Database.EventActivityFields.Create(actionId, actionFieldId);
            db.EventActivityFields.Add(task);
            await db.SaveChangesAsync();

            var taskGet = db.EventActivityFields.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            return taskGet;

        }

        internal async Task DeleteEventActivityField(EventActivityField task)
        {
            var db = this.CollectionFixture.Repository;

            var taskGet = db.EventActivityFields.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            db.EventActivityFields.Remove(taskGet);
            await db.SaveChangesAsync();

            var taskAfterDelete = db.EventActivityFields.FirstOrDefault(x => x.Id == task.Id);
            Assert.Null(taskAfterDelete);
        }

        #endregion
    }
}