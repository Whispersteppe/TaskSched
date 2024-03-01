using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSched.Common.DataModel
{
    /// <summary>
    /// Details on a particular activity that can be performed by the task sched
    /// </summary>
    /// <remarks>
    /// think along the lines of Run Excel, or Open this file in NotePad.
    /// the ActivityType will be the indicator as to how to do the command
    /// </remarks>
    public class Activity
    {
        /// <summary>
        /// Activity ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// the type of activity
        /// </summary>
        public Guid ActivityHandlerId { get; set; }

        /// <summary>
        /// Name of the activity
        /// </summary>
        public string Name { get; set; } = "New Activity";

        /// <summary>
        /// the set of default fields associated with this particular activity
        /// </summary>
        public List<ActivityField>? DefaultFields { get; set; } = new List<ActivityField>();
    }


}
