using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSched.Common.DataModel
{
    public class ExpandedResult
    {
        public List<ResultMessage> Messages { get; set; } = new List<ResultMessage>();
    }

    public class ExpandedResult<T> : ExpandedResult
    {
        public T Result { get; set; }
    }

    public class ResultMessage
    {
        public ResultMessageSeverity Severity { get; set; }
        public string Message { get; set; }
    }

    public enum ResultMessageSeverity
    {
        OK,
        Information,
        Warning,
        Error,
        Catastrophic,
    }
}
