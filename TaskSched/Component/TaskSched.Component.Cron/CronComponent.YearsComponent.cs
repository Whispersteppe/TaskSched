namespace TaskSched.Component.Cron
{
    public class YearsComponent : CronComponentBase, ICronComponent
    {
        public YearsComponent() :
            this("*")
        { }
        public YearsComponent(string value)
            : base(value)
        {
            AllowedRangeValues.Clear();
            for (int i = 1970; i < 2100; i++)
            {
                AllowedRangeValues.Add(i);
            }

        }

        public override bool IsValid(DateTime currentDate)
        {
            if (Range.Contains(currentDate.Day))
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
