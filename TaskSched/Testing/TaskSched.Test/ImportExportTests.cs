using FluentAssertions;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.FieldValidator;
using TaskSched.Common.Interfaces;
using TaskSched.DataStore;
using TaskSched.Test.XUnit;
using Xunit.Abstractions;

namespace TaskSched.Test
{
    public class ImportExportTests : XUnitTestClassBase
    {
        IActivityStore _activityStore;
        IEventStore _eventStore;
        IFolderStore _folderStore;
        IImportExport _importExport;


        public ImportExportTests(XUnitClassFixture fixture, XUnitCollectionFixture collectionFixture, ITestOutputHelper output) 
            : base(fixture, collectionFixture, output)
        {
            //  we want our own connection, since the default is empty
            TaskSchedDbContextConfiguration dbConfig = new TaskSchedDbContextConfiguration()
            {
                DataSource = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "TaskSched.sqlite")
            };

            TaskSchedDbContextFactory contextFactory = new TaskSchedDbContextFactory(dbConfig);

            IDataStoreMapper _mapper = new DataStoreMapper();
            IFieldValidatorSet fieldValidatorSet = new FieldValidatorSet();

            _activityStore = new ActivityStore(contextFactory, _mapper, fieldValidatorSet, this.GetLogger<ActivityStore>());
            _eventStore = new EventStore(contextFactory, _mapper, fieldValidatorSet, this.GetLogger<EventStore>());
            _folderStore = new FolderStore(contextFactory, _mapper, this.GetLogger<FolderStore>());

            _importExport = new ImportExport(_eventStore, _folderStore, _activityStore);
        }

        [Fact]
        public async Task ExportAndImportData()
        {
            var rsltExport = await _importExport.ExportData();
            rsltExport.Should().NotBeNull();
            rsltExport.Result.Should().NotBeNull();
            WriteLine(rsltExport.Result);

            string fileName = "TaskSchedBork.sqlite";
            //  now lets cram it into the empty database
            TaskSchedDbContextConfiguration dbConfig = new TaskSchedDbContextConfiguration()
            {
                DataSource = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), fileName)
            };

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            TaskSchedDbContextFactory contextFactory = new TaskSchedDbContextFactory(dbConfig);

            contextFactory.GetConnection().Database.EnsureCreated();

            IDataStoreMapper _mapper = new DataStoreMapper();
            IFieldValidatorSet fieldValidatorSet = new FieldValidatorSet();

            IActivityStore activityStore = new ActivityStore(contextFactory, _mapper, fieldValidatorSet, this.GetLogger<ActivityStore>());
            IEventStore eventStore = new EventStore(contextFactory, _mapper, fieldValidatorSet, this.GetLogger<EventStore>());
            IFolderStore folderStore = new FolderStore(contextFactory, _mapper, this.GetLogger<FolderStore>());

            IImportExport importExport = new ImportExport(eventStore, folderStore, activityStore);

            var rsltImport = await importExport.ImportData(rsltExport.Result);
            WriteLine(rsltImport);
        }
    }
}
