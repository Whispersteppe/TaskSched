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
    [Target("InternalLogTarget")]
    public class NLogCustomTarget :TargetWithLayout
    {

        List<LogEventInfo> _logs;

        public NLogCustomTarget()
        {
            _logs = new List<LogEventInfo>();
        }


        public List<LogEventInfo> Logs()
        {
            return _logs;
        }

        public List<LogEventInfo> Logs(LogLevel minLevel)
        {
            var selectedLogs = _logs.Where(x => x.Level >= minLevel).ToList();

            return selectedLogs;
        }


        protected override void Write(LogEventInfo logEvent)
        {
            _logs.Add(logEvent);

            //  remove the oldest items
            while (_logs.Count > 1000)
            {
                _logs.RemoveAt(0);
            }


            string logMessage = RenderLogEvent(this.Layout, logEvent);

            SendMessage(logMessage);    
        }

        private void SendMessage(string message)
        {

            Debug.WriteLine(message);
            // TODO - write me 
            //  do my callouts here
        }
    }
}
