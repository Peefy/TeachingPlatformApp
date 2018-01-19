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
        public static IApplicationConfig Config { get; set; }
        public static ILog Log { get; set; }

        public static void Init()
        {
            Config = ConfigManager.GetConfig();
            Log = LogManager.GetCurrentLogger();
        }
    }
}
