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

        [JsonProperty("stringResource")]
        public StringResource StringResource { get; set; }

        [JsonProperty("comConfig")]
        public ComConfig ComConfig { get; set; }

        [JsonProperty("mapGridAxesDrawPara")]
        public GridAxesDrawPara GridAxesDrawPara { get; set; }

        [JsonProperty("wswData")]
        public WswData WswData { get; set; }

        [JsonProperty("dataShowConfig")]
        public DataShowConfig DataShowConfig { get; set; }

        [JsonProperty("myHelicopterInfo")]
        public AirPlaneInfo MyHelicopterInfo { get; set; }

        [JsonProperty("myFlighterInfo")]
        public AirPlaneInfo MyFlighterInfo { get; set; }

        [JsonProperty("speechConfig")]
        public SpeechConfig SpeechConfig { get; set; }

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
            this.DataShowConfig = new DataShowConfig();
            this.MyFlighterInfo = new AirPlaneInfo()
            {
                InitMyPointX = 10,
                InitMyPointY = 10,
                InitMyPointZ = 0,
                InitYaw = 180,
                PointScaleFactorX = 0.5f,
                PointScaleFactorY = 0.5f,
                PointScaleFactorZ = 0.5f,
            };
            this.MyHelicopterInfo = new AirPlaneInfo()
            {
                InitMyPointX = 40,
                InitMyPointY = 40,
                InitMyPointZ = 0,
                InitYaw = 0,
                PointScaleFactorX = 0.5f,
                PointScaleFactorY = 0.5f,
                PointScaleFactorZ = 0.5f,
            };
            this.GridAxesDrawPara = new GridAxesDrawPara();
            this.SpeechConfig = new SpeechConfig();
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

        [JsonProperty("udp720TechingPort")]
        public int Udp720TechingPort { get; set; } = 12000;

        [JsonProperty("udp720TestConsolePort")]
        public int Udp720TestConsolePort { get; set; } = 11000;

        [JsonProperty("renewUIRecieveCount")]
        public int RenewUIRecieveCount { get; set; } = 10;

    }

    public class DataShowConfig
    {
        [JsonProperty("angleShowDigit")]
        public int AngleShowDigit { get; set; } = 2;

        [JsonProperty("pointShowDigit")]
        public int PointShowDigit { get; set; } = 2;

        [JsonProperty("mapUiRefreshMs")]
        public int MapUiRefreshMs { get; set; } = 30;

        [JsonProperty("drawTrailPointNumUp")]
        public int DrawTrailPointNumUp { get; set; } = 10000;

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

    public class SpeechConfig
    {
        [JsonProperty("speechTextOutofRouteLeft")]
        public string SpeechTextOutofRouteLeft { get; set; } = "您已经向左偏离航线";

        [JsonProperty("speechTextOutofRouteRight")]
        public string SpeechTextOutofRouteRight { get; set; } = "您已经向右偏离航线";

        [JsonProperty("speechTextReserved1")]
        public string SpeechTextReserved1 { get; set; } = "SpeechTextReserved1";

        [JsonProperty("speechTextReserved2")]
        public string SpeechTextReserved2 { get; set; } = "SpeechTextReserved2";

        [JsonProperty("speechTextReserved3")]
        public string SpeechTextReserved3 { get; set; } = "SpeechTextReserved3";
    }

}
