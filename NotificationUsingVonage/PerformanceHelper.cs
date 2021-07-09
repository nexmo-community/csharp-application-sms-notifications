using System;
using System.Linq;
using System.Management;

namespace NotificationUsingVonage
{
    public static class PerformanceHelper
    {
        public static double GetCPUUsageInPercentage()
        {
            ObjectQuery objQuery = new ObjectQuery("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name=\"_Total\"");
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(objQuery);
            ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
            double cpu_usage = 0;

            if (managementObjectCollection.Count > 0)
            {
                foreach (ManagementObject managementObject in managementObjectCollection)
                {
                    try
                    {
                        cpu_usage = 100 - Convert.ToUInt32(managementObject["PercentIdleTime"]);
                        break;
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                }
            }

            return cpu_usage;
        }
        public static double GetMemoryUsageInPercentage()
        {
            string objQuery = "Select * from Win32_OperatingSystem";
            var wmiObject = new ManagementObjectSearcher(objQuery);
            var memoryValues = wmiObject.Get().Cast<ManagementObject>().Select(managementObject => new {
                FreePhysicalMemory = double.Parse(managementObject["FreePhysicalMemory"].ToString()),
                TotalVisibleMemorySize = double.Parse(managementObject["TotalVisibleMemorySize"].ToString())
            }).FirstOrDefault();

            double memory_usage = 0;

            try
            {
                memory_usage = ((memoryValues.TotalVisibleMemorySize - memoryValues.FreePhysicalMemory) / memoryValues.TotalVisibleMemorySize) * 100;
            }
            catch (Exception ex)
            {
                throw;
            }

            return memory_usage;
        }
    }
}
