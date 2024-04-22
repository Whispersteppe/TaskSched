using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler.WinForm.Configuration
{

    [AttributeUsage(AttributeTargets.Assembly)]
    public class BuildDateTimeAttribute : Attribute
    {
        public DateTime Built { get; }
        public BuildDateTimeAttribute(string date)
        {
            this.Built = DateTime.Parse(date);
        }
    }
}
