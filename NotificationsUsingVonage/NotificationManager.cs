using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Vonage.Messaging;
using Vonage.Request;

namespace NotificationsUsingVonage
{
    public class NotificationManager
    {
        private ILoggerFactory LoggerFactory { get; }
        private readonly ILogger Logger;
        public IConfiguration Configuration { get; set; }
        public NotificationManager(IConfiguration config, ILoggerFactory loggerFactory)
        {
            Configuration = config;
            LoggerFactory = loggerFactory;

            if (loggerFactory != null)
            {
                Logger = loggerFactory.CreateLogger("NotificationManager");
            }
        }
        public async Task SendNotification()
        {
            await SendResourceUsageInfo();
            await Task.CompletedTask;
        }
        private async Task SendResourceUsageInfo()
        {
            bool isMemoryUsageHigh = false;
            bool isCPUUsageHigh = false;

            var memoryUsage = Math.Round(PerformanceHelper.GetMemoryUsageInPercentage(), 2);
            var cpuUsage = Math.Round(PerformanceHelper.GetCPUUsageInPercentage(), 2);

            var memoryThreshold = Configuration["Memory_Threshold"];
            var cpuThreshold = Configuration["CPU_Threshold"];

            if (memoryUsage > double.Parse(memoryThreshold))
            {
                isMemoryUsageHigh = true;
            }
            if (cpuUsage > double.Parse(cpuThreshold))
            {
                isCPUUsageHigh = true;
            }

            if (isMemoryUsageHigh)
                SendTextMessage(string.Format("Alert!!! Memory Usage: {0}", memoryUsage));
            if (isCPUUsageHigh)
                SendTextMessage(string.Format("Alert!!! CPU Usage: {0}", cpuUsage));

            await Task.CompletedTask;
        }
        private void SendTextMessage(string message)
        {
            try
            {
                var apiKey = Configuration["API_KEY"];
                var apiSecret = Configuration["API_SECRET"];
                var credentials = Credentials.FromApiKeyAndSecret(apiKey, apiSecret);
                var client = new SmsClient(credentials);

                var request = new SendSmsRequest
                {
                    To = Configuration["TO"],
                    From = Configuration["FROM"],
                    Text = message
                };

                var response = client.SendAnSms(request);
                Logger?.LogInformation(response.MessageCount);
            }
            catch (VonageSmsResponseException ex)
            {
                Logger?.LogError(ex.Message);
                throw;
            }
        }
    }
}
