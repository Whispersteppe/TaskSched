using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.DataModel;
using TaskSched.DataStore.DataModel;
using Calendar = TaskSched.Common.DataModel.Calendar;
using Event = TaskSched.Common.DataModel.Event;
using EventActivity = TaskSched.Common.DataModel.EventActivity;
using EventSchedule = TaskSched.Common.DataModel.EventSchedule;

namespace TaskSched.Test.Fakes
{
    public static partial class TaskSchedFaker
    {
        public static partial class Model
        {
            public static class Events
            {
                public static Event Create(int scheduleCount, int activityCount)
                {
                    return Create(1, scheduleCount, activityCount)[0];
                }

                public static List<Event> Create(int count, int scheduleCount, int activityCount)
                {
                    Bogus.Faker<Event> generator = new Bogus.Faker<Event>()
                        .RuleFor(x => x.IsActive, false)
                        .RuleFor(x => x.Name, f => f.Lorem.Sentence())
                        .RuleFor(x => x.CatchUpOnStartup, false)
                        .RuleFor(x => x.IsActive, true)
                        .RuleFor(x => x.Id, Guid.Empty)
                        .RuleFor(x => x.LastExecution, f => f.Date.Between(DateTime.Now.AddDays(-14), DateTime.Now))
                        .RuleFor(x => x.Schedules, f => EventSchedules.Create(scheduleCount))
                        .RuleFor(x => x.Activities, f => EventActivities.Create(activityCount))
                        ;

                    return generator.Generate(count);
                }
            }

            public static class EventSchedules
            {
                public static EventSchedule Create()
                {
                    return Create(1)[0];
                }

                public static List<EventSchedule> Create(int count)
                {
                    Bogus.Faker<EventSchedule> generator = new Bogus.Faker<EventSchedule>()
                        .RuleFor(x => x.Id, Guid.Empty)
                        .RuleFor(x => x.Name, f => f.Lorem.Sentence())
                        .RuleFor(x => x.CRONData, "0 0 8 * * *")
                        ;

                    return generator.Generate(count);
                }
            }

            public static class EventActivities
            {
                public static EventActivity Create()
                {
                    return Create(1)[0];
                }

                public static List<EventActivity> Create(int count)
                {
                    Bogus.Faker<EventActivity> generator = new Bogus.Faker<EventActivity>()
                        .RuleFor(x => x.Id, Guid.Empty)
                        .RuleFor(x => x.Name, f => f.Lorem.Sentence())
                        ;

                    return generator.Generate(count);
                }
            }

            public static class Calendars
            {
                public static Calendar Create()
                {
                    return Create(1)[0];
                }

                public static List<Calendar> Create(int count)
                {
                    Bogus.Faker<Calendar> generator = new Bogus.Faker<Calendar>()
                        .RuleFor(x => x.Name, f => f.Lorem.Sentence())
                        .RuleFor(x => x.Id, Guid.Empty)
                        ;

                    return generator.Generate(count);
                }
            }


        }
    }
}
