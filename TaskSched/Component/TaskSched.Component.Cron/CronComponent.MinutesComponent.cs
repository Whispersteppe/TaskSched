namespace TaskSched.Component.Cron
{
    public class MinutesComponent : CronComponentBase, ICronComponent
    {
        public MinutesComponent() :
            this("*")
        { }
        public MinutesComponent(string value)
            : base(value)
        {
            AllowedRangeValues.Clear();
            for (int i = 0; i < 60; i++)
            {
                AllowedRangeValues.Add(i);
            }

        }

        public override bool IsValid(DateTime currentDate)
        {
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
