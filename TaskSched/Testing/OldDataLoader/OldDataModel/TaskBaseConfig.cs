using System;
using System.Collections.Generic;
using System.Text;

namespace OldDataLoader.OldDataModel
{
    public class TaskBaseConfig
    {
        public string Name { get; set; }

        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

    }
}
