using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
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
                name = value;
                OnPropertyChanged();
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

        [ReadOnly(true)]
        [Browsable(true)]
        [Description("Cron data for the schedule - Sec Min Hour DayOfMonth Month DayOfWeek Year")]
        [Category("ID")]
        public string LongCRONData
        {
            get
            {
                return cronValue.LongValue;
            }
        }

        [ReadOnly(true)]
        [Browsable(true)]
        [Description("Cron data for the schedule - Sec Min Hour DayOfMonth Month DayOfWeek Year")]
        [Category("ID")]
        public string CronDescription
        {
            get 
            {
                CronExpressionDescriptor.ExpressionDescriptor expressionDescriptor = new CronExpressionDescriptor.ExpressionDescriptor(CRONData);

                var description = expressionDescriptor.GetDescription(CronExpressionDescriptor.DescriptionTypeEnum.FULL);

                return description;
            }
        }
        public override string ToString()
        {
            return $"{CronDescription}";
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
        [DisplayName("1. Seconds")]
        public SecondsComponent Seconds => cronValue.Seconds;
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("CRON")]
        [DisplayName("2. Minutes")]
        public MinutesComponent Minutes => cronValue.Minutes;
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("CRON")]
        [DisplayName("3. Hours")]
        public HoursComponent Hours => cronValue.Hours;
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("CRON")]
        [DisplayName("4. Days of Month")]
        public DaysOfMonthComponent DaysOfMonth => cronValue.DaysOfMonth;
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("CRON")]
        [DisplayName("5. Months")]
        public MonthsComponent Months => cronValue.Months;
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("CRON")]
        [DisplayName("6. Days of Week")]
        public DaysOfWeekComponent DaysOfWeek => cronValue.DaysOfWeek;
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("CRON")]
        [DisplayName("7. Years")]
        public YearsComponent Years => cronValue.Years;

    }
}
