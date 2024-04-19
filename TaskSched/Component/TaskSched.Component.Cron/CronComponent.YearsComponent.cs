namespace TaskSched.Component.Cron
{
    /// <summary>
    /// work with the years component of a cron string
    /// </summary>
    public class YearsComponent : CronComponentBase, ICronComponent
    {
        public YearsComponent() :
            this("*")
        { }

        public YearsComponent(string value)
            : base(value, 1970, 2100)
        {
            AllowedComponentTypes.Add(CronComponentType.AllowAny);
            AllowedComponentTypes.Add(CronComponentType.Repeating);
            AllowedComponentTypes.Add(CronComponentType.Range);
            InstanceChanged = false;

        }

        /// <summary>
        /// check validity of a date against the current component information
        /// </summary>
        /// <param name="currentDate"></param>
        /// <returns></returns>
        public override bool IsValid(DateTime currentDate)
        {
            switch (ComponentType)
            {
                case CronComponentType.AllowAny:
                    return true;

                default:
                    if (Range.Contains(currentDate.Year))
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
                        return $"every {RepeatInterval} years starting at {RepeatStart}";
                    case CronComponentType.Range:
                        return $"at {string.Join(',', Range)} years";
                    default:
                        return "";
                }
            }
        }

    }
}
