namespace TaskSched.Component.Cron
{
    public class YearsComponent : CronComponentBase, ICronComponent
    {
        public YearsComponent() :
            this("*")
        { }
        public YearsComponent(string value)
            : base(value, 1970, 2100)
        {
        }

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

    }
}
