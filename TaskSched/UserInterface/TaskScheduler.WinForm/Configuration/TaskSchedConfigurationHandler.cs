using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
namespace TaskScheduler.WinForm.Configuration
{
    public class TaskSchedConfigurationHandler
    {
        readonly IConfiguration _configuration;
        readonly ScheduleManagerConfig _config;

        readonly string _fileName = "TaskScheduler.json";
        public TaskSchedConfigurationHandler()
        {
            _config = new ScheduleManagerConfig();

            string jsonString = File.ReadAllText(_fileName);

            _configuration = new ConfigurationBuilder()
                .AddJsonFile(_fileName)
                .Build();

            _config = JsonConvert.DeserializeObject<ScheduleManagerConfig>(jsonString);

            //_configuration.Bind(_config);

        }

        public void SaveConfiguration()
        {


            string jsonData = JsonConvert.SerializeObject(_config, Formatting.Indented);
            File.WriteAllText(_fileName, jsonData);
        }

        public IConfigurationSection NLogConfig
        {
            get
            {
                IConfigurationSection nlogConfig = _configuration.GetSection("NLog");

                return nlogConfig;
            }

        }

        public ScheduleManagerConfig Configuration
        {
            get
            {
                return _config;
            }
        }



    }
}
