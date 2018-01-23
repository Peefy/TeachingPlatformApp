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
        public static void WriteLine(object obj)
        {
            if(App.Current.MainWindow.DataContext is MainWindowViewModel viewModel)
            {
                viewModel.AppendStatusText(obj.ToString());
            }
        }
    }
}
