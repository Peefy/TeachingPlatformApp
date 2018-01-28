using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Mvvm;
using Newtonsoft.Json;

using TeachingPlatformApp.Controls;
using TeachingPlatformApp.WswPlatform;


namespace TeachingPlatformApp.Utils
{
    /// <summary>
    /// config.json 配置文件操作
    /// </summary>
    public class JsonFileConfig
    {
        [JsonIgnore]
        protected static Lazy<JsonFileConfig> _lazyInstance;

        [JsonIgnore]
        protected static Lazy<JsonFileConfig> LazyInstance =>
            _lazyInstance ?? (_lazyInstance = new Lazy<JsonFileConfig>(() => ReadFromFile(),
                LazyThreadSafetyMode.PublicationOnly));

        [JsonIgnore]
        public static JsonFileConfig Instance => LazyInstance.Value;

        /// <summary>
        /// 配置文件路径和文件名称
        /// </summary>
        [JsonIgnore]
        public static string PathAndFileName { get; set; } = 
            Path.Combine(Environment.CurrentDirectory, "config.json");

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
        public WswModelInfo MyHelicopterInfo { get; set; }

        /// <summary>
        /// 我的战斗机 配置
        /// </summary>
        [JsonProperty("wswModelInfo")]
        public WswModelInfo MyFlighterInfo { get; set; }

        /// <summary>
        /// 我的导弹 配置
        /// </summary>
        [JsonProperty("myMissileInfo")]
        public WswModelInfo MyMissileInfo { get; set; }

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

        /// <summary>
        /// 飞行实验介绍 配置
        /// </summary>
        [JsonProperty("flightExperimentConfig")]
        public FlightExperimentConfig FlightExperimentConfig { get;set;}

