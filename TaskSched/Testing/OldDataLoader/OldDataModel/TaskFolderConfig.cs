using System;
using System.Collections.Generic;
using System.Text;

namespace OldDataLoader.OldDataModel
{
    public class TaskFolderConfig : TaskBaseConfig
    {
        public List<TaskBaseConfig> ChildItems { get; set; } = new List<TaskBaseConfig>();


    }
}
