
namespace TaskSched.Component.Cron
{
    public class SecondsComponent : CronComponentBase, ICronComponent
    {
        public SecondsComponent() :
            this("*")
        { }
        public SecondsComponent(string value)
            : base(value)
        {
            AllowedRangeValues.Clear();
            for(int i = 0; i < 60; i++)
            {
                AllowedRangeValues.Add(i);
            }

        }


        public override bool IsValid(DateTime currentDate)
        {
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
