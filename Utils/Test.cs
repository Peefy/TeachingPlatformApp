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

        static SpVoice speech = new SpVoice();

        public static string[] Run()
        {
            //var bytes = new TeachingCommandBuilder(1, false).BuildCommandBytes();
            //var ip = new IPEndPoint(IPAddress.Parse("192.168.0.134"), 11000);
            //Ioc.Get<ITranslateData>().SendTo(bytes, ip);
            //ip = new IPEndPoint(IPAddress.Parse("192.168.0.134"), 12000);
            //Ioc.Get<ITranslateData>().SendTo(bytes, ip);
            var FlighterInitInfo =  WswHelper.MathRoundAngle(JsonFileConfig.Instance.WswData.FlighterInitInfo, 3);
            var HelicopterInitInfo = WswHelper.MathRoundAngle(JsonFileConfig.Instance.WswData.HelicopterInitInfo, 3);
            var vector1 = new Vector(FlighterInitInfo.X / 1000.0f,
                FlighterInitInfo.Y / 1000.0f );
            var vector2 = new Vector(HelicopterInitInfo.X  / 1000.0f, 
                HelicopterInitInfo.Y / 1000.0f);
            var angle =  Vector.AngleBetween(vector1, vector2);
            var distance = VectorDistance(vector1, vector2);
            var deltaX = vector1.X - vector2.X;
            var deltaY = vector1.Y - vector2.Y;

            var wswData = JsonFileConfig.Instance.WswData;
            var heli = new AngleWithLocation();
            heli.X = wswData.HelicopterInitInfo.X;
            heli.Y = wswData.HelicopterInitInfo.Y;
            var point = WswHelper.MyDealWswAngle(heli, WswAirplane.Flighter);
            var point2 = WswHelper.MyDealWswAngle(heli, WswAirplane.Helicopter);

            Ioc.Get<ISpeek>().SpeekAsync("你好");

            return new string[]
            {
                WswHelper.AngleWithLocationToString(point),
                WswHelper.AngleWithLocationToString(point2),
//                 $"{vector1} and {vector2}",
//                 angle.ToString(),
//                 distance.ToString(),
//                 deltaX.ToString(),
//                 deltaY.ToString(),
            };
        }

        public static double VectorDistance(Vector vector1, Vector vector2)
        {
            var subX = vector1.X - vector2.X;
            var subY = vector1.Y = vector2.Y;
            return Math.Sqrt(subX * subX + subY * subY);
        }

    }
}
