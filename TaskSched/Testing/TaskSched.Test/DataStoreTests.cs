using Bogus;
using TaskSched.DataStore;
using TaskSched.DataStore.DataModel;
using TaskSched.Test.Fakes;
using TaskSched.Test.XUnit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TaskSched.Test
{
    //    [Collection("Context collection")]

    public class DataStoreTests : XUnitTestClassBase
    {

        XUnitCollectionFixture _collectionFixture;
        public DataStoreTests(XUnitClassFixture fixture, XUnitCollectionFixture collectionFixture, ITestOutputHelper output) 
            : base(fixture, collectionFixture, output)
        {
        }

        [Fact]
        public async Task EventCRUD()
        {
            var db = this.CollectionFixture.Repository;

            var taskGet = await CreateEvent();


            taskGet.LastExecution = DateTime.Now;
            db.Events.Update(taskGet);
            await db.SaveChangesAsync();

            await DeleteEvent(taskGet);

        }

        [Fact]
        public async Task EventScheduleCRUD()
        {
            var db = this.CollectionFixture.Repository;

            var taskGet = await CreateEvent();
            var schedule = await CreateEventSchedule(taskGet.Id);

            schedule.CRONData = "1 1 1 1 1 1";
            db.Events.Update(taskGet);
            await db.SaveChangesAsync();



            await DeleteEventSchedule(schedule);
            await DeleteEvent(taskGet);
        }

        [Fact]
        public async Task ActivityCRUD()
        {
            var db = this.CollectionFixture.Repository;

            var item = await CreateActivity();

            item.ActivityType = ActivityTypeEnum.ExternalProgram;
            db.Activities.Update(item);
            await db.SaveChangesAsync();

            await DeleteActivity(item);

        }

        [Fact]
        public async Task ActivityFieldCRUD()
        {
            var db = this.CollectionFixture.Repository;

            var item = await CreateActivity();
            var field = await CreateActivityField(item.Id);


            await DeleteActivityField(field);
            await DeleteActivity(item);

        }

        [Fact]
        public async Task EventActivityCRUD()
        {
            var db = this.CollectionFixture.Repository;

            var task = await CreateEvent();
            var schedule = await CreateEventSchedule(task.Id);
            var action = await CreateActivity();

            var item = await CreateEventActivity(task.Id, action.Id);

            item.Name = DateTime.UtcNow.ToString();
            db.EventActivities.Update(item);
            await db.SaveChangesAsync();

            await DeleteEventActivity(item);
            await DeleteActivity(action);
            await DeleteEventSchedule(schedule);
            await DeleteEvent(task);

        }


        [Fact]
        public async Task EventActivityFieldCRUD()
        {
            var db = this.CollectionFixture.Repository;

            var task = await CreateEvent();
            var schedule = await CreateEventSchedule(task.Id);
            var action = await CreateActivity();
            var actionField = await CreateActivityField(action.Id);
            var taskAction = await CreateEventActivity(task.Id, action.Id);
            var item = await CreateEventActivityField(taskAction.Id, actionField.Id);

            item.Name = DateTime.UtcNow.ToString();
            db.EventActivityFields.Update(item);
            await db.SaveChangesAsync();

            await DeleteEventActivityField(item);
            await DeleteEventActivity(taskAction);
            await DeleteActivityField(actionField);
            await DeleteActivity(action);
            await DeleteEventSchedule(schedule);
            await DeleteEvent(task);

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