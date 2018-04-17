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
        /// 坐标原点数据
        /// </summary>
        [JsonProperty("zeroPointInitInfo")]
        public AngleWithLocation ZeroPointInitInfo;

        /// <summary>
        /// 战斗机的初始数据(姿态，坐标)
        /// </summary>
        [JsonProperty("flighterInitInfo")]
        public AngleWithLocation FlighterInitInfo;

        /// <summary>
        /// 第2个战斗机的初始数据(姿态，坐标)
        /// </summary>
        [JsonProperty("flighterInitInfo2")]
        public AngleWithLocation Flighter2InitInfo;

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
            ZeroPointInitInfo = default;
            ZeroPointInitInfo.X = -2165844.6698;
            ZeroPointInitInfo.Y = 4379369.0368;
            ZeroPointInitInfo.Z = 4104669.0426;
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
            Flighter2InitInfo = default;
            Flighter2InitInfo.X = -2185907.3768;
            Flighter2InitInfo.Y = 4365171.6876;
            Flighter2InitInfo.Z = 4104669.1241;
            Flighter2InitInfo.Yaw = 0.1873927;
            Flighter2InitInfo.Pitch = 0.0011897;
            Flighter2InitInfo.Roll = 0.0013987;
        }

    }

}
