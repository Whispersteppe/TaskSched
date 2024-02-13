using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.DataStore.DataModel;
using Activity = TaskSched.DataStore.DataModel.Activity;

namespace TaskSched.Test.Fakes
{
    public static partial class TaskSchedFaker
    {
        public static partial class Database
        {
            public static class Activities
            {
                public static Activity Create()
                {
                    return Create(1)[0];
                }

                public static List<Activity> Create(int count)
                {
                    Bogus.Faker<Activity> generator = new Bogus.Faker<Activity>()
                        .RuleFor(x => x.Id, Guid.Empty)
                        .RuleFor(x => x.Name, f => f.Lorem.Sentence())
                        .RuleFor(x => x.ActivityHandlerId, f =>  Guid.Empty)  //  fix this
                        ;

                    return generator.Generate(count);
                }
            }
        }
    }
}
