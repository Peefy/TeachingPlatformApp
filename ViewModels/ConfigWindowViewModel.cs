using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

using Prism.Commands;
using Prism.Mvvm;

using TeachingPlatformApp.Utils;
using TeachingPlatformApp.Utils.JsonModels;

namespace TeachingPlatformApp.ViewModels
{
    public class ConfigWindowViewModel : BindableBase
    {
        private string _title = "配置";
        /// <summary>
        /// 配置窗口标题
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _comConfigName = "所有配置";
        /// <summary>
        /// 配置名称
        /// </summary>
        public string ComConfigName
        {
            get => _comConfigName;
            set => SetProperty(ref _comConfigName, value);
        }

        private ComConfig _comConfig;
        /// <summary>
        /// 通信配置
        /// </summary>
        public ComConfig ComConfig
        {
            get => _comConfig;
            set => SetProperty(ref _comConfig, value);
        }

        private string _configString = "";
        /// <summary>
        /// 配置字符串
        /// </summary>
        public string ConfigString
        {
            get => _configString;
            set => SetProperty(ref _configString, value);
        }

        /// <summary>
        /// 保存命令
        /// </summary>
        public DelegateCommand SaveCommand { get; set; }

        public ConfigWindowViewModel()
        {
            ComConfig = JsonFileConfig.Instance.ComConfig;
            ConfigString = JsonFileConfig.Instance.ToString();
            SaveCommand = new DelegateCommand(() =>
            {
                try
                {
                    Title = "部分配置重启之后生效...";
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
