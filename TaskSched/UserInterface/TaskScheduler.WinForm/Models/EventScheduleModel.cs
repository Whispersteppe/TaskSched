using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using TaskSched.Component.Cron;

namespace TaskScheduler.WinForm.Models
{
    public class EventScheduleModel: INotifyPropertyChanged
    {
        private string name;

        CronValue cronValue = new CronValue("0 0 8 * * ? *");

        [ReadOnly(true)]
        [Browsable(false)]
        [Category("ID")]

        public Guid Id { get; set; }
        [ReadOnly(false)]
        [Browsable(true)]
        [Description("The name of the schedule")]
        [Category("ID")]

        public string Name 
        { 
            get => name;
            set
            {
                name = value;OnPropertyChanged();
            }
        }
        [ReadOnly(false)]
        [Browsable(true)]
        [Description("Cron data for the schedule - Sec Min Hour DayOfMonth Month DayOfWeek Year")]
        [Category("ID")]
        public string CRONData 
        {
            get
            {
                return cronValue.Value;
            }
            set
            {
                cronValue.SetCronValue(value);
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [ReadOnly(true)]
        [Browsable(false)]
        public bool InstanceChanged { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            InstanceChanged = true;
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("CRON")]
        public SecondsComponent Seconds => cronValue.Seconds;
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("CRON")]
        public MinutesComponent Minutes => cronValue.Minutes;
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("CRON")]
        public HoursComponent Hours => cronValue.Hours;
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("CRON")]
        [DisplayName("Days of Month")]
        public DaysOfMonthComponent DaysOfMonth => cronValue.DaysOfMonth;
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("CRON")]
        public MonthsComponent Months => cronValue.Months;
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("CRON")]
        [DisplayName("Days of Week")]
        public DaysOfWeekComponent DaysOfWeek => cronValue.DaysOfWeek;
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("CRON")]
        public YearsComponent Years => cronValue.Years;

    }
}
