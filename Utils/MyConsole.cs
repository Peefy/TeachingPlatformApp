using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

using TeachingPlatformApp.ViewModels;

namespace TeachingPlatformApp.Utils
{
    public class MyConsole
    {

        protected static Dispatcher _dip = Dispatcher.CurrentDispatcher;
        protected static SynchronizationContext _ds = new DispatcherSynchronizationContext();

        /// <summary>
        /// 向主窗口的控制台写入内容
        /// </summary>
        /// <param name="obj"></param>
        public static void WriteLine(object obj)
        {
            _dip.Invoke(new Action(() =>
            {
                if (App.Current.MainWindow.DataContext is MainWindowViewModel viewModel)
                {
                    viewModel.AppendStatusText(obj.ToString());
                }
            }));

        }

        /// <summary>
        /// 向主窗口的控制台写入字符串
        /// </summary>
        /// <param name="str"></param>
        public static void WriteLine(string str)
        {
            _dip.Invoke(new Action(() =>
            {
                if (App.Current.MainWindow.DataContext is MainWindowViewModel viewModel)
                {
                    viewModel.AppendStatusText(str);
                }
            }));

        }

        public static void RunStart()
        {
            _dip.Invoke(new Action(() =>
            {
                if (App.Current.MainWindow.DataContext is MainWindowViewModel viewModel)
                {
                    viewModel.StartCommand.Execute();
                }
            }));

        }

        public static void RunStop()
        {
            _dip.Invoke(new Action(() =>
            {
                if (App.Current.MainWindow.DataContext is MainWindowViewModel viewModel)
                {
                    viewModel.StopCommand.Execute();
                }
            }));
        }
    }
}
