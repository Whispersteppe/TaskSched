using TaskSched.Component.Cron;
using TaskSched.Test.XUnit;
using Xunit.Abstractions;

namespace TaskSched.Test
{
    public class CronTests : XUnitTestClassBase
    {
        public CronTests(XUnitClassFixture fixture, XUnitCollectionFixture collectionFixture, ITestOutputHelper output) : base(fixture, collectionFixture, output)
        {
        }

        #region Seconds

        [Fact]
        public void ValidateSeconds()
        {
            SecondsComponent component = new SecondsComponent();
            Assert.Equal(CronComponentType.AllowAny, component.ComponentType);
            Assert.Equal(0, component.GetNext());
            Assert.Equal(1, component.GetNext(0));
            Assert.Equal(59, component.GetNext(58));
            Assert.Equal(-1, component.GetNext(59));

            component = new SecondsComponent();
            component.RepeatStart = 1;
            component.RepeatInterval = 5;
            Assert.Equal(CronComponentType.Repeating, component.ComponentType);
            Assert.Equal(1, component.GetNext());
            Assert.Equal(1, component.GetNext(0));
            Assert.Equal(6, component.GetNext(1));
            Assert.Equal(56, component.GetNext(53));
            Assert.Equal(-1, component.GetNext(56));

            component = new SecondsComponent();
            component.Range.Add(1);
            component.Range.Add(7);
            component.Range.Add(29);
            component.Range.Add(15);
            component.Range.Add(23);
            Assert.Equal(CronComponentType.Range, component.ComponentType);
            Assert.Equal(1, component.GetNext());
            Assert.Equal(7, component.GetNext(1));
            Assert.Equal(23, component.GetNext(21));
            Assert.Equal(-1, component.GetNext(33));


        }

        [Theory]
        [InlineData("*", CronComponentType.AllowAny, null, null, null, null)]
        [InlineData("1/5", CronComponentType.Repeating, 1, 5, null, null)]
        [InlineData("1-5", CronComponentType.Range, null, null, "1-5", "1,2,3,4,5")]
        [InlineData("1,2,3,4,5", CronComponentType.Range, null, null, "1-5", "1,2,3,4,5")]
        [InlineData("1-5,18,22", CronComponentType.Range, null, null, "1-5,18,22", "1,2,3,4,5,18,22")]
        [InlineData("1-5,6,22", CronComponentType.Range, null, null, "1-6,22", "1,2,3,4,5,6,22")]

        public void ValidateSecondsAgain(
            string inData, 
            CronComponentType expectedType,
            int? expectedRepeatStart,
            int? expectedRepeatInterval,
            string expectedRange,
            string expectedRangeExpanded
            )
        {
            SecondsComponent component = new SecondsComponent(inData);
            Assert.Equal(expectedType, component.ComponentType);
            if (expectedRepeatStart != null) { Assert.Equal(expectedRepeatStart, component.RepeatStart); }
            if (expectedRepeatInterval != null) { Assert.Equal(expectedRepeatInterval, component.RepeatInterval); }
            if (expectedRange != null) { Assert.Equal(expectedRange, component.GetPiece()); }
            if (expectedRangeExpanded != null) { Assert.Equal(expectedRangeExpanded, string.Join(',', component.Range));  }

        }

        #endregion

        #region Minutes

        [Fact]
        public void ValidateMinutes()
        {
            MinutesComponent component = new MinutesComponent();
            Assert.Equal(CronComponentType.AllowAny, component.ComponentType);
            Assert.Equal(0, component.GetNext());
            Assert.Equal(1, component.GetNext(0));
            Assert.Equal(59, component.GetNext(58));
            Assert.Equal(-1, component.GetNext(59));

            component = new MinutesComponent();
            component.RepeatStart = 1;
            component.RepeatInterval = 5;
            Assert.Equal(CronComponentType.Repeating, component.ComponentType);
            Assert.Equal(1, component.GetNext());
            Assert.Equal(1, component.GetNext(0));
            Assert.Equal(6, component.GetNext(1));
            Assert.Equal(56, component.GetNext(53));
            Assert.Equal(-1, component.GetNext(56));

            component = new MinutesComponent();
            component.Range.Add(1);
            component.Range.Add(7);
            component.Range.Add(29);
            component.Range.Add(15);
            component.Range.Add(23);
            Assert.Equal(CronComponentType.Range, component.ComponentType);
            Assert.Equal(1, component.GetNext());
            Assert.Equal(7, component.GetNext(1));
            Assert.Equal(23, component.GetNext(21));
            Assert.Equal(-1, component.GetNext(33));


        }

