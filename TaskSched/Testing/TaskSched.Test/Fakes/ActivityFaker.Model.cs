using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.DataModel;

namespace TaskSched.Test.Fakes
{
    public static partial class TaskSchedFaker
    {
        public static partial class Model
        {
            public static class Activities
            {
                public static Activity Create(int activityFieldCount = 0)
                {
                    return Create(1, activityFieldCount)[0];
                }

                public static List<Activity> Create(int count, int activityFieldCount = 0)
                {
                    Bogus.Faker<Activity> generator = new Bogus.Faker<Activity>()
                        .RuleFor(x => x.Id, Guid.Empty)
                        .RuleFor(x => x.Name, f => f.Lorem.Sentence())
                        .RuleFor(x => x.ActivityHandlerId, f =>  Guid.Empty)
                        .RuleFor(x => x.DefaultFields, f => ActivityFields.Create(activityFieldCount, Guid.Empty))
                        ;

                    return generator.Generate(count);
                }
            }

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
                        .RuleFor(x => x.Value, f => f.Lorem.Sentence())
                        ;

                    return generator.Generate(count);
                }
            }

        }
    }
}