        /// <summary>
        /// 配置写入文件
        /// </summary>
        public void WriteToFile()
        {
            try
            {
                var str = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(PathAndFileName, str);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// 从文件读取配置
        /// </summary>
        /// <returns></returns>
        public static JsonFileConfig ReadFromFile()
        {
            try
            {
                var str = File.ReadAllText(PathAndFileName);
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

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public JsonFileConfig()
        {
            this.StringResource = new StringResource();
            this.ComConfig = new ComConfig();
            this.WswData = new WswData();
            this.DataShowConfig = new DataShowConfig();
            
            this.MyHelicopterInfo = new WswModelInfo()
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
            this.MyFlighterInfo = new WswModelInfo()
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
            this.MyMissileInfo = new WswModelInfo()
            {
                InitMyPointX = 50,
                InitMyPointY = 50,
                InitMyPointZ = 50,
                PointScaleFactorX = -0.01f,
                PointScaleFactorY = 0.01f,
                PointScaleFactorZ = 0.01f,
                InitYaw = 180,
                YawSign = false,
            };

            this.GridAxesDrawPara = new GridAxesDrawPara();
            this.SpeechConfig = new SpeechConfig();
            this.TestTrailRouteConfig = new TestTrailRouteConfig();
            this.FlightExperimentConfig = new FlightExperimentConfig();
        }

        public void SetConfig(string jsonString)
        {
            try
            {
                var config = JsonConvert.DeserializeObject<JsonFileConfig>(jsonString);
                _lazyInstance = new Lazy<JsonFileConfig>(() => 
                    ClassObjectDeepCloneUtil.DeepCopyUsingXmlSerialize(config),
                    LazyThreadSafetyMode.PublicationOnly);
                WriteToFile();
            }
            catch (Exception ex)
            {
                LogAndConfig.Log.Error(ex);
            }
        }

        public override string ToString()
        {
            try
            {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }
            catch (Exception ex)
            {
                return "toString() call error:" + ex.Message;
            }
            
        }

    }

    /// <summary>
    /// 显示字符串 标题 配置
    /// </summary>
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

        [JsonProperty("missileName")]
        public string MissileName { get; set; } = "导弹";
    }

    public class ComConfig : BindableBase
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
        public int DrawTrailPointNumUp { get; set; } = 5000;

        [JsonProperty("setPointsFontSize")]
        public double SetPointsFontSize { get; set; } = 24;

        [JsonProperty("setPointsEllipseRadius")]
        public double SetPointsEllipseRadius { get; set; } = 6;

        [JsonProperty("locationStringFontSize")]
        public double LocationStringFontSize { get; set; } = 22;

        [JsonProperty("setPointsLineWidth")]
        public double SetPointsLineWidth { get; set; } = 3;

        [JsonProperty("unRecieveduUdpDataShow")]
        public float UnRecieveduUdpDataShow { get; set; } = 0.0f;

    }

    /// <summary>
    /// Wsw 数据
    /// </summary>
    public class WswData
    {
        /// <summary>
        /// 战斗机的初始数据(姿态，坐标)
        /// </summary>
        [JsonProperty("flighterInitInfo")]
        public AngleWithLocation FlighterInitInfo;

        /// <summary>
        /// 直升机的初始数据(姿态，坐标)
        /// </summary>
        [JsonProperty("helicopterInitInfo")]
        public AngleWithLocation HelicopterInitInfo;

        /// <summary>
        /// 导弹的初始数据(姿态，坐标)
        /// </summary>
        [JsonProperty("missileInitInfo")]
        public AngleWithLocation MissileInitInfo;

        public WswData()
        {
            FlighterInitInfo = default;
            FlighterInitInfo.X = -2185907.3768;
            FlighterInitInfo.Y = 4365171.6876;
            FlighterInitInfo.Z = 4104669.1241;
            FlighterInitInfo.Yaw = 0.1873927;
            FlighterInitInfo.Pitch = 0.0011897;
            FlighterInitInfo.Roll = 0.0013987;
            HelicopterInitInfo = default;
            HelicopterInitInfo.X = -2165844.6698;
            HelicopterInitInfo.Y = 4379369.0368;
            HelicopterInitInfo.Z = 4104669.0426;
            HelicopterInitInfo.Yaw = -0.5154596;
            HelicopterInitInfo.Pitch = 0.0011189;
            HelicopterInitInfo.Roll = 0.00139873;
            MissileInitInfo = default;
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
        [JsonProperty("outOfRouteTestSwitch")]
        public int OutOfRouteTestSwitch { get; set; } = 2;

        /// <summary>
        /// 航线偏离检测角度
        /// </summary>
        [JsonProperty("outOfRouteAngle")]
        public double OutOfRouteAngle { get; set; } = 20.0;

        /// <summary>
        /// 航线检测偏离距离
        /// </summary>
        [JsonProperty("outOfRouteDistance")]
        public double OutOfRouteDistance { get; set; } = 6.0;

        /// <summary>
        /// 航线检测偏离周期
        /// </summary>
        [JsonProperty("outOfRouteTestIntervalMs")]
        public int OutOfRouteTestIntervalMs { get; set; } = 300;

        /// <summary>
        /// 航线检测偏离检测次数
        /// </summary>
        [JsonProperty("outOfRouteSpeechUpCount")]
        public int OutOfRouteSpeechUpCount { get; set; } = 2;

        /// <summary>
        /// 0 代表不跟随，1 代表跟随战斗机 2代表跟随直升机
        /// </summary>
        [JsonProperty("isAutoFollowing")]
        public int IsAutoFollowing { get; set; } = 1;

        /// <summary>
        /// 自动跟随周期
        /// </summary>
        [JsonProperty("autoFollowingIntervalMs")]
        public int AutoFollowingIntervalMs { get; set; } = 30;

        /// <summary>
        /// 检测当前经过的航路点索引合理半径
        /// </summary>
        [JsonProperty("judgeNowSetPointsIndexRadius")]
        public double JudgeNowSetPointsIndexRadius { get; set; } = 5.0;

        /// <summary>
        /// 初始的航路点索引
        /// </summary>
        [JsonProperty("initNowSetPointsIndex")]
        public int InitNowSetPointsIndex { get; set; } = 0;

        [JsonProperty("isShowNowSetPointsIndex")]
        public bool IsShowNowSetPointsIndex { get; set; } = false;

    }

    public class FlightExperimentConfig
    {
        /// <summary>
        /// 飞行实验介绍
        /// </summary>
        [JsonProperty("introductions")]
        public string[] Introductions { get; set; } =
        {
            "    1.起落航线实验：飞行学员驾驶飞行模拟器围绕显示器视景中的T字型标记物，" +
                "巡航飞行路线形状为一个将T字型标记物包围五边形。" +
                "考核指标是将飞机坡度保持在一定范围内并平稳起落降在指定范围内。" +
                "教员台控制界面可以实时记录飞机的滚转角，可以发出收放起落架和转弯的提示音，" +
                "可以设置T字型标记物和飞行五边形的坐标点。",
            "    2.航线飞行实验：飞行学员根据教员台控制系统设置的航路点驾驶飞行模拟器飞行。" +
                "考核指标是实时采集并记录飞机实际飞行航线与预置的航线进行对比判别，" +
                "当实际航线与设置航线保持在一定范围内考核通过。可以教员台控制界面" +
                "设置飞行的航路点坐标和实时记录坐标点。",
            "    3.斤斗实验：学员驾驶模拟歼击机飞行器在铅垂平面进行360度翻转并同时在飞行过程中将坡度" +
                "和偏航角保持在一定范围内。考核指标是飞机在斤斗前段，" +
                "俯仰角经过从0度到180度的渐变过程；在斤斗后段，" +
                "俯仰角经过180度再到0度的渐变过程。教员台控制界面可以预设" +
                "斤斗实验过程中坡度和偏航角的保持范围，并实时记录飞机俯仰角、偏航角和滚转角。",
            "    4.盘旋实验：飞行学员驾驶飞行模拟器在水平面分别以45度和60度的坡度进行盘旋" +
                "并将高度保持在一定范围内。考核指标是飞机在盘旋前段，偏航角经过从0度到180度过程，" +
                "在盘旋后段偏航角经过从180度再到0度的渐变过程；" +
                "同时使飞机坡度和飞行高度保持在一定范围内。" +
                "可以通过教员台界面设置飞机需要保持的坡度和飞行高度并同时" +
                "检测飞机盘旋过程中偏航角的变化是否符合要求并可以实时查看飞机偏航角、" +
                "滚转角和飞机坐标点。",
            "    5.俯冲跃升实验：飞行学员驾驶飞行模拟器进行俯冲跃升实验，" +
                "并在飞行过程中将坡度保持在一定范围内。考核指标是使飞机的坡度保持" +
                "在一定范围内并且飞机在俯冲、跃升的过程中俯仰角保持在一定范围内。" +
                "通过教员台界面可以实时记录飞机滚转角和俯仰角，并且可以设置在实" +
                "验中飞机需要保持的坡度范围指标。",
            "    6.仪表飞行实验：飞行学员驾驶飞行模拟器在夜景下只通过仪表显示的" +
                "内容并保持预设的坡度且根据预设的航路点进行飞行。" +
                "考核指标是是否按照既定的航线和保持坡度进行飞行。" +
                "通过教员台界面可以设置保持的坡度大小以及航路点坐标，" +
                "且在教员台界面上可以实时显示飞机的滚转角和飞机当前飞行的坐标点。",
        };

        /// <summary>
        /// 是否检测飞行实验
        /// </summary>
        [JsonProperty("isJudgeValid ")]
        public bool IsJudgeValid { get; set; } = true;

    }

}
