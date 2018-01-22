using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace TeachingPlatformApp.WswPlatform
{
    public class AirPlaneInfo
    {
        [JsonProperty("initMyPointX")]
        public float InitMyPointX { get; set; }

        [JsonProperty("initMyPointY")]
        public float InitMyPointY { get; set; }

        [JsonProperty("initMyPointZ")]
        public float InitMyPointZ { get; set; }

        [JsonProperty("pointScaleFactorX")]
        public float PointScaleFactorX { get; set; }

        [JsonProperty("pointScaleFactorY")]
        public float PointScaleFactorY { get; set; }

        [JsonProperty("pointScaleFactorZ")]
        public float PointScaleFactorZ { get; set; }

        [JsonProperty("initYaw")]
        public float InitYaw { get; set; }
    }
}
