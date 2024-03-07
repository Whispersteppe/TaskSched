using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TaskSched.Component.Cron
{
    public class CronValue
    {
        #region Properties

        public SecondsComponent secondsComponent { get; private set; }
        public MinutesComponent minutesComponent { get; private set; }
        public HoursComponent hoursComponent { get; private set; }
        public DaysOfMonthComponent daysOfMonthComponent { get; private set; }
        public YearsComponent yearsComponent { get; private set; }
        public MonthsComponent monthsComponent { get; private set; }
        public DaysOfWeekComponent daysOfWeekComponent { get; private set; }

        public string Value 
        {  get
            {
                List<string> valueParts =
                [
                    secondsComponent.GetPiece(),
                    minutesComponent.GetPiece(),
                    hoursComponent.GetPiece(),
                    daysOfMonthComponent.GetPiece(),
                    monthsComponent.GetPiece(),
                    daysOfWeekComponent.GetPiece(),
                    yearsComponent.GetPiece(),
                ];

                string value = string.Join(' ', valueParts.ToArray());
                return value;
            }
        }

        #endregion

        public CronValue(string cronValue) 
        {
            InitializeCronValue(cronValue);

        }

        private void InitializeCronValue(string cronValue)
        {
            string[] cronParts = cronValue.Split(' ');
            secondsComponent = new SecondsComponent(cronParts[0]);
            minutesComponent = new MinutesComponent(cronParts[1]);
            hoursComponent = new HoursComponent(cronParts[2]);
            daysOfMonthComponent = new DaysOfMonthComponent(cronParts[3]);
            monthsComponent = new MonthsComponent(cronParts[4]);
            daysOfWeekComponent = new DaysOfWeekComponent(cronParts[5]);
            yearsComponent = new YearsComponent(cronParts[6]);

        }

        public void SetCronValue(string cronValue)
        {
            InitializeCronValue(cronValue);
        }


        public bool IsValid(DateTime time)
        {
            if (yearsComponent.IsValid(time) == false) return false;
            if (monthsComponent.IsValid(time) == false) return false;
            if (daysOfMonthComponent.IsValid(time) == false) return false;
            if (daysOfWeekComponent.IsValid(time) == false) return false;
            if (hoursComponent.IsValid(time) == false) return false;
            if (minutesComponent.IsValid(time) == false) return false;
            if (secondsComponent.IsValid(time) == false) return false;

            return true;

        }

        public DateTime NextTime()
        {
            return NextTimes()[0];
        }

        public DateTime NextTime(DateTime fromTime)
        {
            return NextTimes(fromTime)[0];
        }

        public List<DateTime> NextTimes(int count = 1)
        {
            return NextTimes(DateTime.Now, count);
        }
        public List<DateTime> NextTimes(DateTime fromTime, int count = 1)
        {
            DateTime currentTime = fromTime;

            List<DateTime> times = new List<DateTime>();
            for(int i = 0; i < count;i++)
            {
                if (IsValid(currentTime))
                {
                    currentTime = currentTime.AddSeconds(1);
                }
                DateTime nextTime = InternalNextTime(currentTime);
                times.Add(nextTime);
                currentTime = nextTime;
            }

            return times;

        }

        private DateTime InternalNextTime(DateTime fromTime)
        {
            DateTime workDate;

            // start high, go in
            if (yearsComponent.IsValid(fromTime) == false)
            {
                int nextYear = yearsComponent.GetNext(fromTime.Year);
                if (nextYear == -1)
                {
                    // we're run off the end of the year component without hitting an event
                    return DateTime.MaxValue;
                }
                workDate = new DateTime(nextYear, 1, 1, 0, 0, 0);
            }
            else if (monthsComponent.IsValid(fromTime) == false)
            {
                int nextMonth = monthsComponent.GetNext(fromTime.Month);
                if (nextMonth == -1)
                {
                    fromTime = fromTime.AddMonths(12 - fromTime.Month + 1);
                    workDate = new DateTime(fromTime.Year, fromTime.Month, 1, 0, 0, 0);
                }
                else
                {
                    workDate = new DateTime(fromTime.Year, nextMonth, 1, 0, 0, 0);
                }
            }
            else if (daysOfMonthComponent.IsValid(fromTime) == false)
            {
                int nextDay = daysOfMonthComponent.GetNext(fromTime);
                if (nextDay == -1)
                {
                    fromTime = fromTime.AddDays(DateTime.DaysInMonth(fromTime.Year, fromTime.Month) - fromTime.Day + 1);
                    workDate = new DateTime(fromTime.Year, fromTime.Month, 1, 0, 0, 0);
                }
                else
                {
                    workDate = new DateTime(fromTime.Year, fromTime.Month, nextDay, 0, 0, 0);
                }
            }
            else if (daysOfWeekComponent.IsValid(fromTime) == false)
            {
                workDate = fromTime;
                while (daysOfWeekComponent.IsValid(workDate) == false)
                {
                    workDate = workDate.AddDays(1);
                }

                //  and reset the time
                workDate = workDate.AddHours(-workDate.Hour);
                workDate = workDate.AddMinutes(-workDate.Minute);
                workDate = workDate.AddSeconds(-workDate.Second);
            }
            else if (hoursComponent.IsValid(fromTime) == false)
            {
                int nextHour = hoursComponent.GetNext(fromTime.Hour);
                if (nextHour == -1)
                {
                    fromTime = fromTime.AddHours(24 - fromTime.Hour);
                    workDate = new DateTime(fromTime.Year, fromTime.Month, fromTime.Day, 0, 0, 0);
                }
                else
                {
                    workDate = new DateTime(fromTime.Year, fromTime.Month, fromTime.Day, nextHour, 0, 0);
                }
            }
            else if (minutesComponent.IsValid(fromTime) == false)
            {
                int nextMinute = minutesComponent.GetNext(fromTime.Minute);
                if (nextMinute == -1)
                {
                    fromTime = fromTime.AddMinutes(60 - fromTime.Minute);
                    workDate = new DateTime(fromTime.Year, fromTime.Month, fromTime.Day, fromTime.Hour, 0, 0);
                }
                else
                {
                    workDate = new DateTime(fromTime.Year, fromTime.Month, fromTime.Day, fromTime.Hour, nextMinute, 0);
                }
            }
            else if (secondsComponent.IsValid(fromTime) == false)
            {
                int nextSecond = secondsComponent.GetNext(fromTime.Second);
                if (nextSecond == -1)
                {
                    fromTime = fromTime.AddSeconds(60 - fromTime.Second);
                    workDate = new DateTime(fromTime.Year, fromTime.Month, fromTime.Day, fromTime.Hour, fromTime.Minute, 0);
                }
                else
                {
                    workDate = new DateTime(fromTime.Year, fromTime.Month, fromTime.Day, fromTime.Hour, fromTime.Minute, nextSecond);
                }

            }
            else
            {
                //  it's completely valid.  return it;
                return fromTime;
            }

            return InternalNextTime(workDate);

        }



    }
}
