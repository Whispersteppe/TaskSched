using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler.WinForm.NLogCustom
{
    [Target("InternalTarget")]
    public class NLogCustomTarget :TargetWithLayout
    {
        protected override void Write(LogEventInfo logEvent)
        {
            string logMessage = RenderLogEvent(this.Layout, logEvent);

            SendMessage(logMessage);    
        }

        private void SendMessage(string message)
        {
            // TODO - write me 
            //  do my callouts here
        }
    }
}
