using System;
using System.IO;

namespace TeachingPlatformApp.WswPlatform
{
    /// <summary>
    /// Wsw数据调试计算记录器
    /// </summary>
    public static class WswDataDebuger
    {
        public static void Record(string ip, AngleWithLocation angleWithLocation)
        {
            var timeStr = DateTime.Now.ToString();
            var str = WswHelper.AngleWithLocationToString(angleWithLocation);
            var deltaStr = WswHelper.GetWswAngleWithLocationDeltaXY(ip, angleWithLocation);
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
