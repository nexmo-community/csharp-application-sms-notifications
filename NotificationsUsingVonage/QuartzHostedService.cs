using Microsoft.Extensions.Hosting;
using Quartz;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationsUsingVonage
{
    public class QuartzHostedService : IHostedService
    {
        private readonly IScheduler _scheduler;
        public QuartzHostedService(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }
        public Task StartAsync(CancellationToken ct)
        {
            return _scheduler.Start(ct);
        }
        public Task StopAsync(CancellationToken ct)
        {
            return _scheduler.Shutdown(ct);
        }
    }
}
