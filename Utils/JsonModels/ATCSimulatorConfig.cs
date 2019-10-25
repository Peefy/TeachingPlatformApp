
using Newtonsoft.Json;

namespace TeachingPlatformApp.Utils.JsonModels
{
    public class ATCSimulatorConfig
    {
        /// <summary>
        /// 空管模拟器数据适配器PC的Ip地址
        /// </summary>
        [JsonProperty("aTCSimulatorIp")]
        public string ATCSimulatorIp { get; set; } = "192.168.0.100";

        /// <summary>
        /// 空管模拟器雷达模拟器PC的Ip地址
        /// </summary>
        [JsonProperty("aTCSimulatorRadarIp")]
        public string ATCSimulatorRadarIp { get; set; } = "192.168.0.101";

        /// <summary>
        /// 空管模拟器视景客户端PC1的Ip地址
        /// </summary>
        [JsonProperty("aTCSimulatorSim1Ip")]
        public string ATCSimulatorSim1Ip { get; set; } = "192.168.0.201";

        /// <summary>
        /// 空管模拟器视景客户端PC2的Ip地址
        /// </summary>
        [JsonProperty("aTCSimulatorSim2Ip")]
        public string ATCSimulatorSim2Ip { get; set; } = "192.168.0.202";

        /// <summary>
        /// 空管模拟器视景客户端PC3的Ip地址
        /// </summary>
        [JsonProperty("aTCSimulatorSim3Ip")]
        public string ATCSimulatorSim3Ip { get; set; } = "192.168.0.203";

        /// <summary>
        /// 空管模拟器数据适配器PC的端口号
        /// </summary>
        [JsonProperty("aTCSimulatorPort")]
        public int ATCSimulatorPort { get; set; } = 7109;


    }

}
