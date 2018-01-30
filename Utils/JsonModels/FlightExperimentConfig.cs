﻿using Newtonsoft.Json;

namespace TeachingPlatformApp.Utils.JsonModels
{
    /// <summary>
    /// 飞行实验 配置
    /// </summary>
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