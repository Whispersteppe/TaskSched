using System.Text.Json.Nodes;

namespace TaskSched.Component.Cron
{
    /// <summary>
    /// manages days of the month
    /// </summary>
    public class DaysOfMonthComponent : CronComponentBase, ICronComponent
    {
        public DaysOfMonthComponent() :
            this("*")
        { }
        public DaysOfMonthComponent(string value)
            : base(value, 0, 31)
        {
            AllowedComponentTypes.Add(CronComponentType.AllowAny);
            AllowedComponentTypes.Add(CronComponentType.Repeating);
            AllowedComponentTypes.Add(CronComponentType.Range);
            AllowedComponentTypes.Add(CronComponentType.DaysOfMonthFromLast);
            AllowedComponentTypes.Add(CronComponentType.Ignored);
            InstanceChanged = false;

        }

        /// <summary>
        /// decodes the incoming string
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="Exception"></exception>
        /// <remarks>
        /// we need to check for '?' and 'L' values and react accordingly.  if we don't see those, we go to the base class
        /// </remarks>
        internal override void DecodeIncomingValue(string value)
        {
            value = value.ToUpper();

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
                    
                    _rangeValues.Clear();
                    _rangeValues.Add(1);
                }
                else if (value.StartsWith('L')) //  L-3 version
                {
                    _rangeValues.Add(int.Parse(value.Substring(2))); //  skip the L and -
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
        /// We need to get the next day of the month.
        /// </summary>
        /// <param name="startAt"></param>
        /// <returns></returns>
        /// <remarks>
        /// we need the entire date in this case, because the days in each month are different, 
        /// like everything else, if we exceed the topmost allowed value, we return a -1.
        /// </remarks>
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
                    {
                        //  we need to check against the end of the month, so that we don't return February 30th
                        int nextDay = base.GetNext(fromTime.Day);
                        if (nextDay > DateTime.DaysInMonth(fromTime.Year, fromTime.Month))
                        {
                            return -1; 
                        }
                        else
                        {
                            return nextDay;
                        }
                    }
            }
        }

        /// <summary>
        /// Get the cron string piece associated with the days of month
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// verify that the given date is valid against this component
        /// </summary>
        /// <param name="currentDate"></param>
        /// <returns></returns>
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

        public override string Text
        {
            get
            {
                switch (ComponentType)
                {
                    case CronComponentType.AllowAny:
                        return "";
                    case CronComponentType.Repeating:
                        return $"every {RepeatInterval} seconds starting at {RepeatStart}";
                    case CronComponentType.Range:
                        return $"at {string.Join(',', Range)} seconds";
                    case CronComponentType.DaysOfMonthFromLast:
                        return $"the {Range[0]} from the end of the month";
                    default:
                        return "";
                }
            }
        }
    }
}
