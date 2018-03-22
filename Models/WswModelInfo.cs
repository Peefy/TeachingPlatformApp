using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TeachingPlatformApp.Utils;
using TeachingPlatformApp.WswPlatform;

namespace TeachingPlatformApp.Models
{
    public class TranslateInfo
    {
        /// <summary>
        /// 战斗机的数据
        /// </summary>
        public AngleWithLocation Flighter { get; set; }

        /// <summary>
        /// 第2个战斗机的数据
        /// </summary>
        public AngleWithLocation Flighter2 { get; set; }

        /// <summary>
        /// 直升机的数据
        /// </summary>
        public AngleWithLocation Helicopter { get; set; }

        /// <summary>
        /// 导弹的数据
        /// </summary>
        public AngleWithLocation Missile { get; set; }

        /// <summary>
        /// Udp是否与所有平台连接
        /// </summary>
        public bool IsConnect { get; set; }

        /// <summary>
        /// 是否检测飞行实验是否合格
        /// </summary>
        public bool IsTest { get; set; }

        /// <summary>
        /// 是否发出了导弹
        /// </summary>
        public bool HasMissile { get; set; }

        /// <summary>
        /// 正在进行的飞行实验名称
        /// </summary>
        public string FlightExperimentName { get; set; } = "航线飞行";

        /// <summary>
        /// 正在进行的飞行实验在实验列表中的索引
        /// </summary>
        public int FlightExperimentIndex { get; set; } = 0;

        public TranslateInfo()
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
