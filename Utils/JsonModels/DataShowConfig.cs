using Newtonsoft.Json;

namespace TeachingPlatformApp.Utils.JsonModels
{
    /// <summary>
    /// 数据展示 配置
    /// </summary>
    public class DataShowConfig
    {
        /// <summary>
        /// 角度显示数据保留几位小数位数
        /// </summary>
        [JsonProperty("angleShowDigit")]
        public int AngleShowDigit { get; set; } = 1;

        /// <summary>
        /// 坐标显示数据保留几位小数位数
        /// </summary>
        [JsonProperty("pointShowDigit")]
        public int PointShowDigit { get; set; } = 1;

        /// <summary>
        /// 地图界面刷新周期
        /// </summary>
        [JsonProperty("mapUiRefreshMs")]
        public int MapUiRefreshMs { get; set; } = 50;

        /// <summary>
        /// 灰色航迹记录点数上限，超过后自动清空
        /// </summary>
        [JsonProperty("drawTrailPointNumUp")]
        public int DrawTrailPointNumUp { get; set; } = 5000;

        /// <summary>
        /// 航路点汉字字体大小
        /// </summary>
        [JsonProperty("setPointsFontSize")]
        public double SetPointsFontSize { get; set; } = 24;

        /// <summary>
        /// 航路点紫色圆圈半径
        /// </summary>
        [JsonProperty("setPointsEllipseRadius")]
        public double SetPointsEllipseRadius { get; set; } = 6;

        /// <summary>
        /// 位置信息字体大小
        /// </summary>
        [JsonProperty("locationStringFontSize")]
        public double LocationStringFontSize { get; set; } = 22;

        /// <summary>
        /// 航路点连线粗细
        /// </summary>
        [JsonProperty("setPointsLineWidth")]
        public double SetPointsLineWidth { get; set; } = 3;

        /// <summary>
        /// 没有接收到Wsw软件的Udp数据时，角度和坐标显示的初始值
        /// </summary>
        [JsonProperty("unRecieveduUdpDataShow")]
        public float UnRecieveduUdpDataShow { get; set; } = 0.0f;

    }

}
