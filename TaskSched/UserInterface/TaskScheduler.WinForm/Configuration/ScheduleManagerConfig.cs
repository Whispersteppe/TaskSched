using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TaskSched.DataStore;

namespace TaskScheduler.WinForm.Configuration
{
    public class ScheduleManagerConfig
    {
        public TaskSchedDbContextConfiguration DatabaseConfig { get; set; }
        public DisplayConfiguration DisplayConfig { get; set; }
        public EngineConfiguration EngineConfig { get; set; }

        public NlogConfig NLog { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JToken> ExtraData { get; set; }
    }

    public class NlogConfig
    {
        public Dictionary<string, JToken> targets { get; set; }
        public List<JToken> rules { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JToken> ExtraData { get; set; }

    }

    public class DisplayConfiguration
    {
        public bool ShowOnStartup { get; set; }
        public WindowCoordinates MainWindowLocation { get; set; } = new WindowCoordinates();
    }

    public class EngineConfiguration
    {
        public bool StartOnStartup { get; set; }
    }

}
