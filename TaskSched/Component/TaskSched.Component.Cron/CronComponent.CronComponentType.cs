namespace TaskSched.Component.Cron
{
    /// <summary>
    /// the different types of cron types
    /// </summary>
    public enum CronComponentType
    {
        /// <summary>
        /// we'll allow any value within the component part range
        /// </summary>
        AllowAny,

        /// <summary>
        /// a repeating value of the form {start}/{increment}
        /// </summary>
        Repeating,

        /// <summary>
        /// a range of values
        /// </summary>
        Range,

        /// <summary>
        /// a date from the end of the month, usually seen as L, L-1, L-2, and so forth
        /// </summary>
        DaysOfMonthFromLast,

        /// <summary>
        /// the last Day of Week in the month, seen as L (last saturday of the month) 1L (last friday) and so forth
        /// </summary>
        DaysOfWeekFromLast,

        /// <summary>
        /// the component is ignored.  you'll only see this on DayOfMonth and DayOfWeek
        /// </summary>
        Ignored,

        /// <summary>
        /// the Nth instance of a day of the week in the month, usually seen as {day}#{instance}
        /// </summary>
        NthWeekday,
    }
}
