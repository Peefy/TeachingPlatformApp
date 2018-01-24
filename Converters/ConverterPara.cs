using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TeachingPlatformApp.Utils;

namespace TeachingPlatformApp.Converters
{
    public static class ConverterPara
    {
  
        public static double XScale { get; set; } = 10;
        public static double YScale { get; set; } = 10;
        public static double XInit { get; set; } = 20;
        public static double YInit { get; set; } = 20;

        public static void Init()
        {
            var config = JsonFileConfig.Instance.GridAxesDrawPara;
            XScale = config.XAxesInternal / config.LabelAxesInterval;
            YScale = config.YAxesInternal / config.LabelAxesInterval;
            XInit = Math.Abs(config.DrawLeft) - 10;
            YInit = Math.Abs(config.DrawTop) - 10;
        }

    }
}
