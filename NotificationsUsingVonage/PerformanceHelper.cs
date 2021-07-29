using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace NotificationsUsingVonage
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
        public static Dictionary<string, double> GetMemoryUsageInfo()
        {
            Dictionary<string, double> keyValuePairs = new Dictionary<string, double>();

            ObjectQuery objQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(objQuery);
            ManagementObjectCollection results = searcher.Get();
            Dictionary<string, double> resourceConsumptionResults = new Dictionary<string, double>();

            foreach (ManagementObject result in results)
            {
                var totalVisibleMemory = Math.Round(double.Parse(result["TotalVisibleMemorySize"].ToString()), 2);
                var totalFreeMemory = Math.Round(double.Parse(result["FreePhysicalMemory"].ToString()), 2);

                resourceConsumptionResults.Add("Total_Visible_Memory", totalVisibleMemory);
                resourceConsumptionResults.Add("Free_Physical_Memory", totalFreeMemory);
            }

            double totalVisibleMemorySize = Math.Round(resourceConsumptionResults["Total_Visible_Memory"] / (1024 * 1024), 2);            
            double freePhysicalMemory = Math.Round(resourceConsumptionResults["Free_Physical_Memory"] / (1024 * 1024), 2);
            double totalUsedMemory = totalVisibleMemorySize - freePhysicalMemory;
            double memory_usage = 0;

            try
            {
                memory_usage = ((totalVisibleMemorySize - freePhysicalMemory) / totalVisibleMemorySize) * 100;
                keyValuePairs.Add("Total_Visible_Memory", totalVisibleMemorySize);
                keyValuePairs.Add("Total_Free_Memory", freePhysicalMemory);
                keyValuePairs.Add("Total_Used_Memory", totalUsedMemory);
                keyValuePairs.Add("Total_Used_Memory_Percentage", memory_usage);
            }
            catch
            {
                throw;
            }

            return keyValuePairs;
        }
    }
}