        [Theory]
        [InlineData("*", CronComponentType.AllowAny, null, null, null, null)]
        [InlineData("1/5", CronComponentType.Repeating, 1, 5, null, null)]
        [InlineData("1-5", CronComponentType.Range, null, null, "1-5", "1,2,3,4,5")]
        [InlineData("1,2,3,4,5", CronComponentType.Range, null, null, "1-5", "1,2,3,4,5")]
        [InlineData("1-5,18,22", CronComponentType.Range, null, null, "1-5,18,22", "1,2,3,4,5,18,22")]
        [InlineData("1-5,6,22", CronComponentType.Range, null, null, "1-6,22", "1,2,3,4,5,6,22")]

        public void ValidateMinutesAgain(
            string inData,
            CronComponentType expectedType,
            int? expectedRepeatStart,
            int? expectedRepeatInterval,
            string expectedRange,
            string expectedRangeExpanded
            )
        {
            MinutesComponent component = new MinutesComponent(inData);
            Assert.Equal(expectedType, component.ComponentType);
            if (expectedRepeatStart != null) { Assert.Equal(expectedRepeatStart, component.RepeatStart); }
            if (expectedRepeatInterval != null) { Assert.Equal(expectedRepeatInterval, component.RepeatInterval); }
            if (expectedRange != null) { Assert.Equal(expectedRange, component.GetPiece()); }
            if (expectedRangeExpanded != null) { Assert.Equal(expectedRangeExpanded, string.Join(',', component.Range)); }

        }

        #endregion


        #region Hours

        [Fact]
        public void ValidateHours()
        {
            HoursComponent component = new HoursComponent();
            Assert.Equal(CronComponentType.AllowAny, component.ComponentType);
            Assert.Equal(0, component.GetNext());
            Assert.Equal(1, component.GetNext(0));
            Assert.Equal(23, component.GetNext(22));
            Assert.Equal(-1, component.GetNext(23));

            component = new HoursComponent();
            component.RepeatStart = 1;
            component.RepeatInterval = 5;
            Assert.Equal(CronComponentType.Repeating, component.ComponentType);
            Assert.Equal(1, component.GetNext());
            Assert.Equal(1, component.GetNext(0));
            Assert.Equal(6, component.GetNext(1));
            Assert.Equal(21, component.GetNext(16));
            Assert.Equal(-1, component.GetNext(21));

            component = new HoursComponent();
            component.Range.Add(1);
            component.Range.Add(7);
            component.Range.Add(23);
            component.Range.Add(15);
            component.Range.Add(9);
            Assert.Equal(CronComponentType.Range, component.ComponentType);
            Assert.Equal(1, component.GetNext());
            Assert.Equal(7, component.GetNext(1));
            Assert.Equal(23, component.GetNext(21));
            Assert.Equal(-1, component.GetNext(23));


        }

        [Theory]
        [InlineData("*", CronComponentType.AllowAny, null, null, null, null)]
        [InlineData("1/5", CronComponentType.Repeating, 1, 5, null, null)]
        [InlineData("1-5", CronComponentType.Range, null, null, "1-5", "1,2,3,4,5")]
        [InlineData("1,2,3,4,5", CronComponentType.Range, null, null, "1-5", "1,2,3,4,5")]
        [InlineData("1-5,18,22", CronComponentType.Range, null, null, "1-5,18,22", "1,2,3,4,5,18,22")]
        [InlineData("1-5,6,22", CronComponentType.Range, null, null, "1-6,22", "1,2,3,4,5,6,22")]

        public void ValidateHoursAgain(
            string inData,
            CronComponentType expectedType,
            int? expectedRepeatStart,
            int? expectedRepeatInterval,
            string expectedRange,
            string expectedRangeExpanded
            )
        {
            HoursComponent component = new HoursComponent(inData);
            Assert.Equal(expectedType, component.ComponentType);
            if (expectedRepeatStart != null) { Assert.Equal(expectedRepeatStart, component.RepeatStart); }
            if (expectedRepeatInterval != null) { Assert.Equal(expectedRepeatInterval, component.RepeatInterval); }
            if (expectedRange != null) { Assert.Equal(expectedRange, component.GetPiece()); }
            if (expectedRangeExpanded != null) { Assert.Equal(expectedRangeExpanded, string.Join(',', component.Range)); }

        }

        #endregion


        #region Months

