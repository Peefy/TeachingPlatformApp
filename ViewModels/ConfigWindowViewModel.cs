using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ConfigWindowViewModel()
        {

        }
    }
}
