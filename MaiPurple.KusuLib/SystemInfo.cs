using System;
using System.Diagnostics;
using System.Management;
using MaiPurple.KusuLib.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaiPurple.KusuLib.Enum;

namespace MaiPurple.KusuLib
{
    /// <summary>
    /// 系统信息类 - 获取CPU、内存、进程信息
    /// </summary>
    public class SystemInfo
    {
        private PerformanceCounter _cpuCounter = new("Processor", "% Processor Time", "_Total") { MachineName = "." };

        private ManagementClass _memoryManager = new("Win32_PerfFormattedData_PerfOS_Memory");

        private ManagementClass _totalMemoryManager = new("Win32_PhysicalMemory");

        /// <summary>
        /// 获取CPU占用率
        /// </summary>
        public double CpuUsage => GetCpuUsage();

        /// <summary>
        /// 总物理内存（单位GB）
        /// </summary>
        /// <returns></returns>
        public double TotalPhysicalMemory => ToFileFormat(GetTotalPhysicalMemory(), FileSizeUnit.GB);

        /// <summary>
        /// 已用内存（单位GB）
        /// </summary>
        /// <returns></returns>
        public double AvailablePhysicalMemory => ToFileFormat(GetAvailablePhysicalMemory(), FileSizeUnit.GB);

        /// <summary>
        /// 获取CPU占用
        /// </summary>
        /// <returns></returns>
        private double GetCpuUsage()
        {
            var value = _cpuCounter.NextValue();
            return Math.Round(value, 2);
        }


        /// <summary>
        /// 获得已使用的物理内存的大小，单位 (Byte)，如果获取失败，返回 -1.
        /// </summary>
        /// <returns></returns>
        private long GetTotalPhysicalMemory()
        {
            long capacity = 0;
            try
            {
                foreach (var o in _totalMemoryManager.GetInstances())
                {
                    var mo1 = (ManagementObject)o;
                    capacity += long.Parse(mo1.Properties["Capacity"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                capacity = -1;
                Console.WriteLine(ex.Message);
            }

            return capacity;
        }

        /// <summary>
        /// 获得已使用的物理内存的大小，单位 (Byte)，如果获取失败，返回 -1.
        /// </summary>
        /// <returns></returns>
        private long GetAvailablePhysicalMemory()
        {
            long capacity = 0;
            try
            {
                foreach (var o in _memoryManager.GetInstances())
                {
                    var mo1 = (ManagementObject)o;
                    capacity += long.Parse(mo1.Properties["AvailableBytes"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                capacity = -1;
                Console.WriteLine(ex.Message);
            }

            return capacity;
        }

        /// <summary>
        /// 根据指定的文件大小单位，对输入的文件大小（字节表示）进行转换。
        /// </summary>
        /// <param name="filesize">文件文件大小，单位为字节。</param>
        /// <param name="targetUnit">目标单位。</param>
        /// <returns></returns>
        public static double ToFileFormat(long filesize, FileSizeUnit targetUnit = FileSizeUnit.MB)
            => targetUnit switch
            {
                FileSizeUnit.KB => filesize / 1024.0,
                FileSizeUnit.MB => filesize / 1024.0 / 1024,
                FileSizeUnit.GB => filesize / 1024.0 / 1024 / 1024,
                FileSizeUnit.TB => filesize / 1024.0 / 1024 / 1024 / 1024,
                FileSizeUnit.PB => filesize / 1024.0 / 1024 / 1024 / 1024 / 1024,
                _ => filesize
            };

        #region 获得进程列表

        /// <summary>
        /// 获得进程列表 
        /// </summary>
        /// <returns>目前正在运行中的进程列表</returns>
        public List<ProcessInfo> GetProcessInfo()
        {
            var processInfos = new List<ProcessInfo>();
            Process[] processes = Process.GetProcesses();
            foreach (Process instance in processes)
            {
                try
                {
                    var item = new ProcessInfo()
                    {
                        Pid = instance.Id,
                        MainWindowTitle = instance.MainWindowTitle,
                        StartTime = instance.StartTime,
                        ProcessName = instance.ProcessName,
                        RuningTotalSecond = instance.TotalProcessorTime.TotalMilliseconds,
                        MemoryFootprint = instance.WorkingSet64,
                        ExecuteFilePath = instance.MainModule?.FileName
                    };

                    processInfos.Add(item);
                }
                catch
                {
                }
            }

            return processInfos;
        }

        /// <summary>
        /// 获得特定进程信息 
        /// </summary>
        /// <param name="ProcessName">进程名称</param>
        /// <returns></returns>
        public ProcessInfo? GetProcessInfo(string ProcessName)
        {
            Process process = Process.GetProcessesByName(ProcessName).FirstOrDefault();

            if (process == null)
            {
                return null;
            }

            return new ProcessInfo()
            {
                Pid = process.Id,
                MainWindowTitle = process.MainWindowTitle,
                StartTime = process.StartTime,
                ProcessName = process.ProcessName,
                RuningTotalSecond = process.TotalProcessorTime.TotalMilliseconds,
                MemoryFootprint = process.WorkingSet64,
                ExecuteFilePath = process.MainModule?.FileName
            };
        }

        #endregion

        #region 结束指定进程

        /// <summary>
        /// 结束指定进程 
        /// </summary>
        /// <param name="pid">进程的 Process ID </param>
        public static void EndProcess(int pid)
        {
            try
            {
                Process.GetProcessById(pid).Kill();
            }
            catch
            {
            }
        }

        #endregion
    }
}