using System;
using System.IO;

namespace TeachingPlatformApp.WswPlatform
{
    /// <summary>
    /// Wsw数据调试计算记录器
    /// </summary>
    public static class WswDataDebuger
    {
        /// <summary>
        /// 记录ip地址和wsw平台发来的数据
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="angleWithLocation"></param>
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
