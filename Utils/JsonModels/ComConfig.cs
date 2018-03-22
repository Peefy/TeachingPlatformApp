
using Prism.Mvvm;
using Newtonsoft.Json;

namespace TeachingPlatformApp.Utils.JsonModels
{
    /// <summary>
    /// 通信 配置
    /// </summary>
    public class ComConfig : BindableBase
    {
        /// <summary>
        /// 自己PC设备Udp的监听端口号
        /// </summary>
        [JsonProperty("selfPort")]
        public int SelfPort { get; set; } = 16000;

        /// <summary>
        /// 720度平台Unity控制软件监听端口号
        /// </summary>
        [JsonProperty("udp720Port")]
        public int Udp720Port { get; set; } = 15000;

        /// <summary>
        /// 自己PC设备Udp的Ip地址
        /// </summary>
        [JsonProperty("ipSelf")]
        public string IpSelf { get; set; } = "192.168.0.135";

        /// <summary>
        /// 720度平台PC的Ip地址
        /// </summary>
        [JsonProperty("ip720Platform")]
        public string Ip720Platform { get; set; } = "192.168.0.134";

        /// <summary>
        /// 第二个720度平台PC的Ip地址
        /// </summary>
        [JsonProperty("ip720Platform2")]
        public string Ip720Platform2 { get; set; } = "192.168.0.132";

        /// <summary>
        /// Wsw视景软件UdpServer(六自由度平台)PC的Ip地址
        /// </summary>
        [JsonProperty("ipWswUdpServer")]
        public string IpWswUdpServer { get; set; } = "192.168.0.131";

        /// <summary>
        /// 单兵平台PC的Ip地址
        /// </summary>
        [JsonProperty("ipGunBarrel")]
        public string IpGunBarrel { get; set; } = "192.168.0.133";

        /// <summary>
        /// 所有Wsw视景软件Udp的监听端口号
        /// </summary>
        [JsonProperty("wswUdpPort")]
        public int WswUdpPort { get; set; } = 14000;

        /// <summary>
        /// 720度平台Unity控制软件接收实验指令Udp监听的端口号
        /// </summary>
        [JsonProperty("udp720TechingPort")]
        public int Udp720TechingPort { get; set; } = 12000;

        /// <summary>
        /// 720度平台Console测试软件Udp监听的端口号
        /// </summary>
        [JsonProperty("udp720TestConsolePort")]
        public int Udp720TestConsolePort { get; set; } = 11000;

        /// <summary>
        /// 收到Wsw软件RenewUIRecieveCount次数据后更新一次UI，防止更新太快UI太卡
        /// </summary>
        [JsonProperty("renewUIRecieveCount")]
        public int RenewUIRecieveCount { get; set; } = 10;

        /// <summary>
        /// 教研台软件额外Udp监听的端口号，用于测试集成和扩展Api
        /// </summary>
        [JsonProperty("udpClientExtraPort")]
        public int UdpClientExtraPort { get; set; } = 10000;

    }

}
