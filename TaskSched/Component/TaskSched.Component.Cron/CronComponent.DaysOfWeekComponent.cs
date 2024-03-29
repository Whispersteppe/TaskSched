﻿namespace TaskSched.Component.Cron
{
    /// <summary>
    /// handles the day of week component
    /// </summary>
    public class DaysOfWeekComponent : CronComponentBase, ICronComponent
    {
        public int DayOfWeek { get; private set; }
        public int WeekOfMonth { get; private set; }


        public DaysOfWeekComponent() :
            this("*")
        { }
        public DaysOfWeekComponent(string value)
            : base(value, 1, 7)
        {
            AllowedComponentTypes.Add(CronComponentType.AllowAny);
            AllowedComponentTypes.Add(CronComponentType.Range);
            AllowedComponentTypes.Add(CronComponentType.DaysOfWeekFromLast);
            AllowedComponentTypes.Add(CronComponentType.Ignored);
            AllowedComponentTypes.Add(CronComponentType.NthWeekday);

        }

        /// <summary>
        /// list of allowed string values in the segment
        /// </summary>
        Dictionary<string, string> stringReplacements = new Dictionary<string, string>() 
        { 
            {"SUN", "1" },
            {"MON", "2" },
            {"TUE", "3" },
            {"WED", "4" },
            {"THU", "5" },
            {"FRI", "6" },
            {"SAT", "7" },
        };

        /// <summary>
        /// decode the incoming value
        /// </summary>
        /// <param name="value"></param>
        /// <remarks>
        /// we need to replace the string references with the numeric day of the week
        /// we then check for ?, L, and # and react accordingly.
        /// if we don't hit thoes items, we'll do it as a normal range/iteration
        /// </remarks>
        internal override void DecodeIncomingValue(string value)
        {
            value = value.ToUpper();

            //  could be numbers or strings
            //  lets turn the strings into numbers, if they exist
            foreach (var item in stringReplacements)
            {
                value = value.Replace(item.Key, item.Value);
            }
            //TODO need to handle # stuff

            if (value.Contains('?'))
            {
                ComponentType = CronComponentType.Ignored;
            }
            else if (value.EndsWith('L'))
            {

                //  forms we can find:
                //  L     - last day of week (saturday
                //  7L    - last saturday of month
                if (value == "L")
                {
                    SetLastWeekDayOfMonth(7);
                }
                else
                {
                    //  it's the last N day of the month
                    SetLastWeekDayOfMonth(int.Parse(value.Substring(0, value.Length - 1)));
                }
            }
            else if (value.Contains('#'))
            {
                //  handling the Nth weekday of the month
                //  <day of week>#<iteration>
                string[] parts = value.Split('#');
                SetNthWeekDayOfMonth(int.Parse(parts[0]), int.Parse(parts[1]));

            }
            else
            {
                //  it's normal stuff
                base.DecodeIncomingValue(value);
            }
        }

        public void SetLastWeekDayOfMonth(int dayOfWeek)
        {
            DayOfWeek = dayOfWeek;
            ComponentType = CronComponentType.DaysOfWeekFromLast;
        }

        public void SetNthWeekDayOfMonth(int dayOfWeek, int weekOfMonth)
        {
            DayOfWeek = dayOfWeek; 
            WeekOfMonth = weekOfMonth;
            ComponentType = CronComponentType.NthWeekday;

        }

        /// <summary>
        /// check validity of a date against this segment
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

                case CronComponentType.DaysOfWeekFromLast:
                    {
                        //int lastDay = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                        DateTime lastDay = currentDate.AddDays(DateTime.DaysInMonth(currentDate.Year, currentDate.Month) - currentDate.Day);
                        while (((int)lastDay.DayOfWeek + 1) != DayOfWeek)
                        {
                            lastDay = lastDay.AddDays(-1);
                        }
                        if (currentDate.Day == lastDay.Day)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                case CronComponentType.NthWeekday:
                    {
                        //  first the first of the current month
                        DateTime workDate = currentDate.AddDays(1 - currentDate.Day);
                        //  find the first Day in the range
                        while (((int)workDate.DayOfWeek + 1) != DayOfWeek)
                        {
                            workDate = workDate.AddDays(1);
                        }
                        //  now find the Nth DayOfWeek
                        int n = 1;
                        while (n < WeekOfMonth)
                        {
                            workDate = workDate.AddDays(7);
                            n++;
                        }

                        if (workDate.Day == currentDate.Day)
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
                        if (_rangeValues.Contains(((int)currentDate.DayOfWeek) + 1))
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

        /// <summary>
        /// get the segment
        /// </summary>
        /// <returns></returns>
        public override string GetPiece()
        {
            switch (ComponentType)
            {
                case CronComponentType.Ignored:
                    {
                        return "?";
                    }
                case CronComponentType.DaysOfWeekFromLast:
                    {
                        if (DayOfWeek == 7)
                        {
                            return "L";
                        }
                        else
                        {
                            return $"{DayOfWeek}L";
                        }
                    }
                case CronComponentType.NthWeekday:
                    {
                        return $"{DayOfWeek}#{WeekOfMonth}";
                    }
                default:
                    {
                        return base.GetPiece();
                    }
            }

        }

    }
}
