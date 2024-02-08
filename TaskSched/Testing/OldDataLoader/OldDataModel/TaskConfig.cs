using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OldDataLoader.OldDataModel
{

    public class TaskConfig : TaskBaseConfig
    {
        public int TemplateID { get; set; }
        public bool IsActive { get; set; }
        public TriggerConfig Trigger { get; set; }
        public DateTime LastExecution { get; set; } = DateTime.Now;
        public bool AllowLaunchOnStartup { get; set; } = true;

    }
}
