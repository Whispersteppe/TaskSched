using Bogus;
using TaskSched.Common.DataModel;
using TaskSched.Common.FieldValidator;
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

        public EventStoreTests(XUnitClassFixture fixture, XUnitCollectionFixture collectionFixture, ITestOutputHelper output) 
            : base(fixture, collectionFixture, output)
        {
        }



        [Fact]
        public async Task EventActivityFieldCRUD()
        {
            var factory = CollectionFixture.RepositoryFactory;

            var activity = await CreateActivity();
            var activityField = await CreateActivityField(activity.Id);

            IFieldValidatorSet fieldValidatorSet = new FieldValidatorSet();
            IDataStoreMapper mapper = new TaskSched.DataStore.DataStoreMapper();
            IEventStore eventStore = new TaskSched.DataStore.EventStore(factory, mapper, fieldValidatorSet);


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
            var factory = CollectionFixture.RepositoryFactory;




            IFieldValidatorSet fieldValidatorSet = new FieldValidatorSet();
            IDataStoreMapper mapper = new TaskSched.DataStore.DataStoreMapper();
            IActivityStore activityStore = new TaskSched.DataStore.ActivityStore(factory, mapper, fieldValidatorSet);


            var activity = Fakes.TaskSchedFaker.Model.Activities.Create();

            //  add fields
            activity.DefaultFields.Add(Fakes.TaskSchedFaker.Model.ActivityFields.Create( FieldTypeEnum.String, "string data"));
            activity.DefaultFields.Add(Fakes.TaskSchedFaker.Model.ActivityFields.Create(FieldTypeEnum.Url, "https://www.mozilla.org"));
            activity.DefaultFields.Add(Fakes.TaskSchedFaker.Model.ActivityFields.Create(FieldTypeEnum.Number, "12345"));
            activity.DefaultFields.Add(Fakes.TaskSchedFaker.Model.ActivityFields.Create(FieldTypeEnum.DateTime, DateTime.Now.ToString()));
            activity.DefaultFields.Add(Fakes.TaskSchedFaker.Model.ActivityFields.Create(FieldTypeEnum.ExecutablePath, @"c:\MyDFatae.exe"));



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
        public async Task FolderCRUD()
        {
            var factory = CollectionFixture.RepositoryFactory;

            IDataStoreMapper mapper = new TaskSched.DataStore.DataStoreMapper();
            IFolderStore folderStore = new TaskSched.DataStore.FolderStore(factory, mapper);


            var folder = Fakes.TaskSchedFaker.Model.Folders.Create();


            var rsltCreate = await folderStore.Create(folder);
            Assert.NotNull(rsltCreate);

            var rsltGet = await folderStore.Get(rsltCreate.Result);
            Assert.NotNull(rsltGet);

            FolderRetrievalParameters retrievalParameters = new FolderRetrievalParameters()
            {
                AddChildEvents = true,
                AsTree = true,
                AddChildFolders = true
            };

            var rsltGetAll = await folderStore.GetAll(retrievalParameters);

            Assert.NotNull(rsltGetAll);


            var rsltDelete = folderStore.Delete(rsltCreate.Result);
            Assert.NotNull(rsltDelete);

        }


        #region helpers

        internal async Task<DataStore.DataModel.Event> CreateEvent()
        {
            var db = this.CollectionFixture.Repository;

            var task = TaskSchedFaker.Database.Events.Create();
            db.Events.Add(task);
            await db.SaveChangesAsync();

            var taskGet = db.Events.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            return taskGet;

        }

        internal async Task DeleteEvent(DataStore.DataModel.Event task)
        {
            var db = this.CollectionFixture.Repository;

            var taskGet = db.Events.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            db.Events.Remove(taskGet);
            await db.SaveChangesAsync();

            var taskAfterDelete = db.Events.FirstOrDefault(x => x.Id == task.Id);
            Assert.Null(taskAfterDelete);
        }

        internal async Task<DataStore.DataModel.EventSchedule> CreateEventSchedule(Guid itemId)
        {
            var db = this.CollectionFixture.Repository;

            var task = TaskSchedFaker.Database.EventSchedules.Create(itemId);
            db.EventSchedules.Add(task);
            await db.SaveChangesAsync();

            var taskGet = db.EventSchedules.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            return taskGet;

        }

        internal async Task DeleteEventSchedule(DataStore.DataModel.EventSchedule task)
        {
            var db = this.CollectionFixture.Repository;

            var taskGet = db.EventSchedules.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            db.EventSchedules.Remove(taskGet);
            await db.SaveChangesAsync();

            var taskAfterDelete = db.EventSchedules.FirstOrDefault(x => x.Id == task.Id);
            Assert.Null(taskAfterDelete);
        }

        internal async Task<DataStore.DataModel.Activity> CreateActivity()
        {
            var db = this.CollectionFixture.Repository;

            var task = TaskSchedFaker.Database.Activities.Create();
            db.Activities.Add(task);
            await db.SaveChangesAsync();

            var taskGet = db.Activities.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            return taskGet;

        }

        internal async Task DeleteActivity(DataStore.DataModel.Activity task)
        {
            var db = this.CollectionFixture.Repository;

            var taskGet = db.Activities.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            db.Activities.Remove(taskGet);
            await db.SaveChangesAsync();

            var taskAfterDelete = db.Activities.FirstOrDefault(x => x.Id == task.Id);
            Assert.Null(taskAfterDelete);
        }


        internal async Task<DataStore.DataModel.ActivityField> CreateActivityField(Guid itemId)
        {
            var db = this.CollectionFixture.Repository;

            var task = TaskSchedFaker.Database.ActivityFields.Create(itemId);
            db.ActivityFields.Add(task);
            await db.SaveChangesAsync();

            var taskGet = db.ActivityFields.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            return taskGet;

        }

        internal async Task DeleteActivityField(DataStore.DataModel.ActivityField task)
        {
            var db = this.CollectionFixture.Repository;

            var taskGet = db.ActivityFields.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            db.ActivityFields.Remove(taskGet);
            await db.SaveChangesAsync();

            var taskAfterDelete = db.ActivityFields.FirstOrDefault(x => x.Id == task.Id);
            Assert.Null(taskAfterDelete);
        }


        internal async Task<DataStore.DataModel.EventActivity> CreateEventActivity(Guid itemId, Guid actionId)
        {
            var db = this.CollectionFixture.Repository;

            var task = TaskSchedFaker.Database.EventActivities.Create(itemId, actionId);
            db.EventActivities.Add(task);
            await db.SaveChangesAsync();

            var taskGet = db.EventActivities.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            return taskGet;

        }

        internal async Task DeleteEventActivity(DataStore.DataModel.EventActivity task)
        {
            var db = this.CollectionFixture.Repository;

            var taskGet = db.EventActivities.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            db.EventActivities.Remove(taskGet);
            await db.SaveChangesAsync();

            var taskAfterDelete = db.EventActivities.FirstOrDefault(x => x.Id == task.Id);
            Assert.Null(taskAfterDelete);
        }


        internal async Task<DataStore.DataModel.EventActivityField> CreateEventActivityField(Guid actionId, Guid actionFieldId)
        {
            var db = this.CollectionFixture.Repository;

            var task = TaskSchedFaker.Database.EventActivityFields.Create(actionId, actionFieldId);
            db.EventActivityFields.Add(task);
            await db.SaveChangesAsync();

            var taskGet = db.EventActivityFields.FirstOrDefault(x => x.Id == task.Id);
            Assert.NotNull(taskGet);

            return taskGet;

        }

        internal async Task DeleteEventActivityField(DataStore.DataModel.EventActivityField task)
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