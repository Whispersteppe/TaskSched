
namespace TaskSched.Component.Cron
{
    public class SecondsComponent : CronComponentBase, ICronComponent
    {
        public SecondsComponent() :
            this("*")
        { }
        public SecondsComponent(string value)
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


    }
}
