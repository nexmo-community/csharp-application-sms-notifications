using Quartz;
using System.Threading.Tasks;

namespace NotificationsUsingVonage
{
    public class NotificationJob : IJob
    {
        private readonly NotificationManager _notificationManager;
        public NotificationJob(NotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _notificationManager.SendNotification();
        }
    }
}
