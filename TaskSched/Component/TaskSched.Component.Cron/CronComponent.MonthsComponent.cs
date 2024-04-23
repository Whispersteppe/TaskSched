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

        ///// <summary>
        ///// list of allowed values for text representation of months
        ///// </summary>
        //Dictionary<string, string> stringReplacements = new Dictionary<string, string>()
        //{
        //    {"OCT", "10" },
        //    {"NOV", "11" },
        //    {"DEC", "12" },
        //    {"JAN", "1" },
        //    {"FEB", "2" },
        //    {"MAR", "3" },
        //    {"APR", "4" },
        //    {"MAY", "5" },
        //    {"JUN", "6" },
        //    {"JUL", "7" },
        //    {"AUG", "8" },
        //    {"SEP", "9" },
        //};

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
            foreach (var item in replacements)
            {
                value = value.Replace(item.ShortName, item.ID);
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


        internal class MonthConversion
        {
            public string ShortName { get; set; }
            public string Name { get; set; }
            public string ID
            {
                get => IDInt.ToString();
            }
            public int IDInt { get; set; }
        }
        List<MonthConversion> replacements = new List<MonthConversion>()
        {
            new MonthConversion(){ IDInt = 11, Name = "November", ShortName = "NOV"},
            new MonthConversion(){ IDInt = 12, Name = "December", ShortName = "DEC"},
            new MonthConversion(){ IDInt = 1, Name = "January", ShortName = "JAN"},
            new MonthConversion(){ IDInt = 2, Name = "February", ShortName = "FEB"},
            new MonthConversion(){ IDInt = 3, Name = "March", ShortName = "MAR"},
            new MonthConversion(){ IDInt = 4, Name = "April", ShortName = "APR"},
            new MonthConversion(){ IDInt = 5, Name = "May", ShortName = "MAY"},
            new MonthConversion(){ IDInt = 6, Name = "June", ShortName = "JUN"},
            new MonthConversion(){ IDInt = 7, Name = "July", ShortName = "JUL"},
            new MonthConversion(){ IDInt = 8, Name = "August", ShortName = "AUG"},
            new MonthConversion(){ IDInt = 9, Name = "September", ShortName = "SEP"},
            new MonthConversion(){ IDInt = 10, Name = "October", ShortName = "OCT"},
        };


        private string ToMonthString(int day)
        {
            var replacement = replacements.FirstOrDefault(x => x.IDInt == day);
            return replacement?.Name ?? "";
        }

        private string ToMonthShortString(int day)
        {
            var replacement = replacements.FirstOrDefault(x => x.IDInt == day);
            return replacement?.ShortName ?? "";
        }

        public override string Text
        {
            get
            {
                switch (ComponentType)
                {
                    case CronComponentType.AllowAny:
                        return "every month";
                    case CronComponentType.Repeating:
                        return $"every {RepeatInterval} month starting on {ToMonthString(RepeatStart)}";
                    case CronComponentType.Range:
                        return $"on {string.Join(',', Range.Select(x => ToMonthString(x)).ToList())}";
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
                foreach (var replacement in replacements)
                {
                    valueString = valueString.Replace(replacement.ID, replacement.ShortName);
                }

                return valueString;
            }
            set => base.ValueText = value;
        }


    }
}
