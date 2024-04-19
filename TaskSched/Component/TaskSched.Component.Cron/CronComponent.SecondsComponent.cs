
namespace TaskSched.Component.Cron
{
    /// <summary>
    /// handle working with the seconds component
    /// </summary>
    public class SecondsComponent : CronComponentBase, ICronComponent
    {
        public SecondsComponent() :
            this("*")
        { }
        public SecondsComponent(string value)
            : base(value, 0, 59)
        {
            AllowedComponentTypes.Add(CronComponentType.AllowAny);
            AllowedComponentTypes.Add(CronComponentType.Repeating);
            AllowedComponentTypes.Add(CronComponentType.Range);

            InstanceChanged = false;
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

                    if (Range.Contains(currentDate.Second))
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
                switch(ComponentType)
                {
                    case CronComponentType.AllowAny:
                        return "";
                    case CronComponentType.Repeating:
                        return $"every {RepeatInterval} seconds starting at {RepeatStart}";
                    case CronComponentType.Range:
                        return $"at {string.Join(',', Range)} seconds";
                    default:
                        return "";
                }
            }
        }


    }
}
