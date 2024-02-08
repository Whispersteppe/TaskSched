using System;
using System.Collections.Generic;
using System.Text;

namespace OldDataLoader.OldDataModel
{
    public class TriggerCronConfig : TriggerConfig
    {
        public override TriggerType TriggerType => TriggerType.CronTrigger;
        public string CronExpression { get; set; }
    }
}
