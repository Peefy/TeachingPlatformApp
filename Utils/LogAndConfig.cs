using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DuGu.NetFramework.Logs;
using DuGu.NetFramework.Configs;

namespace TeachingPlatformApp.Utils
{
    public class LogAndConfig
    {
        /// <summary>
        /// 键值对配置操作
        /// </summary>
        public static IApplicationConfig Config { get; set; }

        /// <summary>
        /// 日志记录操作
        /// </summary>
        public static ILog Log { get; set; }

        /// <summary>
        /// 使用键值对操作和日志记录操作前初始化
        /// </summary>
        public static void Init()
        {
            Config = ConfigManager.GetConfig();
            Log = LogManager.GetCurrentLogger();
            
        }
    }
}
