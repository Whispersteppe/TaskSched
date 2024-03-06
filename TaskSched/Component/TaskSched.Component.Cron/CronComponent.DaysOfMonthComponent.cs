namespace TaskSched.Component.Cron
{
    public class DaysOfMonthComponent : CronComponentBase, ICronComponent
    {
        public DaysOfMonthComponent() :
            this("*")
        { }
        public DaysOfMonthComponent(string value)
            : base(value)
        {
            AllowedRangeValues.Clear();
            for (int i = 1; i < 31; i++)
            {
                AllowedRangeValues.Add(i);
            }

        }

        public bool FromLast { get; set; } = false;
        public bool Ignored { get; set; } = false;

        internal override void DecodeIncomingValue(string value)
        {

            if (value.Contains('L'))
            {
                FromLast = true;
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
                    Range.Add(int.Parse(value.Substring(1)));
                }
                else
                {
                    throw new Exception($"Day of month is invalid - {value}");
                }
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
        public override int GetNext(int startAt = -1)
        {
            //TODO we need to pass in a datetime, and return a datetime....
            //  RESWIZZLE TIME

            return base.GetNext(startAt);
        }

        public override void SetAllowAny()
        {
            FromLast = false;
            Ignored = false;

            base.SetAllowAny();
        }

        public override string GetPiece()
        {
            if (FromLast == true)
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
                        return "L-{Range[0]}";
                    }
                }
            }
            else if (Ignored == true)
            {
                return "?";
            }
            else
            {
                return base.GetPiece();
            }
        }

        public override bool IsValid(DateTime currentDate)
        {
            if (Ignored == true)
            {
                return true;
            }
            else if (FromLast == true)
            {
                DateTime workDate = currentDate.AddDays(DateTime.DaysInMonth(currentDate.Year, currentDate.Month) - currentDate.Day - Range[0]);
                if (workDate.Day == currentDate.Day)
                {
                    return true;
                }
                else
                { 
                    return false; 
                }
            }
            else if (Range.Contains(currentDate.Day))
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
