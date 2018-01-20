using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TeachingPlatformApp.Utils;

namespace TeachingPlatformApp.WswPlatform
{
    public static class WswHelper
    {
        public static string AngleWithLocationToString(AngleWithLocation angle) => 
            $"x:{angle.X} y:{angle.Y} z:{angle.Z} roll:{angle.Roll} yaw:{angle.Yaw} pitch:{angle.Pitch}";

        public static AngleWithLocation MathRoundAngle(AngleWithLocation angle, int digit)
        {
            return new AngleWithLocation
            {
                X = Math.Round(angle.X, digit),
                Y = Math.Round(angle.Y, digit),
                Z = Math.Round(angle.Z, digit),
                Yaw = Math.Round(angle.Yaw, digit),
                Pitch = Math.Round(angle.Pitch, digit),
                Roll = Math.Round(angle.Roll, digit)
            };
        }

        public static string GetAngleWithLocationDeltaXY(string ip, AngleWithLocation angleWithLocation)
        {
            var deltaX = 0.0;
            var deltaY = 0.0;
            var config = JsonFileConfig.Instance;
            if(ip.StartsWith(config.ComConfig.Ip720Platform))
            {
                deltaX = angleWithLocation.X - config.WswData.FlighterInitInfo.X;
                deltaY = angleWithLocation.Y - config.WswData.FlighterInitInfo.Y;
            }
            else if(ip.StartsWith(config.ComConfig.IpWswUdpServer))
            {
                deltaX = angleWithLocation.X - config.WswData.HelicopterInitInfo.X;
                deltaY = angleWithLocation.Y - config.WswData.HelicopterInitInfo.Y;
            }
            return $"deltaX:{deltaX}; deltaY:{deltaY}";
        }

    }
}
