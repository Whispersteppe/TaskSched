using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [ReadOnly(true)]
        [Browsable(false)]
        [Category("Message")]
        public string Message { get; set; }
        [ReadOnly(true)]
        [Browsable(false)]
        public LogEventInfo Extended { get; set; }
        [ReadOnly(true)]
        [Category("ID")]
        public int SequenceID { get => Extended.SequenceID; }
        [ReadOnly(true)]
        [Category("ID")]
        public DateTime TimeStamp { get => Extended.TimeStamp; }
        [ReadOnly(true)]
        [Category("ID")]
        public LogLevel Level { get => Extended.Level; }
        [ReadOnly(true)]
        [Category ("Exception")]
        public StackFrame UserStackFrame { get => Extended.UserStackFrame; }
        [ReadOnly(true)]
        [Category("Exception")]
        public int UserStackFrameNumber { get => Extended.UserStackFrameNumber; }
        [ReadOnly(true)]
        [Category("Exception")]
        public StackTrace StackTrace { get => Extended.StackTrace; }
        [ReadOnly(true)]
        [Category("Location")]
        public string CallerClassName { get => Extended.CallerClassName; }
        [ReadOnly(true)]
        [Category("Location")]
        public string CallerMemberName { get => Extended.CallerMemberName; }
        [ReadOnly(true)]
        [Category("Location")]
        public string CallerFilePath { get => Extended.CallerFilePath; }
        [ReadOnly(true)]
        [Category("Location")]
        public int CallerLineNumber { get => Extended.CallerLineNumber; }
        [ReadOnly(true)]
        [Category("Exception")]
        public Exception? Exception { get => Extended.Exception; }
        [ReadOnly(true)]
        [Category("ID")]
        public string LoggerName { get => Extended.LoggerName; }
        [ReadOnly(true)]
        public object[] Parameters { get => Extended.Parameters; }
        [ReadOnly(true)]
        [Category("Message")]
        public string FormattedMessage { get => Extended.FormattedMessage; }

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
