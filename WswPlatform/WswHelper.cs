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

        public static AngleWithLocation MathRoundAngle(AngleWithLocation angleWithLocation, int digit)
        {
            return new AngleWithLocation
            {
                X = (float)Math.Round(angleWithLocation.X, digit),
                Y = (float)Math.Round(angleWithLocation.Y, digit),
                Z = (float)Math.Round(angleWithLocation.Z, digit),
                Roll = (float)Math.Round(angleWithLocation.Roll, digit),
                Yaw = (float)Math.Round(angleWithLocation.Yaw, digit),
                Pitch = (float)Math.Round(angleWithLocation.Pitch, digit),
            };
        }

        public static AngleWithLocation MyDealWswAngle(AngleWithLocation angle,WswAirplane wswAirplane, int digit = 2)
        {
            var config = JsonFileConfig.Instance;
            var wswInitData = config.WswData;
            var angleNew = new AngleWithLocation();
            if(wswAirplane == WswAirplane.Flighter)
            {
                var myFlighterInfo = config.MyFlighterInfo;
                angleNew.X = (angle.X - wswInitData.FlighterInitInfo.X) *
                    myFlighterInfo.PointScaleFactorX + myFlighterInfo.InitMyPointX;
                angleNew.Y = (angle.Y - wswInitData.FlighterInitInfo.Y) *
                    myFlighterInfo.PointScaleFactorY + myFlighterInfo.InitMyPointY;
                angleNew.Z = (angle.Z - wswInitData.FlighterInitInfo.Z) *
                    myFlighterInfo.PointScaleFactorZ + myFlighterInfo.InitMyPointZ;
                angleNew.Roll = angle.Roll;
                angleNew.Pitch = angle.Pitch;
                angleNew.Yaw = angle.Yaw - myFlighterInfo.InitYaw;

            };
            if(wswAirplane == WswAirplane.Helicopter)
            {
                var myHelicopterInfo = config.MyHelicopterInfo;
                angleNew.X = (angle.X - wswInitData.FlighterInitInfo.X) *
                    myHelicopterInfo.PointScaleFactorX + myHelicopterInfo.InitMyPointX;
                angleNew.Y = (angle.Y - wswInitData.FlighterInitInfo.Y) *
                    myHelicopterInfo.PointScaleFactorY + myHelicopterInfo.InitMyPointY;
                angleNew.Z = (angle.Z - wswInitData.FlighterInitInfo.Z) *
                    myHelicopterInfo.PointScaleFactorZ + myHelicopterInfo.InitMyPointZ;
                angleNew.Roll = angle.Roll;
                angleNew.Pitch = angle.Pitch;
                angleNew.Yaw = angle.Yaw - myHelicopterInfo.InitYaw;
            }
            angleNew.X = Math.Round(angleNew.X, digit);
            angleNew.Y = Math.Round(angleNew.Y, digit);
            angleNew.Z = Math.Round(angleNew.Z, digit);
            angleNew.Roll = Math.Round(angleNew.Roll, digit);
            angleNew.Yaw = Math.Round(angleNew.Yaw, digit);
            angleNew.Pitch = Math.Round(angleNew.Pitch, digit);

            return angleNew;
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
