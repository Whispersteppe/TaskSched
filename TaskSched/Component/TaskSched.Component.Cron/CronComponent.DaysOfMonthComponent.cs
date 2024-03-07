namespace TaskSched.Component.Cron
{
    public class DaysOfMonthComponent : CronComponentBase, ICronComponent
    {
        public DaysOfMonthComponent() :
            this("*")
        { }
        public DaysOfMonthComponent(string value)
            : base(value, 0, 31)
        {
        }

        internal override void DecodeIncomingValue(string value)
        {
            if (value.Contains("?"))
            {
                ComponentType = CronComponentType.Ignored;
            }
            else if (value.Contains('L'))
            {

                //  forms we can find:
                //  L     - last day of month
                //  3L    - last day of week of month
                //  L-3   - third last day of month
                if (value.Length ==  1) // L
                {
                    Range.Clear();
                    Range.Add(1);
                }
                else if (value.StartsWith('L')) //  L-3 version
                {
                    Range.Add(int.Parse(value.Substring(2))); //  skip the L and -
                }
                else
                {
                    throw new Exception($"Day of month is invalid - {value}");
                }

                ComponentType = CronComponentType.DaysOfMonthFromLast;

            }
            else
            {
                //  it's normal stuff
                base.DecodeIncomingValue(value);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startAt"></param>
        /// <returns></returns>
        public int GetNext(DateTime fromTime)
        {
            switch(ComponentType)
            {
                case CronComponentType.DaysOfMonthFromLast:
                    {
                        int lastDayOfMonth = DateTime.DaysInMonth(fromTime.Year, fromTime.Month) - Range[0] + 1;
                        if (fromTime.Day >= lastDayOfMonth)
                        {
                            return -1;
                        }
                        else
                        {
                            return lastDayOfMonth;
                        }
                    }
                default:
                    return base.GetNext(fromTime.Day);
            }
        }

        public override void SetAllowAny()
        {

            base.SetAllowAny();
        }

        public override string GetPiece()
        {
            switch (ComponentType)
            {
                case CronComponentType.DaysOfMonthFromLast:
                    {
                        if (Range.Count == 0)
                        {
                            return "L";
                        }
                        else
                        {
                            if (Range[0] == 1)
                            {
                                return "L";
                            }
                            else
                            {
                                return $"L-{Range[0]}";
                            }
                        }
                    }
                case CronComponentType.Ignored:
                    {
                        return "?";
                    }
                default:
                    {
                        return base.GetPiece();
                    }
            }
        }

        public override bool IsValid(DateTime currentDate)
        {
            switch (ComponentType)
            {
                case CronComponentType.Ignored:
                    {
                        return true;
                    }
                case CronComponentType.AllowAny:
                    {
                        return true;
                    }

                case CronComponentType.DaysOfMonthFromLast:
                    {

                        int date = DateTime.DaysInMonth(currentDate.Year, currentDate.Month) - Range[0] + 1;
                        //DateTime workDate =  currentDate.AddDays(DateTime.DaysInMonth(currentDate.Year, currentDate.Month) - Range[0]);
                        if (currentDate.Day == date)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                default:
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
    }
}
