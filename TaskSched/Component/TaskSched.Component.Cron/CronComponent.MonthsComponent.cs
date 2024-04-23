namespace TaskSched.Component.Cron
{
    /// <summary>
    /// work with the months component of the cron string
    /// </summary>
    public class MonthsComponent : CronComponentBase, ICronComponent
    {
        //TODO allow JAN, FEB, etc
        public MonthsComponent() :
            this("*")
        { }
        public MonthsComponent(string value)
            : base(value, 1, 12)
        {
            AllowedComponentTypes.Add(CronComponentType.AllowAny);
            AllowedComponentTypes.Add(CronComponentType.Repeating);
            AllowedComponentTypes.Add(CronComponentType.Range);
            InstanceChanged = false;

        }

        /// <summary>
        /// list of allowed values for text representation of months
        /// </summary>
        Dictionary<string, string> stringReplacements = new Dictionary<string, string>()
        {
            {"OCT", "10" },
            {"NOV", "11" },
            {"DEC", "12" },
            {"JAN", "1" },
            {"FEB", "2" },
            {"MAR", "3" },
            {"APR", "4" },
            {"MAY", "5" },
            {"JUN", "6" },
            {"JUL", "7" },
            {"AUG", "8" },
            {"SEP", "9" },
        };

        /// <summary>
        /// decode the incoming value
        /// </summary>
        /// <param name="value"></param>
        /// <remarks>
        /// months we need to check for and replace any three letter month abbreviations.
        /// </remarks>
        internal override void DecodeIncomingValue(string value)
        {
            value = value.ToUpper();

            //  could be numbers or strings
            //  lets turn the strings into numbers, if they exist
            foreach (var item in stringReplacements)
            {
                value = value.Replace(item.Key, item.Value);
            }

            base.DecodeIncomingValue(value);
        }

        /// <summary>
        /// check validity of a date against the current component information
        /// </summary>
        public override bool IsValid(DateTime currentDate)
        {
            switch (ComponentType)
            {
                case CronComponentType.AllowAny:
                    return true;
                default:

                    if (Range.Contains(currentDate.Month))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
        }

        public override string Text
        {
            get
            {
                switch (ComponentType)
                {
                    case CronComponentType.AllowAny:
                        return "";
                    case CronComponentType.Repeating:
                        return $"every {RepeatInterval} months starting at {RepeatStart}";
                    case CronComponentType.Range:
                        return $"at {string.Join(',', Range)} months";
                    default:
                        return "";
                }
            }
        }

        public override string ValueText
        {
            get
            {
                string valueString = Value;
                foreach (var replacement in stringReplacements)
                {
                    valueString = valueString.Replace(replacement.Value, replacement.Key);
                }

                return valueString;
            }
            set => base.ValueText = value;
        }


    }
}
