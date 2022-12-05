using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MaiPurple_Box
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //timer_Tick();
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            // 设置此属性可以防止拖动到屏幕边缘，窗体最大化
            this.ResizeMode = ResizeMode.NoResize;
        }
         private void timer_Tick()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            Task.Run(() =>
            {   
                while (true)
                {
                    ramCounter.NextValue();
                    cpuCounter.NextValue();
                    Thread.Sleep(1000);
                    double total = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
                    double available = 1024.0 * 1024.0 * ramCounter.NextValue();
                    var cpuUsage = cpuCounter.NextValue();
                    string cpuUsageStr = string.Format("{0:f2} %", cpuUsage);
                    var ramAvailable = ramCounter.NextValue();
                    //var ramleft = 100.0 * (total - used) / total;
                    string ramleft = string.Format("{0:f2} %", 100.0 * (total - available) / total);
                    MEM.Dispatcher.Invoke((Action)(() =>
                    {
                        MEM.Text = ramleft;
                        //CPU.Text = cpuUsageStr;
                    }));
                    CPU.Dispatcher.Invoke((Action)(() =>
                    {
                        //MEM.Text = ramAvaiableStr;
                        CPU.Text = cpuUsageStr;
                    }));
                }
            });


            //My.Computer.Info.TotalPhysicalMemory


        }

    }
}
