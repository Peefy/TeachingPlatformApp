using Newtonsoft.Json;

namespace TeachingPlatformApp.Utils.JsonModels
{
    /// <summary>
    /// 显示字符串 标题 配置
    /// </summary>
    public class StringResource
    {
        /// <summary>
        /// 主窗口标题
        /// </summary>
        [JsonProperty("windowTitle")]
        public string WindowTitle { get; set; } = "教员台界面";

        /// <summary>
        /// 地图窗口标题
        /// </summary>
        [JsonProperty("flightMapTitle")]
        public string FlightMapTitle { get; set; } = "地图界面";

        /// <summary>
        /// 直升机名称
        /// </summary>
        [JsonProperty("helicopterName")]
        public string HelicopterName { get; set; } = "直升机";

        /// <summary>
        /// 战斗机名称
        /// </summary>
        [JsonProperty("flighterName")]
        public string FlighterName { get; set; } = "战斗机1";

        /// <summary>
        /// 战斗机名称
        /// </summary>
        [JsonProperty("flighter2Name")]
        public string Flighter2Name { get; set; } = "战斗机2";

        /// <summary>
        /// 导弹名称
        /// </summary>
        [JsonProperty("missileName")]
        public string MissileName { get; set; } = "导弹";

        /// <summary>
        /// 飞行实验名称集合
        /// </summary>
        [JsonProperty("flightExperimentNames")]
        public string[] FlightExperimentNames { get; set; } =
        {
            "起落航线",
            "航线飞行",
            "斤斗",
            "盘旋",
            "俯冲跃升",
            "仪表飞行"
        };

    }

}
