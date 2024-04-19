using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TaskSched.Component.Cron
{
    public class CronValue : INotifyPropertyChanged
    {
        #region Properties

        public SecondsComponent Seconds { get; private set; }
        public MinutesComponent Minutes { get; private set; }
        public HoursComponent Hours { get; private set; }
        public DaysOfWeekComponent DaysOfWeek { get; private set; }
        public DaysOfMonthComponent DaysOfMonth { get; private set; }
        public MonthsComponent Months { get; private set; }
        public YearsComponent Years { get; private set; }


        /// <summary>
        /// returns the entire cron string
        /// </summary>
        public string Value 
        {  get
            {
                List<string> valueParts =
                [
                    Seconds.GetPiece(),
                    Minutes.GetPiece(),
                    Hours.GetPiece(),
                    DaysOfMonth.GetPiece(),
                    Months.GetPiece(),
                    DaysOfWeek.GetPiece(),
                    Years.GetPiece(),
                ];

                string value = string.Join(' ', valueParts.ToArray());
                return value;
            }
        }

        #endregion

        /// <summary>
        /// primary constructor
        /// </summary>
        /// <param name="cronValue"></param>
        public CronValue(string cronValue) 
        {
            InitializeCronValue(cronValue);

        }

        #region Property Notify

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool InstanceChanged { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            InstanceChanged = true;
        }
        #endregion


        /// <summary>
        /// builds the cron value from the various pieces
        /// </summary>
        /// <param name="cronValue"></param>
        private void InitializeCronValue(string cronValue)
        {
            cronValue = cronValue.ToUpper();

            string[] cronParts = cronValue.Split(' ');

            Seconds = (cronParts.Length >= 1) ? new SecondsComponent(cronParts[0]) : new SecondsComponent();
            Minutes = (cronParts.Length >= 2) ? new MinutesComponent(cronParts[1]) : new MinutesComponent();
            Hours = (cronParts.Length >= 3) ?  new HoursComponent(cronParts[2]) : new HoursComponent();
            DaysOfMonth = (cronParts.Length >= 4) ? new DaysOfMonthComponent(cronParts[3]) : new DaysOfMonthComponent();
            Months = (cronParts.Length >= 5) ?  new MonthsComponent(cronParts[4]) : new MonthsComponent();
            DaysOfWeek = (cronParts.Length >= 6) ? new DaysOfWeekComponent(cronParts[5]) : new DaysOfWeekComponent();
            Years = (cronParts.Length >= 7) ? new YearsComponent(cronParts[6]) : new YearsComponent();

            Seconds.PropertyChanged += CronComponent_Changed;
            Minutes.PropertyChanged += CronComponent_Changed;
            Hours.PropertyChanged += CronComponent_Changed;
            DaysOfMonth.PropertyChanged += CronComponent_Changed;
            Months.PropertyChanged += CronComponent_Changed;
            DaysOfWeek.PropertyChanged += CronComponent_Changed;
            Years.PropertyChanged += CronComponent_Changed;

        }

        private void CronComponent_Changed(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Cron Component");
        }

        /// <summary>
        /// resets the cron string
        /// </summary>
        /// <param name="cronValue"></param>
        public void SetCronValue(string cronValue)
        {
            InitializeCronValue(cronValue);
        }

        /// <summary>
        /// checks to see if a date is valid against all the subcomponents
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsValid(DateTime time)
        {
            if (Years.IsValid(time) == false) return false;
            if (Months.IsValid(time) == false) return false;
            if (DaysOfMonth.IsValid(time) == false) return false;
            if (DaysOfWeek.IsValid(time) == false) return false;
            if (Hours.IsValid(time) == false) return false;
            if (Minutes.IsValid(time) == false) return false;
            if (Seconds.IsValid(time) == false) return false;

            return true;

        }

        /// <summary>
        /// returns the next time from now
        /// </summary>
        /// <returns></returns>
        public DateTime NextTime()
        {
            return NextTimes()[0];
        }

        /// <summary>
        /// returns the next time from a particular date
        /// </summary>
        /// <param name="fromTime"></param>
        /// <returns></returns>
        public DateTime NextTime(DateTime fromTime)
        {
            return NextTimes(fromTime)[0];
        }

        /// <summary>
        /// returns the next N times from now
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<DateTime> NextTimes(int count = 1)
        {
            return NextTimes(DateTime.Now, count);
        }

        /// <summary>
        /// returns the next N times from a certain date
        /// </summary>
        /// <param name="fromTime"></param>
        /// <param name="count"></param>
        /// <returns></returns>
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

        /// <summary>
        /// calculate the next time.
        /// </summary>
        /// <param name="fromTime"></param>
        /// <returns></returns>
        /// <remarks>
        /// given a date, we want to get the next date.  we'll check each segment in turn,
        /// and if it is not valid get the next valid one on that piece and continue on.  
        /// </remarks>
        /// 
        private DateTime InternalNextTime(DateTime fromTime)
        {
            DateTime workDate = fromTime;
            while (IsValid(workDate) == false)
            {

                // start high, go in
                if (Years.IsValid(workDate) == false)
                {
                    int nextYear = Years.GetNext(workDate.Year);
                    if (nextYear == -1)
                    {
                        // we're run off the end of the year component without hitting an event
                        return DateTime.MaxValue;
                    }
                    workDate = new DateTime(nextYear, 1, 1, 0, 0, 0);
                }
                else if (Months.IsValid(workDate) == false)
                {
                    int nextMonth = Months.GetNext(workDate.Month);
                    if (nextMonth == -1)
                    {
                        workDate = workDate.AddMonths(12 - workDate.Month + 1);
                        workDate = new DateTime(workDate.Year, workDate.Month, 1, 0, 0, 0);
                    }
                    else
                    {
                        workDate = new DateTime(workDate.Year, nextMonth, 1, 0, 0, 0);
                    }
                }
                else if (DaysOfMonth.IsValid(workDate) == false)
                {
                    int nextDay = DaysOfMonth.GetNext(workDate);
                    if (nextDay == -1)
                    {
                        workDate = workDate.AddDays(DateTime.DaysInMonth(workDate.Year, workDate.Month) - workDate.Day + 1);
                        workDate = new DateTime(workDate.Year, workDate.Month, 1, 0, 0, 0);
                    }
                    else
                    {
                        workDate = new DateTime(workDate.Year, workDate.Month, nextDay, 0, 0, 0);
                    }
                }
                else if (DaysOfWeek.IsValid(workDate) == false)
                {
                    while (DaysOfWeek.IsValid(workDate) == false)
                    {
                        workDate = workDate.AddDays(1);
                    }

                    //  and reset the time
                    workDate = workDate.AddHours(-workDate.Hour);
                    workDate = workDate.AddMinutes(-workDate.Minute);
                    workDate = workDate.AddSeconds(-workDate.Second);
                }
                else if (Hours.IsValid(workDate) == false)
                {
                    int nextHour = Hours.GetNext(workDate.Hour);
                    if (nextHour == -1)
                    {
                        workDate = workDate.AddHours(24 - workDate.Hour);
                        workDate = new DateTime(workDate.Year, workDate.Month, workDate.Day, 0, 0, 0);
                    }
                    else
                    {
                        workDate = new DateTime(workDate.Year, workDate.Month, workDate.Day, nextHour, 0, 0);
                    }
                }
                else if (Minutes.IsValid(workDate) == false)
                {
                    int nextMinute = Minutes.GetNext(workDate.Minute);
                    if (nextMinute == -1)
                    {
                        workDate = workDate.AddMinutes(60 - workDate.Minute);
                        workDate = new DateTime(workDate.Year, workDate.Month, workDate.Day, workDate.Hour, 0, 0);
                    }
                    else
                    {
                        workDate = new DateTime(workDate.Year, workDate.Month, workDate.Day, workDate.Hour, nextMinute, 0);
                    }
                }
                else if (Seconds.IsValid(workDate) == false)
                {
                    int nextSecond = Seconds.GetNext(workDate.Second);
                    if (nextSecond == -1)
                    {
                        workDate = workDate.AddSeconds(60 - workDate.Second);
                        workDate = new DateTime(workDate.Year, workDate.Month, workDate.Day, workDate.Hour, workDate.Minute, 0);
                    }
                    else
                    {
                        workDate = new DateTime(workDate.Year, workDate.Month, workDate.Day, workDate.Hour, workDate.Minute, nextSecond);
                    }

                }
                else
                {
                    //  it's completely valid. we'll exit shortly
                }
            }

            return workDate;

        }
      


    }


}
