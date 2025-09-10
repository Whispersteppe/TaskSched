using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSched.SchedulerEngine
{
    internal static class SchedulerUtility
    {
        public static async Task<DateTime?> GetNextFireTimeForJob(JobKey jobKey, IScheduler scheduler)
        {
            DateTime? nextFireTime = null;

            var isJobExisting = scheduler.CheckExists(jobKey);
            if (isJobExisting.Result)
            {
                var triggers = await scheduler.GetTriggersOfJob(jobKey);

                if (triggers.Count > 0)
                {
                    foreach (var trigger in triggers)
                    {
                        var nextFireTimeUtc = trigger.GetNextFireTimeUtc();
                        if (nextFireTimeUtc.HasValue)
                        {
                            DateTime triggerNextFireTime = TimeZone.CurrentTimeZone.ToLocalTime(nextFireTimeUtc.Value.DateTime);

                            if (nextFireTime != null)
                            {
                                if (triggerNextFireTime < nextFireTime)
                                {
                                    nextFireTime = triggerNextFireTime;
                                }
                            }
                            else
                            {
                                nextFireTime = triggerNextFireTime;
                            }
                        }
                    }

                }
            }

            return nextFireTime;
        }
    }
}
