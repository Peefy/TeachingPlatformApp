using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using DuGu.NetFramework.Services;
using TeachingPlatformApp.WswPlatform;
using TeachingPlatformApp.Communications;

namespace TeachingPlatformApp.Utils
{
    public static class Test
    {
        public static string[] Run()
        {
            var bytes = new TeachingCommandBuilder(1, false).BuildCommandBytes();
            var ip = new IPEndPoint(IPAddress.Broadcast, 13000);
            Ioc.Get<ITranslateData>().SendTo(bytes, ip);
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
            return new string[]
            {
                $"{vector1} and {vector2}",
                angle.ToString(),
                distance.ToString(),
                deltaX.ToString(),
                deltaY.ToString()
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
