namespace TaskSched.Component.Cron
{
    /// <summary>
    /// works with the hours component of the cron string
    /// </summary>
    public class HoursComponent : CronComponentBase, ICronComponent
    {
        public HoursComponent() :
            this("*")
        { }
        public HoursComponent(string value)
            : base(value, 0, 23)
        {
            AllowedComponentTypes.Add(CronComponentType.AllowAny);
            AllowedComponentTypes.Add(CronComponentType.Repeating);
            AllowedComponentTypes.Add(CronComponentType.Range);

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

                    if (Range.Contains(currentDate.Hour))
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
