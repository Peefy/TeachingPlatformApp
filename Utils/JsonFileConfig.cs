using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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
        protected static Lazy<JsonFileConfig> _lazyInstance;

        [JsonIgnore]
        protected static Lazy<JsonFileConfig> LazyInstance =>
            _lazyInstance ?? (_lazyInstance = new Lazy<JsonFileConfig>(() => ReadFromFile(), 
                LazyThreadSafetyMode.PublicationOnly));

        public static JsonFileConfig Instance => LazyInstance.Value;

        [JsonIgnore]
        public const string FileName = "config.json";

        /// <summary>
        /// 显示字符串标题 配置
        /// </summary>
        
        [JsonProperty("stringResource")]
        public StringResource StringResource { get; set; }

        /// <summary>
        /// 通信 配置
        /// </summary>
        [JsonProperty("comConfig")]
        public ComConfig ComConfig { get; set; }

        /// <summary>
        /// 地图界面 配置
        /// </summary>
        [JsonProperty("mapGridAxesDrawPara")]
        public GridAxesDrawPara GridAxesDrawPara { get; set; }

        /// <summary>
        /// 威视微数据 配置
        /// </summary>
        [JsonProperty("wswData")]
        public WswData WswData { get; set; }

        /// <summary>
        /// 数据展示格式 配置
        /// </summary>
        [JsonProperty("dataShowConfig")]
        public DataShowConfig DataShowConfig { get; set; }

        /// <summary>
        /// 我的直升机消息 配置
        /// </summary>
        [JsonProperty("myHelicopterInfo")]
        public AirPlaneInfo MyHelicopterInfo { get; set; }

        /// <summary>
        /// 我的战斗机 配置
        /// </summary>
        [JsonProperty("myFlighterInfo")]
        public AirPlaneInfo MyFlighterInfo { get; set; }

        /// <summary>
        /// 语音配置
        /// </summary>
        [JsonProperty("speechConfig")]
        public SpeechConfig SpeechConfig { get; set; }

        /// <summary>
        /// 航线检测参数 配置
        /// </summary>
        [JsonProperty("testTrailRouteConfig")]
        public TestTrailRouteConfig TestTrailRouteConfig { get; set; }

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
            
            this.MyHelicopterInfo = new AirPlaneInfo()
            {
                InitMyPointX = 15,
                InitMyPointY = 15,
                InitMyPointZ = 50,          
                PointScaleFactorX = -0.01f,
                PointScaleFactorY = 0.01f,
                PointScaleFactorZ = 0.01f,
                InitYaw = -180,
                YawSign = true
            };
            this.MyFlighterInfo = new AirPlaneInfo()
            {
                InitMyPointX = 10,
                InitMyPointY = 10,
                InitMyPointZ = 50,            
                PointScaleFactorX = -0.01f,
                PointScaleFactorY = 0.01f,
                PointScaleFactorZ = 0.01f,
                InitYaw = 270,
                YawSign = false,
            };
            
            this.GridAxesDrawPara = new GridAxesDrawPara();
            this.SpeechConfig = new SpeechConfig();
            this.TestTrailRouteConfig = new TestTrailRouteConfig();
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
        public int SelfPort { get; set; } = 16000;

        [JsonProperty("udp720Port")]
        public int Udp720Port { get; set; } = 15000;

        [JsonProperty("ipSelf")]
        public string IpSelf { get; set; } = "192.168.0.135";

        [JsonProperty("ip720Platform")]
        public string Ip720Platform { get; set; } = "192.168.0.134";

        [JsonProperty("ipWswUdpServer")]
        public string IpWswUdpServer { get; set; } = "192.168.0.131";

        [JsonProperty("ipGunBarrel")]
        public string IpGunBarrel { get; set; } = "192.168.0.133";

        [JsonProperty("wswUdpPort")]
        public int WswUdpPort { get; set; } = 14000;

        [JsonProperty("udp720TechingPort")]
        public int Udp720TechingPort { get; set; } = 12000;

        [JsonProperty("udp720TestConsolePort")]
        public int Udp720TestConsolePort { get; set; } = 11000;

        [JsonProperty("renewUIRecieveCount")]
        public int RenewUIRecieveCount { get; set; } = 10;

        [JsonProperty("udpClientExtraPort")]
        public int UdpClientExtraPort { get; set; } = 10000;

    }

    public class DataShowConfig
    {
        [JsonProperty("angleShowDigit")]
        public int AngleShowDigit { get; set; } = 1;

        [JsonProperty("pointShowDigit")]
        public int PointShowDigit { get; set; } = 1;

        [JsonProperty("mapUiRefreshMs")]
        public int MapUiRefreshMs { get; set; } = 30;

        [JsonProperty("drawTrailPointNumUp")]
        public int DrawTrailPointNumUp { get; set; } = 10000;

        [JsonProperty("setPointsFontSize")]
        public double SetPointsFontSize { get; set; } = 24;

        [JsonProperty("setPointsEllipseRadius")]
        public double SetPointsEllipseRadius { get; set; } = 6;

        [JsonProperty("locationStringFontSize")]
        public double LocationStringFontSize { get; set; } = 22;

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
            FlighterInitInfo.X = -2185907.3768;
            FlighterInitInfo.Y = 4365171.6876;
            FlighterInitInfo.Z = 4104669.1241;
            FlighterInitInfo.Yaw = 0.1873927;
            FlighterInitInfo.Pitch = 0.0011897;
            FlighterInitInfo.Roll = 0.0013987;
            HelicopterInitInfo = default(AngleWithLocation);
            HelicopterInitInfo.X = -2165844.6698;
            HelicopterInitInfo.Y = 4379369.0368;
            HelicopterInitInfo.Z = 4104669.0426;
            HelicopterInitInfo.Yaw = -0.5154596;
            HelicopterInitInfo.Pitch = 0.0011189;
            HelicopterInitInfo.Roll = 0.00139873;
        }

    }

    public class SpeechConfig
    {
        [JsonProperty("volume")]
        public double Volume { get; set; } = 70;

        [JsonProperty("rate")]
        public double Rate { get; set; } = 0;

        [JsonProperty("isUsingDotnetSpeech")]
        public bool IsUsingDotnetSpeech { get; set; } = true;

        [JsonProperty("speechEnable")]
        public bool SpeechEnable { get; set; } = false;

        [JsonProperty("speechTextOutofRouteLeft")]
        public string SpeechTextOutofRoute { get; set; } = "偏离航线";

        [JsonProperty("speechTextReserved1")]
        public string SpeechTextReserved1 { get; set; } = "SpeechTextReserved1";

        [JsonProperty("speechTextReserved2")]
        public string SpeechTextReserved2 { get; set; } = "SpeechTextReserved2";

        [JsonProperty("speechTextReserved3")]
        public string SpeechTextReserved3 { get; set; } = "SpeechTextReserved3";
    }

    public class TestTrailRouteConfig
    {
        /// <summary>
        /// 0 代表只检测战斗机，1代表只检测直升机，2代表du检测
        /// </summary>
        [JsonProperty("TestSwitch")]
        public int TestSwitch { get; set; } = 2;

        [JsonProperty("outOfRouteAngle")]
        public double OutOfRouteAngle { get; set; } = 20.0;

        [JsonProperty("outOfRouteDistance")]
        public double OutOfRouteDistance { get; set; } = 10.0;

        [JsonProperty("outOfRouteTestIntervalMs")]
        public int OutOfRouteTestIntervalMs { get; set; } = 100;

        [JsonProperty("outOfRouteSpeechUpCount")]
        public int OutOfRouteSpeechUpCount { get; set; } = 10;
     
    }

}
