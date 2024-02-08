using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.DataStore.DataModel;

namespace TaskSched.Test.Fakes
{
    public static partial class TaskSchedFaker
    {
        public static partial class Database
        {
            public static class Events
            {
                public static Event Create()
                {
                    return Create(1)[0];
                }

                public static List<Event> Create(int count)
                {
                    Bogus.Faker<Event> generator = new Bogus.Faker<Event>()
                        .RuleFor(x => x.IsActive, false)
                        .RuleFor(x => x.Name, f => f.Lorem.Sentence())
                        .RuleFor(x=>x.CatchUpOnStartup, false)
                        .RuleFor(x=>x.IsActive, true)
                        .RuleFor(x=>x.Id, Guid.Empty)
                        .RuleFor(x=>x.LastExecution, f=>f.Date.Between(DateTime.Now.AddDays(-14), DateTime.Now))
                        .RuleFor(x => x.Schedules, new List<EventSchedule>())
                        .RuleFor(x => x.Activities, new List<EventActivity>())
                        ;

                    return generator.Generate(count);
                }
            }
        }
    }
}
