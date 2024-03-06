namespace TaskSched.Component.Cron
{
    public class HoursComponent : CronComponentBase, ICronComponent
    {
        public HoursComponent() :
            this("*")
        { }
        public HoursComponent(string value)
            : base(value)
        {
            AllowedRangeValues.Clear();
            for (int i = 0; i < 24; i++)
            {
                AllowedRangeValues.Add(i);
            }

        }

        public override bool IsValid(DateTime currentDate)
        {
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
