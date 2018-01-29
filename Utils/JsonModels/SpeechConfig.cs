using Newtonsoft.Json;

namespace TeachingPlatformApp.Utils.JsonModels
{
    /// <summary>
    /// 语音 配置
    /// </summary>
    public class SpeechConfig
    {
        /// <summary>
        /// 音量
        /// </summary>
        [JsonProperty("volume")]
        public double Volume { get; set; } = 70;

        /// <summary>
        /// 语速
        /// </summary>
        [JsonProperty("rate")]
        public double Rate { get; set; } = 2.0;

        /// <summary>
        /// 是否采用DotNetSpeech.dll产生语音
        /// </summary>
        [JsonProperty("isUsingDotnetSpeech")]
        public bool IsUsingDotnetSpeech { get; set; } = true;

        /// <summary>
        /// 是否发出语音
        /// </summary>
        [JsonProperty("speechEnable")]
        public bool SpeechEnable { get; set; } = false;

        /// <summary>
        /// 偏离航线语音
        /// </summary>
        [JsonProperty("speechTextOutofRouteLeft")]
        public string SpeechTextOutofRoute { get; set; } = "偏离航线";

        /// <summary>
        /// 保留语音1
        /// </summary>
        [JsonProperty("speechTextReserved1")]
        public string SpeechTextReserved1 { get; set; } = "SpeechTextReserved1";

        /// <summary>
        /// 保留语音2
        /// </summary>
        [JsonProperty("speechTextReserved2")]
        public string SpeechTextReserved2 { get; set; } = "SpeechTextReserved2";

        /// <summary>
        /// 保留语音3
        /// </summary>
        [JsonProperty("speechTextReserved3")]
        public string SpeechTextReserved3 { get; set; } = "SpeechTextReserved3";
    }

}
