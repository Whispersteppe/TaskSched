using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TaskSched.SchedulerEngine.Listeners
{
    internal class ListenerBase
    {
        public ListenerBase(ILogger logger) 
        { 
            Logger = logger;
        }

        public ILogger Logger { get; private set; }

        public string ToJsonString(object o)
        {
            try
            {
                string data = JsonConvert.SerializeObject(o);
                return data;
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, "error converting object in logger");
            }

            return "Cannot Convert";
        }
    }
}
