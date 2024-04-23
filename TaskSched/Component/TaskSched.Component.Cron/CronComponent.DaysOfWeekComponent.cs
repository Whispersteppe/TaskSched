using System.Runtime.InteropServices;

namespace TaskSched.Component.Cron
{
    /// <summary>
    /// handles the day of week component
    /// </summary>
    public class DaysOfWeekComponent : CronComponentBase, ICronComponent
    {



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
            InstanceChanged = false;

        }

        ///// <summary>
        ///// list of allowed string values in the segment
        ///// </summary>
        //Dictionary<string, string> stringReplacements = new Dictionary<string, string>() 
        //{ 
        //    {"SUN", "1" },
        //    {"MON", "2" },
        //    {"TUE", "3" },
        //    {"WED", "4" },
        //    {"THU", "5" },
        //    {"FRI", "6" },
        //    {"SAT", "7" },
        //};
        private int _dayOfWeek;
        private int _weekOfMonth;

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
            foreach (var item in replacements)
            {
                value = value.Replace(item.ShortName, item.ID);
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
        public int DayOfWeek
        {
            get => _dayOfWeek;

            set
            {
                _dayOfWeek = value;
                ComponentType = CronComponentType.DaysOfWeekFromLast;
                OnPropertyChanged();
            }
        }

        public int WeekOfMonth 
        { get => _weekOfMonth;
            set
            {
                _weekOfMonth = value;
                ComponentType = CronComponentType.NthWeekday;
                OnPropertyChanged();
            }
        }

        public void SetLastWeekDayOfMonth(int dayOfWeek)
        {
            _dayOfWeek = dayOfWeek;
            ComponentType = CronComponentType.DaysOfWeekFromLast;
            OnPropertyChanged("DayOfWeek");
        }

        public void SetNthWeekDayOfMonth(int dayOfWeek, int weekOfMonth)
        {
            _dayOfWeek = dayOfWeek; 
            _weekOfMonth = weekOfMonth;
            ComponentType = CronComponentType.NthWeekday;
            OnPropertyChanged("DayOfWeek");
            OnPropertyChanged("WeekOfMonth");
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

        internal class DayOfWeekConversion
        {
            public string ShortName { get; set; }
            public string Name { get; set; }
            public string ID
            {
                get => IDInt.ToString();
            }
            public int IDInt { get; set; }
        }
        List<DayOfWeekConversion> replacements = new List<DayOfWeekConversion>()
        {
            new DayOfWeekConversion(){ IDInt = 1, Name = "Sunday", ShortName = "SUN"},
            new DayOfWeekConversion(){ IDInt = 2, Name = "Monday", ShortName = "MON"},
            new DayOfWeekConversion(){ IDInt = 3, Name = "Tuesday", ShortName = "TUE"},
            new DayOfWeekConversion(){ IDInt = 4, Name = "Wednesday", ShortName = "WED"},
            new DayOfWeekConversion(){ IDInt = 5, Name = "Thursday", ShortName = "THU"},
            new DayOfWeekConversion(){ IDInt = 6, Name = "Friday", ShortName = "FRI"},
            new DayOfWeekConversion(){ IDInt = 7, Name = "Saturday", ShortName = "SAT"},
        };


        private string ToDayOfWeekString(int day)
        {
            var replacement = replacements.FirstOrDefault(x=>x.IDInt == day);
            return replacement?.Name ?? "";
        }

        private string ToDayOfWeekShortString(int day)
        {
            var replacement = replacements.FirstOrDefault(x => x.IDInt == day);
            return replacement?.ShortName ?? "";
        }

        public override string Text
        {
            get
            {
                switch (ComponentType)
                {
                    case CronComponentType.AllowAny:
                        return "every day";
                    case CronComponentType.Repeating:
                        return $"every {RepeatInterval} day starting on {ToDayOfWeekString(RepeatStart)}";
                    case CronComponentType.Range:
                        return $"on {string.Join(',', Range.Select(x=> ToDayOfWeekString(x)).ToList())}";
                    case CronComponentType.DaysOfWeekFromLast:
                        return $"the last {ToDayOfWeekString(DayOfWeek)} of the month";
                    case CronComponentType.NthWeekday:
                        return $"the {WeekOfMonth} {ToDayOfWeekString(DayOfWeek)}";
                    default:
                        return "";
                }
            }
        }

        public override string ValueText 
        { 
            get 
            {
                string valueString = Value;
                foreach(var replacement in replacements)
                {
                    valueString = valueString.Replace(replacement.ID, replacement.ShortName);
                }

                return valueString;
            }
            set => base.ValueText = value; 
        }

    }
}
