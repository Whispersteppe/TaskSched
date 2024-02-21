using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.DataStore;

namespace TaskScheduler.WinForm
{
    public class ScheduleManagerConfig
    {
        public TaskSchedDbContextConfiguration DatabaseConfig { get; set; }
    }
}
