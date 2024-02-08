using System;
using System.Collections.Generic;
using System.Text;

namespace OldDataLoader.OldDataModel
{
    public class TriggerDatePickerConfig : TriggerConfig
    {
        public override TriggerType TriggerType => TriggerType.DatePicker;

        public string CronExpression { get; set; }

    }
}
