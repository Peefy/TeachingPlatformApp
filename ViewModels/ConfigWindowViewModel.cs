using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Prism.Commands;
using Prism.Mvvm;

using TeachingPlatformApp.Utils;

namespace TeachingPlatformApp.ViewModels
{
    public class ConfigWindowViewModel : BindableBase
    {
        private string _title = "配置";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _comConfigName = "通信配置";
        public string ComConfigName
        {
            get => _comConfigName;
            set => SetProperty(ref _comConfigName, value);
        }

        private ComConfig _comConfig;
        public ComConfig ComConfig
        {
            get => _comConfig;
            set => SetProperty(ref _comConfig, value);
        }

        private string _configString = "";
        public string ConfigString
        {
            get => _configString;
            set => SetProperty(ref _configString, value);
        }

        public DelegateCommand SaveCommand { get; set; }

        public ConfigWindowViewModel()
        {
            ComConfig = JsonFileConfig.Instance.ComConfig;
            ConfigString = JsonFileConfig.Instance.ToString();
            SaveCommand = new DelegateCommand(() =>
            {
                try
                {
                    JsonFileConfig.Instance.SetConfig(ConfigString);
                }
                catch (Exception ex)
                {
                    LogAndConfig.Log.Error(ex);
                }
            });
        }
    }
}
