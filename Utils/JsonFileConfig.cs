using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using TeachingPlatformApp.Controls;
using TeachingPlatformApp.WswPlatform;

namespace TeachingPlatformApp.Utils
{
    public class JsonFileConfig 
    {
        [JsonIgnore]
        private static JsonFileConfig _instance;

        [JsonIgnore]
        public static JsonFileConfig Instance =>
            _instance ?? (_instance = ReadFromFile());

        [JsonIgnore]
        public const string FileName = "config.json";

        public StringResource StringResource { get; set; } 

        public ComConfig ComConfig { get; set; }

        public GridAxesDrawPara GridAxesDrawPara { get; set; }

        public WswData WswData { get; set; }

        public void WriteToFile()
        {
            try
            {
                var str = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(FileName, str);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static JsonFileConfig ReadFromFile()
        {
            try
            {
                var str = File.ReadAllText(FileName);
                var config = JsonConvert.DeserializeObject<JsonFileConfig>(str);
                return config;
            }
            catch (Exception)
            {
                var config = new JsonFileConfig();
                config.WriteToFile();
                return new JsonFileConfig();
            }
        }

        public JsonFileConfig()
        {
            this.StringResource = new StringResource();
            this.ComConfig = new ComConfig();
            this.WswData = new WswData();
            GridAxesDrawPara = new GridAxesDrawPara();
        }

    }

    public class StringResource
    {
        [JsonProperty("windowTitle")]
        public string WindowTitle { get; set; } = "教员台界面";

        [JsonProperty("flightMapTitle")]
        public string FlightMapTitle { get; set; } = "地图界面";

        [JsonProperty("helicopterName")]
        public string HelicopterName { get; set; } = "直升机";

        [JsonProperty("flighterName")]
        public string FlighterName { get; set; } = "战斗机";
    }

    public class ComConfig
    {
        [JsonProperty("selfPort")]
        public int SelfPort { get; set; } = 15000;

        [JsonProperty("udp720Port")]
        public int Udp720Port { get; set; } = 16000;

        [JsonProperty("ipSelf")]
        public string IpSelf { get; set; } = "192.168.0.132";

        [JsonProperty("ip720Platform")]
        public string Ip720Platform { get; set; } = "192.168.0.134";

        [JsonProperty("ipWswUdpServer")]
        public string IpWswUdpServer { get; set; } = "192.168.0.131";

        [JsonProperty("ipGunBarrel")]
        public string IpGunBarrel { get; set; } = "192.168.0.133";

        [JsonProperty("wswUdpPort")]
        public int WswUdpPort { get; set; } = 15000;
    }

    public class WswData
    {
        /// <summary>
        /// flighterInitInfo
        /// </summary>
        [JsonProperty("flighterInitInfo")]
        public AngleWithLocation FlighterInitInfo;

        /// <summary>
        /// helicopterInitInfo
        /// </summary>
        [JsonProperty("helicopterInitInfo")]
        public AngleWithLocation HelicopterInitInfo;

        public WswData()
        {
            FlighterInitInfo = default(AngleWithLocation);
            FlighterInitInfo.X = -2169349.3768;
            FlighterInitInfo.Y = 4386443.6876;
            FlighterInitInfo.Z = 4106805.1241;
            FlighterInitInfo.Yaw = 0.1873927;
            FlighterInitInfo.Pitch = 0.0011897;
            FlighterInitInfo.Roll = 0.0013987;
            HelicopterInitInfo = default(AngleWithLocation);
            HelicopterInitInfo.X = -2166765.6698;
            HelicopterInitInfo.Y = 4381229.0368;
            HelicopterInitInfo.Z = 4101920.0426;
            HelicopterInitInfo.Yaw = -0.5154596;
            HelicopterInitInfo.Pitch = 0.0011189;
            HelicopterInitInfo.Roll = 0.00139873;
        }

    }

}
