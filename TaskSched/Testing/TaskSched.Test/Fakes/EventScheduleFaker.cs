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
            public static class EventSchedules
            {
                public static EventSchedule Create(Guid eventId)
                {
                    return Create(1, eventId)[0];
                }

                public static List<EventSchedule> Create(int count, Guid eventId)
                {
                    Bogus.Faker<EventSchedule> generator = new Bogus.Faker<EventSchedule>()
                        .RuleFor(x => x.Id, Guid.Empty)
                        .RuleFor(x => x.Name, f => f.Lorem.Sentence())
                        .RuleFor(x => x.CRONData, "0 0 8 * * *")
                        .RuleFor(x=>x.EventId, eventId)
                        ;

                    return generator.Generate(count);
                }
            }
        }
    }
}
