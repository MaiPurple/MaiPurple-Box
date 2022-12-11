using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaiPurple.KusuLib.Model
{
    /// <summary>
    /// Windows Progress Process Info
    /// <para></para>
    /// Windows 运行中的程序信息
    /// </summary>
    public class ProcessInfo
    {
        /// <summary>
        /// Pid 程序Pid
        /// </summary>
        public long Pid { get; set; }
        /// <summary>
        /// 主窗口程序标题
        /// </summary>
        public string? MainWindowTitle { get; set; }
        /// <summary>
        /// 进程名称
        /// </summary>
        public string? ProcessName { get; set; }
        /// <summary>
        /// 开始运行的时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 运行总时间（秒/s)
        /// </summary>
        public double RuningTotalSecond { get; set; }
        /// <summary>
        /// 程序内存占用
        /// </summary>
        public long MemoryFootprint { get; set; }
        /// <summary>
        /// 当前程序的物理位置
        /// </summary>
        public string? ExecuteFilePath { get; set; }
    }
}
