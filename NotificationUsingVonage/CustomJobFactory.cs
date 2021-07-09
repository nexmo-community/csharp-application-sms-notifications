using Quartz;
using Quartz.Spi;
using System;

namespace NotificationUsingVonage
{
    public class CustomJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public CustomJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;
            return (IJob)_serviceProvider.GetService(jobDetail.JobType);
        }
        public void ReturnJob(IJob job)
        {
            var disposableJobInstance = job as IDisposable;
            if (disposableJobInstance != null)
            {
                disposableJobInstance.Dispose();
            }
        }
    }
}
