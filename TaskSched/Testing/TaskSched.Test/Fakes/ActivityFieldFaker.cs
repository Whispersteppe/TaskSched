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
            public static class ActivityFields
            {
                public static ActivityField Create(Guid activityId)
                {
                    return Create(1, activityId)[0];
                }

                public static List<ActivityField> Create(int count, Guid activityId)
                {
                    Bogus.Faker<ActivityField> generator = new Bogus.Faker<ActivityField>()
                        .RuleFor(x => x.Id, Guid.Empty)
                        .RuleFor(x => x.Name, f => f.Lorem.Sentence())
                        .RuleFor(x => x.ActivityId, activityId)
                        .RuleFor(x => x.Value, f => f.Lorem.Sentence())
                        ;

                    return generator.Generate(count);
                }
            }
        }
    }
}
