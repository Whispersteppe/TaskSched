using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSched.Common.DataModel
{
    public class ExpandedResult
    {
        public ResultMessageSeverity Status 
        { 
            get
            {
                if (Messages == null) return ResultMessageSeverity.OK;
                if (Messages.Count == 0) return ResultMessageSeverity.OK;
                return Messages.Max(x => x.Severity);

            } 
        }
        public List<ResultMessage> Messages { get; set; } = new List<ResultMessage>();
    }

    public class ExpandedResult<T> : ExpandedResult
    {
        public T? Result { get; set; }
    }

    public class ResultMessage
    {
        public ResultMessageSeverity Severity { get; set; }
        public string Message { get; set; }
    }

    public enum ResultMessageSeverity
    {
        OK = 0,
        Information = 1,
        Warning = 2,
        Error = 3,
        Catastrophic = 4,
    }
}
