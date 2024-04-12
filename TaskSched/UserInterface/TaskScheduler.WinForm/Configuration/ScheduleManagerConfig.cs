using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using TaskSched.DataStore;

namespace TaskScheduler.WinForm.Configuration
{
    public class ScheduleManagerConfig
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [DisplayName("Database")]
        [Category("Configuration")]

        public TaskSchedDbContextConfiguration DatabaseConfig { get; set; }

        [Category("Configuration")]
        [DisplayName("Display")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public DisplayConfiguration DisplayConfig { get; set; }

        [Category("Configuration")]
        [DisplayName("Engine")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public EngineConfiguration EngineConfig { get; set; }

        [Category("Configuration")]
        [DisplayName("NLog")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public NlogConfig NLog { get; set; }

        [DisplayName("Extra Data")]
        [JsonExtensionData]
        public Dictionary<string, JToken>? ExtraData { get; set; }

        public override string ToString()
        {
            return "Schedule Manager Configuration";
        }
    }

    public class NlogConfig
    {
        //public Dictionary<string, JToken> targets { get; set; }
        public Dictionary<string, NlogTargetConfig> Targets { get; set; }
        //public List<JToken> rules { get; set; }
        public List<NlogRuleConfig> Rules { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JToken>? ExtraData { get; set; }

        public override string ToString()
        {
            return "NLog Configuration";
        }

    }

    /*
     *       "errorLog": {
        "type": "File",
        "maxArchiveDays": 3,
        "fileName": "log\\Error-${date:format=yyyy-MM-dd HH}.log",
        "layout": "${longdate}|${event-properties:item=EventId_Id}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}"
      }
     */
    public class NlogTargetConfig
    {
        public string? Type { get; set; }
        public int? MaxArchiveDays { get; set; }
        public string? FileName { get; set; }
        public string? Layout { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JToken>? ExtraData { get; set; }

        public override string ToString()
        {
            return $"{Type}";
        }

    }

    public class NlogRuleConfig
    {
        public string? Logger { get; set; }
        public string? MinLevel { get; set; }
        public string? WriteTo { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JToken>? ExtraData { get; set; }

        public override string ToString()
        {
            return $"{WriteTo} - {MinLevel} - {Logger}";
        }


    }

    public class DisplayConfiguration
    {
        [DisplayName("Show on Startup")]
        public bool ShowOnStartup { get; set; }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [DisplayName("Main Window Location")]
        public WindowCoordinates MainWindowLocation { get; set; } = new WindowCoordinates();

        public override string ToString()
        {
            return "Display Configuration";
        }
    }

    public class EngineConfiguration
    {
        [DisplayName("Start on Startup")]
        public bool StartOnStartup { get; set; }

        public override string ToString()
        {
            return "Engine Configuration";
        }
    }

}
