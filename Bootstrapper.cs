using Ninject;
using Prism.Ninject;
using TeachingPlatformApp.Views;
using System.Windows;

namespace TeachingPlatformApp
{
    class Bootstrapper : NinjectBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Kernel.TryGet<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
