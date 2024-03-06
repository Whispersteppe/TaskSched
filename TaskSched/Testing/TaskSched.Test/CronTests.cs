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


    }
}
