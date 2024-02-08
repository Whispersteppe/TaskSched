using System;
using System.Collections.Generic;
using System.Text;

namespace OldDataLoader.OldDataModel
{
    public class QuartzFactoryConfig
    {

        public int MaxConcurrency { get; set; }
        public string InstanceName { get; set; }
        public string JobStoreType { get; set; }

    }
}
