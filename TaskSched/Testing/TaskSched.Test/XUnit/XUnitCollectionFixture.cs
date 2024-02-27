using Microsoft.Extensions.Configuration;
using TaskSched.DataStore;

namespace TaskSched.Test.XUnit
{

    /// <summary>
    /// collection fixture.  things that are shared between all unit tests without reloading
    /// </summary>
    public class XUnitCollectionFixture : IDisposable
    {
        IConfigurationRoot _config;

        public XUnitCollectionFixture()
        {
            _config = SetupConfig();

            //  refresh the database
            TaskSchedDbContextConfiguration config = GetConfig<TaskSchedDbContextConfiguration>("Repository");
            TaskSchedDbContextFactory contextFactory = new TaskSchedDbContextFactory(config);
            TaskSchedDbContext repository = contextFactory.GetConnection();

            if (File.Exists(config.DataSource))
            {
                repository.Database.EnsureDeleted();
            }

            repository.Database.EnsureCreated();


        }

        /// <summary>
        /// set up the configuration
        /// </summary>
        /// <returns></returns>
        private IConfigurationRoot SetupConfig()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("TestSettings.json")
                //                .AddEnvironmentVariables()
                .Build();

            _config = config;

            return config;

        }

        TaskSchedDbContextFactory? _contextFactory = null;

        public TaskSchedDbContextFactory RepositoryFactory 
        { 
            get
            {
                if (_contextFactory == null)
                {
                    TaskSchedDbContextConfiguration config = GetConfig<TaskSchedDbContextConfiguration>("Repository");
                    _contextFactory = new TaskSchedDbContextFactory(config);
                }

                return _contextFactory;

            }
        }

        public TaskSchedDbContext Repository
        {
            get
            {
                TaskSchedDbContext repository = RepositoryFactory.GetConnection();

                return repository;

            }
        }




        /// <summary>
        /// get a config and bind it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configName"></param>
        /// <returns></returns>
        public T GetConfig<T>(string configName) where T : new()
        {

            T config = new();

            _config.Bind(configName, config);

            return config;
        }

        /// <summary>
        /// dispose of anything that needs disposing
        /// </summary>
        public void Dispose()
        {
            //  nothing to dispose.  carry on
            GC.SuppressFinalize(this);
        }
    }
}
