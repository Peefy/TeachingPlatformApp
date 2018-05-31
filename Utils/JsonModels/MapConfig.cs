
using Newtonsoft.Json;

namespace TeachingPlatformApp.Utils.JsonModels
{
    public class MapConfig
    {
        /// <summary>
        /// 清华在地图中的坐标X
        /// </summary>
        [JsonProperty("thuPositionX")]
        public float ThuPositionX { get; set; } = 10;

        /// <summary>
        /// 清华在地图中的坐标Y
        /// </summary>
        [JsonProperty("thuPositionY")]
        public float ThuPositionY { get; set; } = 10;

        /// <summary>
        /// 北京国际机场在地图中的坐标X
        /// </summary>
        [JsonProperty("beijingAirportPositionX")]
        public float BeijingAirportPositionX { get; set; } = 70;

        /// <summary>
        /// 北京国际机场在地图中的坐标Y
        /// </summary>
        [JsonProperty("beijingAirportPositionY")]
        public float BeijingAirportPositionY { get; set; } = -35;

    }

}
