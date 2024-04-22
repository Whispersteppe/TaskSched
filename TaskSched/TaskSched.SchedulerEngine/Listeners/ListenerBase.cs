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

    }
}
