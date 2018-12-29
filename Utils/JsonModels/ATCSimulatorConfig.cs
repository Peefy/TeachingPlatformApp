
using Newtonsoft.Json;

namespace TeachingPlatformApp.Utils.JsonModels
{
    public class ATCSimulatorConfig
    {
        /// <summary>
        /// 空管模拟器视景服务器PC的Ip地址
        /// </summary>
        [JsonProperty("ipATCSceneServer")]
        public string IpATCSceneServer { get; set; } = "192.168.0.170";

        /// <summary>
        /// 空管模拟器视景客户端PC1的Ip地址
        /// </summary>
        [JsonProperty("ipATCSceneClient1")]
        public string IpATCSceneClient1 { get; set; } = "192.168.0.171";

        /// <summary>
        /// 空管模拟器视景客户端PC2的Ip地址
        /// </summary>
        [JsonProperty("ipATCSceneClient2")]
        public string IpATCSceneClient2 { get; set; } = "192.168.0.172";

        /// <summary>
        /// 空管模拟器视景客户端PC3的Ip地址
        /// </summary>
        [JsonProperty("ipATCSceneClient3")]
        public string IpATCSceneClient3 { get; set; } = "192.168.0.173";

        /// <summary>
        /// 空管模拟器数据适配器PC的Ip地址
        /// </summary>
        [JsonProperty("ipATCSDataAdapter")]
        public string IpATCSDataAdapter { get; set; } = "192.168.0.180";


    }

}
