
using System.Windows;

namespace TeachingPlatformApp.Models
{
    public class HelicopterModel : WswBaseModel
    {
        public HelicopterModel()
        {
            this.Angle = Config.MyHelicopterInfo.InitYaw;
            this.IsOutofRoute = false;
            this.MyMapPosition = new Point()
            {
                X = Config.MyHelicopterInfo.InitMyPointX,
                Y = Config.MyHelicopterInfo.InitMyPointY
            };
            this.Name = Config.StringResource.HelicopterName;
            this.WswAngleWithLocation = Config.WswData.HelicopterInitInfo;
            this.IsJudgeRoute = Config.TestTrailRouteConfig.TestSwitch == 1 ||
                Config.TestTrailRouteConfig.TestSwitch >= 2;
        }
    }

}
