using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace TeachingPlatformApp.Utils.JsonModels
{
    /// <summary>
    /// 窗体UI 配置
    /// </summary>
    public class WindowUiConfig
    {
        [JsonProperty("isInitShowOn")]
        public bool IsInitShowOn { get; set; } = true;
    }
}
