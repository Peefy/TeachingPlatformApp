using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TeachingPlatformApp.Utils;
using TeachingPlatformApp.WswPlatform;

namespace TeachingPlatformApp.Models
{
    public class WswModelInfo
    {
        public AngleWithLocation Flighter { get; set; }

        public AngleWithLocation Helicopter { get; set; }

        public AngleWithLocation Missile { get; set; }

        public bool IsConnect { get; set; }

        public WswModelInfo()
        {
            var flighterInfo = JsonFileConfig.Instance.MyFlighterInfo;
            var helicopterInfo = JsonFileConfig.Instance.MyHelicopterInfo;
            var missileInfo = JsonFileConfig.Instance.MyMissileInfo;
            Flighter = new AngleWithLocation()
            {
                X = flighterInfo.InitMyPointX,
                Y = flighterInfo.InitMyPointY,
                Z = flighterInfo.InitMyPointZ,
                Yaw = flighterInfo.InitYaw,
            };
            Helicopter = new AngleWithLocation()
            {
                X = helicopterInfo.InitMyPointX,
                Y = helicopterInfo.InitMyPointY,
                Z = helicopterInfo.InitMyPointZ,
                Yaw = helicopterInfo.InitYaw,
            };
            Missile = new AngleWithLocation()
            {
                X = missileInfo.InitMyPointX,
                Y = missileInfo.InitMyPointY,
                Z = missileInfo.InitMyPointZ,
                Yaw = missileInfo.InitYaw
            };
        }

    }
}
