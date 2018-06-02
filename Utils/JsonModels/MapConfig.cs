
using Newtonsoft.Json;

namespace TeachingPlatformApp.Utils.JsonModels
{
    public class MapConfig
    {

        /// <summary>
        /// 清华在地图中的维度
        /// </summary>
        [JsonProperty("thuPositionLon")]
        public float ThuPositionLon { get; set; } = 40.004241339066f;

        /// <summary>
        /// 清华在地图中的经度
        /// </summary>
        [JsonProperty("thuPositionLat")]
        public float ThuPositionLat { get; set; } = 116.314987125576f;

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
        public float BeijingAirportPositionX { get; set; } = 70.2f;

        /// <summary>
        /// 北京国际机场在地图中的坐标Y
        /// </summary>
        [JsonProperty("beijingAirportPositionY")]
        public float BeijingAirportPositionY { get; set; } = -32.6f;

        /// <summary>
        /// 北京国际机场在地图中的纬度
        /// </summary>
        [JsonProperty("beijingAirportPositionLon")]
        public float BeijingAirportPositionLon { get; set; } = 40.05692751f;

        /// <summary>
        /// 北京国际机场在地图中的经度
        /// </summary>
        [JsonProperty("beijingAirportPositionLat")]
        public float BeijingAirportPositionLat { get; set; } = 116.59992303f;

    }

}
