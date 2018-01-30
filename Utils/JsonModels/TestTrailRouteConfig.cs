using Newtonsoft.Json;

namespace TeachingPlatformApp.Utils.JsonModels
{
    /// <summary>
    /// 航线检测 配置
    /// </summary>
    public class TestTrailRouteConfig
    {
        /// <summary>
        /// 0 代表只检测战斗机，1代表只检测直升机，2代表du检测
        /// </summary>
        [JsonProperty("outOfRouteTestSwitch")]
        public int OutOfRouteTestSwitch { get; set; } = 0;

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
        public int AutoFollowingIntervalMs { get; set; } = 50;

        /// <summary>
        /// 检测当前经过的航路点索引合理半径
        /// </summary>
        [JsonProperty("judgeNowSetPointsIndexRadius")]
        public double JudgeNowSetPointsIndexRadius { get; set; } = 5.0;

        /// <summary>
        /// 初始的航路点索引
        /// </summary>
        [JsonProperty("initNowSetPointsIndex")]
        public int InitNowSetPointsIndex { get; set; } = -1;

        /// <summary>
        /// 地图界面是否显示飞机上一次经过的航路点
        /// </summary>
        [JsonProperty("isShowNowSetPointsIndex")]
        public bool IsShowNowSetPointsIndex { get; set; } = false;

        [JsonProperty("unConnectedFlighterRotateSpeed")]
        public float UnConnectedFlighterRotateSpeed { get; set; } = 0.5f;

    }

}
