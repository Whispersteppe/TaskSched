namespace TaskSched.Component.Cron
{
    public enum CronComponentType
    {
        AllowAny,
        Repeating,
        Range,
        DaysOfMonthFromLast,
        DaysOfWeekFromLast,
        Ignored,
        NthWeekday,
    }
}