        [Fact]
        public void ValidateMonths()
        {
            MonthsComponent component = new MonthsComponent();
            Assert.Equal(CronComponentType.AllowAny, component.ComponentType);
            Assert.Equal(1, component.GetNext());
            Assert.Equal(2, component.GetNext(1));
            Assert.Equal(12, component.GetNext(11));
            Assert.Equal(-1, component.GetNext(12));

            component = new MonthsComponent();
            component.RepeatStart = 1;
            component.RepeatInterval = 3;
            Assert.Equal(CronComponentType.Repeating, component.ComponentType);
            Assert.Equal(1, component.GetNext());
            Assert.Equal(1, component.GetNext(0));
            Assert.Equal(4, component.GetNext(1));
            Assert.Equal(10, component.GetNext(7));
            Assert.Equal(-1, component.GetNext(11));

            component = new MonthsComponent();
            component.Range.Add(1);
            component.Range.Add(7);
            component.Range.Add(3);
            component.Range.Add(11);
            component.Range.Add(4);
            Assert.Equal(CronComponentType.Range, component.ComponentType);
            Assert.Equal(1, component.GetNext());
            Assert.Equal(3, component.GetNext(1));
            Assert.Equal(7, component.GetNext(4));
            Assert.Equal(-1, component.GetNext(12));


        }

        [Theory]
        [InlineData("*", CronComponentType.AllowAny, null, null, null, null)]
        [InlineData("1/5", CronComponentType.Repeating, 1, 5, null, null)]
        [InlineData("1-5", CronComponentType.Range, null, null, "1-5", "1,2,3,4,5")]
        [InlineData("1,2,3,4,5", CronComponentType.Range, null, null, "1-5", "1,2,3,4,5")]
        [InlineData("1-5,18,22", CronComponentType.Range, null, null, "1-5,18,22", "1,2,3,4,5,18,22")]
        [InlineData("1-5,6,22", CronComponentType.Range, null, null, "1-6,22", "1,2,3,4,5,6,22")]

        public void ValidateMonthsAgain(
            string inData,
            CronComponentType expectedType,
            int? expectedRepeatStart,
            int? expectedRepeatInterval,
            string expectedRange,
            string expectedRangeExpanded
            )
        {
            MonthsComponent component = new MonthsComponent(inData);
            Assert.Equal(expectedType, component.ComponentType);
            if (expectedRepeatStart != null) { Assert.Equal(expectedRepeatStart, component.RepeatStart); }
            if (expectedRepeatInterval != null) { Assert.Equal(expectedRepeatInterval, component.RepeatInterval); }
            if (expectedRange != null) { Assert.Equal(expectedRange, component.GetPiece()); }
            if (expectedRangeExpanded != null) { Assert.Equal(expectedRangeExpanded, string.Join(',', component.Range)); }

        }

        #endregion


        #region Years

        [Fact]
        public void ValidateYears()
        {
            YearsComponent component = new YearsComponent();
            Assert.Equal(CronComponentType.AllowAny, component.ComponentType);
            Assert.Equal(1970, component.GetNext());
            Assert.Equal(1970, component.GetNext(0));
            Assert.Equal(1971, component.GetNext(1970));
            Assert.Equal(2024, component.GetNext(2023));
            Assert.Equal(-1, component.GetNext(2100));

            component = new YearsComponent();
            component.RepeatStart = 1980;
            component.RepeatInterval = 5;
            Assert.Equal(CronComponentType.Repeating, component.ComponentType);
            Assert.Equal(1980, component.GetNext());
            Assert.Equal(1985, component.GetNext(1980));
            Assert.Equal(1995, component.GetNext(1990));
            Assert.Equal(2055, component.GetNext(2050));
            Assert.Equal(-1, component.GetNext(2100));

            component = new YearsComponent();
            component.Range.Add(1970);
            component.Range.Add(1980);
            component.Range.Add(2050);
            component.Range.Add(1999);
            component.Range.Add(2024);
            Assert.Equal(CronComponentType.Range, component.ComponentType);
            Assert.Equal(1970, component.GetNext());
            Assert.Equal(1980, component.GetNext(1970));
            Assert.Equal(1999, component.GetNext(1981));
            Assert.Equal(-1, component.GetNext(2070));


        }

        [Theory]
        [InlineData("*", CronComponentType.AllowAny, null, null, null, null)]
        [InlineData("1/5", CronComponentType.Repeating, 1, 5, null, null)]
        [InlineData("1-5", CronComponentType.Range, null, null, "1-5", "1,2,3,4,5")]
        [InlineData("1,2,3,4,5", CronComponentType.Range, null, null, "1-5", "1,2,3,4,5")]
        [InlineData("1-5,18,22", CronComponentType.Range, null, null, "1-5,18,22", "1,2,3,4,5,18,22")]
        [InlineData("1-5,6,22", CronComponentType.Range, null, null, "1-6,22", "1,2,3,4,5,6,22")]

