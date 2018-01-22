using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TeachingPlatformApp.Utils;

namespace TeachingPlatformApp.WswPlatform
{
    public static class WswDataDebuger
    {
        public static void Record(string ip, AngleWithLocation angleWithLocation)
        {
            var timeStr = DateTime.Now.ToString();
            var str = WswHelper.AngleWithLocationToString(angleWithLocation);
            var deltaStr = WswHelper.GetAngleWithLocationDeltaXY(ip, angleWithLocation);
            var fileName = "ip.txt";
            File.AppendAllLines(fileName, new string[]
            {
                timeStr + $"__{ip}:",
                str,
                deltaStr
            });
        }
    }
}
