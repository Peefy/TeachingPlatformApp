using System;
using System.Net;
using System.Windows;

using DuGu.NetFramework.Services;

using TeachingPlatformApp.Speech;
using TeachingPlatformApp.WswPlatform;
using TeachingPlatformApp.Communications;

using DotNetSpeech;

namespace TeachingPlatformApp.Utils
{
    public static class Test
    {
        /// <summary>
        /// 运行测试函数，在主窗口按下键盘T键可以显示出测试按钮
        /// </summary>
        /// <returns>返回测试字符串集合显示在MyConsole上</returns>
        public static string[] Run()
        {

            var FlighterInitInfo =  WswHelper.MathRound(JsonFileConfig.Instance.WswData.FlighterInitInfo, 3);
            var HelicopterInitInfo = WswHelper.MathRound(JsonFileConfig.Instance.WswData.HelicopterInitInfo, 3);
            var vector1 = new Vector(FlighterInitInfo.X / 1000.0f,
                FlighterInitInfo.Y / 1000.0f );
            var vector2 = new Vector(HelicopterInitInfo.X  / 1000.0f, 
                HelicopterInitInfo.Y / 1000.0f);
            var angle =  Vector.AngleBetween(vector1, vector2);
            var deltaX = vector1.X - vector2.X;
            var deltaY = vector1.Y - vector2.Y;

            var wswData = JsonFileConfig.Instance.WswData;
            var heli = new AngleWithLocation
            {
                X = wswData.HelicopterInitInfo.X,
                Y = wswData.HelicopterInitInfo.Y
            };
            var point = WswHelper.DealWswAngleToMyMapData(heli, WswModelKind.Flighter);
            var point2 = WswHelper.DealWswAngleToMyMapData(heli, WswModelKind.Helicopter);

            Ioc.Get<ISpeek>().SpeekAsync("你好");

            return new string[]
            {
                WswHelper.AngleWithLocationToString(point),
                WswHelper.AngleWithLocationToString(point2),
            };
        }
    }
}
