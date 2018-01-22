
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using DuGu.NetFramework.Services;

using TeachingPlatformApp.Speech;
using TeachingPlatformApp.Communications;
using TeachingPlatformApp.Utils;

namespace TeachingPlatformApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppInit();
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        void AppInit()
        {
            Ioc.Register<ITranslateData, Server>();
            Ioc.Register<ISpeek, TeachingSpeeker>();
            LogAndConfig.Init();            
            this.Exit += App_Exit;         
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            JsonFileConfig.Instance.WriteToFile();
            LogAndConfig.Config.RenewAllConfig();
        }

    }
}
