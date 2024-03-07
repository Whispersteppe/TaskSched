namespace TaskSched.Component.Cron
{
    public class MinutesComponent : CronComponentBase, ICronComponent
    {
        public MinutesComponent() :
            this("*")
        { }
        public MinutesComponent(string value)
            : base(value, 0, 59)
        {
        }

        public override bool IsValid(DateTime currentDate)
        {
            switch (ComponentType)
            {
                case CronComponentType.AllowAny:
                    return true;
                default:

                    if (Range.Contains(currentDate.Minute))
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
