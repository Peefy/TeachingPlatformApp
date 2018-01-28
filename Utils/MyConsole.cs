using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TeachingPlatformApp.ViewModels;

namespace TeachingPlatformApp.Utils
{
    public static class MyConsole
    {
        /// <summary>
        /// 向主窗口的控制台写入内容
        /// </summary>
        /// <param name="obj"></param>
        public static void WriteLine(object obj)
        {
            if(App.Current.MainWindow.DataContext is MainWindowViewModel viewModel)
            {
                viewModel.AppendStatusText(obj.ToString());
            }
        }

        /// <summary>
        /// 向主窗口的控制台写入字符串
        /// </summary>
        /// <param name="str"></param>
        public static void WriteLine(string str)
        {
            if (App.Current.MainWindow.DataContext is MainWindowViewModel viewModel)
            {
                viewModel.AppendStatusText(str);
            }
        }

    }
}
