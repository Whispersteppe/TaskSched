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
            : base(value)
        {
            AllowedRangeValues.Clear();
            for (int i = 1; i <= 7; i++)
            {
                AllowedRangeValues.Add(i);
            }

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

            if (value.EndsWith('L'))
            {
                FromLast = true;
                //  forms we can find:
                //  L     - last day of week (saturday
                //  3L    - last tuesday of month
                if (value == "L")
                {
                    FromLast = false;
                    value = "7";
                }
                else
                {
                    //  it's the last N day of the month
                    FromLast = true;
                    value = value.Substring(0, value.Length - 1);
                }


            }
            else if (value.Contains('#'))
            {
                //  handling the Nth weekday of the month

                string[] parts = value.Split('#');
                AllowedRangeValues.Add(int.Parse(parts[0]));
                NthWeekday = int.Parse(parts[1]);

            }
            else
            {
                //  it's normal stuff
                base.DecodeIncomingValue(value);
            }



            base.DecodeIncomingValue(value);
        }

        public int NthWeekday { get; set; } = -1;

        public bool FromLast { get; set; } = true;
        public bool Ignored { get; set; } = true;


        public override bool IsValid(DateTime currentDate)
        {
            if (Ignored == true)
            {
                return true;
            }
            else if (FromLast == true)
            {
                //  it's a last N day of the month situation
                if (Range.Contains(currentDate.Day))
                {
                    if (DateTime.DaysInMonth(currentDate.Year, currentDate.Month) - currentDate.Day < 7)
                    {
                        return true;
                    }
                    else 
                    { 
                        return false; 
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (NthWeekday > -1)
            {
                //  first the first of the current month
                DateTime workDate = currentDate.AddDays(1 - currentDate.Day);
                //  find the first Day in the range
                while ((int)workDate.DayOfWeek != Range[0])
                {
                    workDate = workDate.AddDays(1);
                }
                //  now find the Nth DayOfWeek
                int n = 1;
                while (n < NthWeekday)
                {
                    workDate = workDate.AddDays(7);
                    n++;
                }

                if (workDate.Day ==  currentDate.Day)
                {
                    return true;
                }
                else 
                { 
                    return false; 
                }

            }
            else
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
