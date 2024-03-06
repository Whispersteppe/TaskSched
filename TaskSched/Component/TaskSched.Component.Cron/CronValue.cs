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

        public string Value { get; private set; }
        public string Seconds { get; private set; }
        public string Minutes { get; private set; }
        public string Hours { get; private set; }
        public string DaysOfWeek { get; private set; }
        public string Months { get; private set; }
        public string DaysOfMonth {  get; private set; }
        public string Years {  get; private set; }

        SecondsComponent secondsComponent;
        MinutesComponent minutesComponent;
        HoursComponent hoursComponent;
        DaysOfMonthComponent daysOfMonthComponent;
        YearsComponent yearsComponent;
        MonthsComponent monthsComponent;
        DaysOfWeekComponent daysOfWeekComponent;

        #endregion

        public CronValue(string cronValue) 
        {
            Value = cronValue;

            //  TODO may want to make sure there are enough parts here
            string[] cronParts = cronValue.Split(' ');
            secondsComponent = new SecondsComponent(cronParts[0]);
            minutesComponent = new MinutesComponent(cronParts[1]);
            hoursComponent = new HoursComponent(cronParts[2]);
            daysOfMonthComponent = new DaysOfMonthComponent(cronParts[3]);
            monthsComponent = new MonthsComponent(cronParts[4]);
            daysOfWeekComponent = new DaysOfWeekComponent(cronParts[5]);
            yearsComponent = new YearsComponent(cronParts[6]);


        }


        public DateTime NextTime()
        {
            return NextTime(DateTime.Now);
        }

        public DateTime NextTime(DateTime fromTime)
        {
            DateTime workDate;

            // start high, go in
            if (yearsComponent.IsValid(fromTime) == false)
            {
                int nextYear = yearsComponent.GetNext(fromTime.Year);
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
                int nextDay = daysOfMonthComponent.GetNext(fromTime.Day);
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
                    workDate.AddDays(1);
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
                    workDate = new DateTime(fromTime.Year, fromTime.Month, fromTime.Day, fromTime.Hour, fromTime.Second, nextSecond);
                }

            }
            else
            {
                //  it's completely valid.  return it;
                return fromTime;
            }

            return NextTime(workDate);

        }

        public List<DateTime> NextTimes(int count = 1) 
        {
            return [];
        }
        public List<DateTime> NextTimes(DateTime fromTime, int count = 1)
        {
            return [];
        }

    }
}
