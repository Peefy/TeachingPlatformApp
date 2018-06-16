using System.Windows;

using DuGu.NetFramework.Services;

using TeachingPlatformApp.Converters;
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
            new Bootstrapper().Run();
        }

        void AppInit()
        {
            ServiceInit();
            ConverterPara.Init();
            LogAndConfig.Init();            
            this.Exit += App_Exit;         
        }

        void ServiceInit()
        {
            // 注册服务
            Ioc.Register<ITranslateData, Server>();
            Ioc.Register<ISpeek, TeachingSpeeker>();
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            JsonFileConfig.Instance.WriteToFile();
            LogAndConfig.Config.RenewAllConfig();
        }

    }
}
