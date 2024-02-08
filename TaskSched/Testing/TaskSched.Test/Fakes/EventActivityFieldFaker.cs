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
            public static class EventActivityFields
            {
                public static EventActivityField Create(Guid eventId, Guid activityFieldId)
                {
                    return Create(1, eventId, activityFieldId)[0];
                }

                public static List<EventActivityField> Create(int count, Guid eventId, Guid activityFieldId)
                {
                    Bogus.Faker<EventActivityField> generator = new Bogus.Faker<EventActivityField>()
                        .RuleFor(x => x.Id, Guid.Empty)
                        .RuleFor(x => x.Name, f => f.Lorem.Sentence())
                        .RuleFor(x => x.EventActivityId, eventId)
                        .RuleFor(x => x.Value, f => f.Lorem.Sentence())
                        .RuleFor(x => x.ActivityFieldId, activityFieldId)
                        ;

                    return generator.Generate(count);
                }
            }
        }
    }
}
