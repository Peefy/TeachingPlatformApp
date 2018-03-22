using Newtonsoft.Json;
using TeachingPlatformApp.WswPlatform;

namespace TeachingPlatformApp.Utils.JsonModels
{
    /// <summary>
    /// Wsw 数据
    /// </summary>
    public class WswData
    {
        /// <summary>
        /// 战斗机的初始数据(姿态，坐标)
        /// </summary>
        [JsonProperty("flighterInitInfo")]
        public AngleWithLocation FlighterInitInfo;

        /// <summary>
        /// 第2个战斗机的初始数据(姿态，坐标)
        /// </summary>
        [JsonProperty("flighterInitInfo2")]
        public AngleWithLocation FlighterInitInfo2;

        /// <summary>
        /// 直升机的初始数据(姿态，坐标)
        /// </summary>
        [JsonProperty("helicopterInitInfo")]
        public AngleWithLocation HelicopterInitInfo;

        /// <summary>
        /// 导弹的初始数据(姿态，坐标)
        /// </summary>
        [JsonProperty("missileInitInfo")]
        public AngleWithLocation MissileInitInfo;

        public WswData()
        {
            FlighterInitInfo = default;
            FlighterInitInfo.X = -2185907.3768;
            FlighterInitInfo.Y = 4365171.6876;
            FlighterInitInfo.Z = 4104669.1241;
            FlighterInitInfo.Yaw = 0.1873927;
            FlighterInitInfo.Pitch = 0.0011897;
            FlighterInitInfo.Roll = 0.0013987;
            HelicopterInitInfo = default;
            HelicopterInitInfo.X = -2165844.6698;
            HelicopterInitInfo.Y = 4379369.0368;
            HelicopterInitInfo.Z = 4104669.0426;
            HelicopterInitInfo.Yaw = -0.5154596;
            HelicopterInitInfo.Pitch = 0.0011189;
            HelicopterInitInfo.Roll = 0.00139873;
            MissileInitInfo = default;
            FlighterInitInfo2 = default;
            FlighterInitInfo2.X = -2185907.3768;
            FlighterInitInfo2.Y = 4365171.6876;
            FlighterInitInfo2.Z = 4104669.1241;
            FlighterInitInfo2.Yaw = 0.1873927;
            FlighterInitInfo2.Pitch = 0.0011897;
            FlighterInitInfo2.Roll = 0.0013987;
        }

    }

}
