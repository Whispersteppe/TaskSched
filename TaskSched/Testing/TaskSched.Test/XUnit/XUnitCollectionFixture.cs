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
            TaskSchedDbContext repository = new TaskSchedDbContext(config);

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


        public TaskSchedDbContext Repository
        {
            get
            {
                TaskSchedDbContextConfiguration config = GetConfig<TaskSchedDbContextConfiguration>("Repository");
                TaskSchedDbContext repository = new TaskSchedDbContext(config);

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
