namespace TaskSched.Component.Cron
{
    public class DaysOfWeekComponent : CronComponentBase, ICronComponent
    {
        //TODO allow MON,TUE, WED, etc
        //TODO allow 1-7 instead of 0-6

        public DaysOfWeekComponent() :
            this("*")
        { }
        public DaysOfWeekComponent(string value)
            : base(value, 1, 7)
        {
        }

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

        internal override void DecodeIncomingValue(string value)
        {
            //  could be numbers or strings
            //  lets turn the strings into numbers, if they exist
            foreach(var item in stringReplacements)
            {
                value = value.Replace(item.Key, item.Value);
            }
            //TODO need to handle # stuff

            if (value.Contains("?"))
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
                    Range.Add(7);
                }
                else
                {
                    //  it's the last N day of the month
                    Range.Add(int.Parse(value.Substring(0, value.Length - 1)));
                }

                ComponentType = CronComponentType.DaysOfWeekFromLast;

            }
            else if (value.Contains('#'))
            {
                //  handling the Nth weekday of the month
                //  <day of week>#<iteration>

                string[] parts = value.Split('#');
                DayOfWeek = int.Parse(parts[0]);
                WeekOfMonth = int.Parse(parts[1]);
                ComponentType = CronComponentType.NthWeekday;


            }
            else
            {
                //  it's normal stuff
                base.DecodeIncomingValue(value);
            }
        }

        public int DayOfWeek { get; set; }
        public int WeekOfMonth { get; set; }


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
                        while (((int)lastDay.DayOfWeek + 1) != Range[0])
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
                        if (Range.Contains(((int)currentDate.DayOfWeek) + 1))
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
                        if (Range.Count == 0)
                        {
                            return "L";
                        }
                        else if (Range[0] == 7)
                        {
                            return "L";
                        }
                        else
                        {
                            return $"{Range[0]}L";
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
