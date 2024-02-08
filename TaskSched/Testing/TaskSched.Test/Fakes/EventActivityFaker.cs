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
            public static class EventActivities
            {
                public static EventActivity Create(Guid eventId, Guid activityId)
                {
                    return Create(1, eventId, activityId)[0];
                }

                public static List<EventActivity> Create(int count, Guid eventId, Guid activityId)
                {
                    Bogus.Faker<EventActivity> generator = new Bogus.Faker<EventActivity>()
                        .RuleFor(x => x.Id, Guid.Empty)
                        .RuleFor(x => x.Name, f => f.Lorem.Sentence())
                        .RuleFor(x => x.EventId, eventId)
                        .RuleFor(x => x.ActivityId, activityId)
                        ;

                    return generator.Generate(count);
                }
            }
        }
    }
}
