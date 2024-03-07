namespace TaskSched.Component.Cron
{
    public class HoursComponent : CronComponentBase, ICronComponent
    {
        public HoursComponent() :
            this("*")
        { }
        public HoursComponent(string value)
            : base(value, 0, 23)
        {
        }

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
