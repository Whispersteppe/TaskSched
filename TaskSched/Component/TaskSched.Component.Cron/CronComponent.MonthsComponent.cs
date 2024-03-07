namespace TaskSched.Component.Cron
{
    public class MonthsComponent : CronComponentBase, ICronComponent
    {
        //TODO allow JAN, FEB, etc
        public MonthsComponent() :
            this("*")
        { }
        public MonthsComponent(string value)
            : base(value, 1, 12)
        {
        }

        Dictionary<string, string> stringReplacements = new Dictionary<string, string>()
        {
            {"JAN", "1" },
            {"FEB", "2" },
            {"MAR", "3" },
            {"APR", "4" },
            {"MAY", "5" },
            {"JUN", "6" },
            {"JUL", "7" },
            {"AUG", "8" },
            {"SEP", "9" },
            {"OCT", "10" },
            {"NOV", "11" },
            {"DEC", "12" },
        };
        internal override void DecodeIncomingValue(string value)
        {
            //  could be numbers or strings
            //  lets turn the strings into numbers, if they exist
            foreach (var item in stringReplacements)
            {
                value = value.Replace(item.Key, item.Value);
            }

            base.DecodeIncomingValue(value);
        }

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


    }
}