        public void ValidateYearsAgain(
            string inData,
            CronComponentType expectedType,
            int? expectedRepeatStart,
            int? expectedRepeatInterval,
            string expectedRange,
            string expectedRangeExpanded
            )
        {
            YearsComponent component = new YearsComponent(inData);
            Assert.Equal(expectedType, component.ComponentType);
            if (expectedRepeatStart != null) { Assert.Equal(expectedRepeatStart, component.RepeatStart); }
            if (expectedRepeatInterval != null) { Assert.Equal(expectedRepeatInterval, component.RepeatInterval); }
            if (expectedRange != null) { Assert.Equal(expectedRange, component.GetPiece()); }
            if (expectedRangeExpanded != null) { Assert.Equal(expectedRangeExpanded, string.Join(',', component.Range)); }

        }

        #endregion

        [Theory]
        [InlineData(101, "* * * * * ? *", "* * * * * ? *", "1/1/2024 9:00:00", "1/1/2024 9:00:01")]
        [InlineData(102, "0 * * * * ? *", "0 * * * * ? *", "1/1/2024 9:00:00", "1/1/2024 9:01:00" )]
        [InlineData(103, "0/5 * * * * ? *", "0/5 * * * * ? *", "1/1/2024 9:00:00", "1/1/2024 9:00:05")]
        [InlineData(104, "0 0/5 * * * ? *", "0 0/5 * * * ? *", "1/1/2024 9:00:00", "1/1/2024 9:05:00")]
        [InlineData(105, "0 0 9/5 * * ? *", "0 0 9/5 * * ? *", "1/1/2024 9:00:00", "1/1/2024 14:00:00")]
        [InlineData(106, "0 0 0 1 1/3 ? *", "0 0 0 1 1/3 ? *", "1/1/2024 9:00:00", "4/1/2024 00:00:00")]
        [InlineData(107, "0 0 0 ? * 1 *", "0 0 0 ? * 1 *", "1/1/2024 9:00:00", "1/7/2024 00:00:00")]
        [InlineData(108, "0 0 0 L * ? *", "0 0 0 L * ? *", "1/1/2024 9:00:00", "1/31/2024 00:00:00")]
        [InlineData(109, "0 0 0 L-3 * ? *", "0 0 0 L-3 * ? *", "1/1/2024 9:00:00", "1/29/2024 00:00:00")]
        [InlineData(110, "0 0 0 L-3 * ? *", "0 0 0 L-3 * ? *", "1/31/2024 9:00:00", "2/27/2024 00:00:00")]
        [InlineData(111, "0 0 0 2,4,6 * ? *", "0 0 0 2,4,6 * ? *", "1/1/2024 9:00:00", "1/2/2024 00:00:00")]

        [InlineData(112, "0 0 0 ? * L *", "0 0 0 ? * L *", "1/1/2024 9:00:00", "1/27/2024 00:00:00")]
        [InlineData(113, "0 0 0 ? * 5L *", "0 0 0 ? * 5L *", "1/1/2024 9:00:00", "1/25/2024 00:00:00")]

        [InlineData(114, "0 0 0 ? * 6#2 *", "0 0 0 ? * 6#2 *", "1/1/2024 9:00:00", "1/12/2024 00:00:00")]

        public void NextDateTestIteration(int index, string cronString, string expectedCronString, string startDateString, string expectedDateString)
        {
            DateTime startDate = DateTime.Parse(startDateString);
            DateTime expectedDate = DateTime.Parse(expectedDateString);

            CronValue cronValue = new CronValue(cronString);

            DateTime actualNextDate = cronValue.NextTime(startDate);

            Assert.Equal (expectedDate, actualNextDate);

            string cronStringResult = cronValue.Value;
            Assert.Equal(expectedCronString, cronStringResult);


        }

        [Theory]
        [InlineData(103, "0 0 8 * * ? *", "1/1/2024 9:00:00")]

        public void NextDateTest(int index, string cronString, string startDateString)
        {
            DateTime startDate = DateTime.Parse(startDateString);

            CronValue cronValue = new CronValue(cronString);

            List<DateTime> actualNextDates = cronValue.NextTimes(startDate, 10);

            Assert.NotEmpty (actualNextDates);
            Assert.Equal(10, actualNextDates.Count);
            WriteLine(actualNextDates);

            //  and from today
            var singleTime = cronValue.NextTime();
            WriteLine(singleTime);

            var datesFromNow = cronValue.NextTimes(10);
            WriteLine (datesFromNow);


        }

        [Fact]
        public void YearEdges()
        {
            DateTime startDate = DateTime.Parse("1/1/1960 9:00:00");
            CronValue cronValue = new CronValue("0 0 8 * * ? 1970-1980");

            var nextDate = cronValue.NextTime(startDate);

            DateTime expectedDate = DateTime.Parse("1/1/1970 8:00:00");
            Assert.Equal(expectedDate, nextDate);

            nextDate = cronValue.NextTime(DateTime.Parse("1/1/1990 9:00:00"));
            Assert.Equal(DateTime.MaxValue, nextDate);

        }

    }
}
