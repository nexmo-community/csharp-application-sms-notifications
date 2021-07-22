using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;
using Vonage.Messaging;
using Vonage.Request;

namespace NotificationsUsingVonage
{
    public class NotificationJob : IJob
    {
        private ILoggerFactory LoggerFactory { get; }
        private readonly ILogger Logger;
        public IConfiguration Configuration { get; set; }
        public NotificationJob(IConfiguration config, ILoggerFactory loggerFactory)
        {
            Configuration = config;

            LoggerFactory = loggerFactory;

            if (loggerFactory != null)
            {
                Logger = loggerFactory.CreateLogger("NotificationJob");
            }
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var memoryUsage = Math.Round(PerformanceHelper.GetMemoryUsageInPercentage(), 2);
            var cpuUsage = Math.Round(PerformanceHelper.GetCPUUsageInPercentage(), 2);

            Logger?.LogInformation("CPU Usage %: {0}", cpuUsage.ToString());
            Logger?.LogInformation("Memory Usage %: {0}", memoryUsage.ToString());

            var memoryThreshold = Configuration["Memory_Threshold"];
            var cpuThreshold = Configuration["CPU_Threshold"];

            if (memoryUsage > double.Parse(memoryThreshold))
            {
                Logger?.LogWarning(string.Format("Alert!!! Memory Usage: {0}", memoryUsage));
                SendTextMessage(string.Format("Alert!!! Memory Usage: {0}", memoryUsage));
            }
            if (cpuUsage > double.Parse(cpuThreshold))
            {
                Logger?.LogWarning(string.Format("Alert!!! CPU Usage: {0}", cpuUsage));
                SendTextMessage(string.Format("Alert!!! CPU Usage: {0}", cpuUsage));
            }

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
