using Quartz;
using System;

namespace NotificationUsingVonage
{
    public class CustomJobScheduler
    {
        public static void ScheduleJob(IScheduler scheduler)
        {
            var jobName = "NotificationJob";

            var job = JobBuilder.Create<NotificationJob>()
                .WithIdentity(jobName)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{jobName}.trigger")
                .StartNow()
                .WithSimpleSchedule(scheduleBuilder =>
                    scheduleBuilder
                        .WithInterval(TimeSpan.FromSeconds(5))
                        .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
