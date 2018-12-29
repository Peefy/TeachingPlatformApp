using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

using TeachingPlatformApp.Controls;
using TeachingPlatformApp.WswPlatform;
using TeachingPlatformApp.Utils.JsonModels;

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

        /// <summary>
        /// 配置文件静态实例
        /// </summary>
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
        [JsonProperty("myFlighertInfo")]
        public WswModelInfo MyFlighterInfo { get; set; }

        /// <summary>
        /// 我的战斗机 配置
        /// </summary>
        [JsonProperty("myFlighert2Info")]
        public WswModelInfo MyFlighter2Info { get; set; }

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
        /// 窗体UI 配置
        /// </summary>
        [JsonProperty("windowUiConfig")]
        public WindowUiConfig WindowUiConfig { get; set; }

        /// <summary>
        /// 地图 配置
        /// </summary>
        [JsonProperty("mapConfig")]
        public MapConfig MapConfig { get; set; }

        /// <summary>
        /// 空管模拟器 配置
        /// </summary>
        [JsonProperty("ATCSimulatorConfig")]
        public ATCSimulatorConfig ATCSimulatorConfig { get; set; }

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
                InitMyPointX = 10,
                InitMyPointY = 10,
                InitMyPointZ = 50,          
                PointScaleFactorX = -0.003f,
                PointScaleFactorY = 0.003f,
                PointScaleFactorZ = 0.003f,
                InitYaw = -63.35f,
                YawSign = true
            };
            this.MyFlighterInfo = new WswModelInfo()
            {
                InitMyPointX = 10,
                InitMyPointY = 10,
                InitMyPointZ = 50,
                PointScaleFactorX = -0.003f,
                PointScaleFactorY = 0.003f,
                PointScaleFactorZ = 0.003f,
                InitYaw = -63.35f,
                YawSign = false,
            };
            this.MyFlighter2Info = new WswModelInfo()
            {
                InitMyPointX = 10,
                InitMyPointY = 10,
                InitMyPointZ = 50,
                PointScaleFactorX = -0.003f,
                PointScaleFactorY = 0.003f,
                PointScaleFactorZ = 0.003f,
                InitYaw = -63.35f,
                YawSign = false,
            };
            this.MyMissileInfo = new WswModelInfo()
            {
                InitMyPointX = 10,
                InitMyPointY = 10,
                InitMyPointZ = 50,
                PointScaleFactorX = -0.003f,
                PointScaleFactorY = 0.003f,
                PointScaleFactorZ = 0.003f,
                InitYaw = -63.35f,
                YawSign = false,
            };

            this.GridAxesDrawPara = new GridAxesDrawPara();
            this.SpeechConfig = new SpeechConfig();
            this.TestTrailRouteConfig = new TestTrailRouteConfig();
            this.FlightExperimentConfig = new FlightExperimentConfig();
            this.WindowUiConfig = new WindowUiConfig();
            this.MapConfig = new MapConfig();
            this.ATCSimulatorConfig = new ATCSimulatorConfig();
        }

        /// <summary>
        /// 用json字符串更改配置
        /// </summary>
        /// <param name="jsonString"></param>
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

        /// <summary>
        /// 返回配置文件的json字符串
        /// </summary>
        /// <returns></returns>
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

}
