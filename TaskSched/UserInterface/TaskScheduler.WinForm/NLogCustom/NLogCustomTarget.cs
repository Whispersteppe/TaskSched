using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler.WinForm.NLogCustom
{

    public delegate void OnLogMessage(LogEventInfoExtended logEventInfo);

    public class LogEventInfoExtended
    {
        public LogEventInfoExtended(LogEventInfo logInfo, string message) 
        { 
            Message = message;
            Extended = logInfo;
        }

        public string Message { get; set; }
        public LogEventInfo Extended { get; set; }
    }

    public interface ILogEmitter
    {
        event OnLogMessage OnLogMessage;

    }


    [Target("InternalLogTarget")]
    public class NLogCustomTarget :TargetWithLayout, ILogEmitter
    {
        public event OnLogMessage OnLogMessage;

        List<LogEventInfoExtended> _logs;

        public NLogCustomTarget()
        {
            _logs = new List<LogEventInfoExtended>();
        }


        public List<LogEventInfoExtended> Logs()
        {
            return _logs;
        }

        public List<LogEventInfoExtended> Logs(LogLevel minLevel)
        {
            var selectedLogs = _logs.Where(x => x.Extended.Level >= minLevel).ToList();

            return selectedLogs;
        }


        protected override void Write(LogEventInfo logEvent)
        {

            string logMessage = RenderLogEvent(this.Layout, logEvent);
            LogEventInfoExtended logInfo = new LogEventInfoExtended(logEvent, logMessage);


            _logs.Add(logInfo);

            //  remove the oldest items
            while (_logs.Count > 1000)
            {
                _logs.RemoveAt(0);
            }

            OnLogMessage?.Invoke(logInfo);

        }


    }
}
