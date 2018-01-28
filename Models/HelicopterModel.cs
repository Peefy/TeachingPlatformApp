
using System.Windows;

namespace TeachingPlatformApp.Models
{
    public class HelicopterModel : WswBaseModel
    {
        public HelicopterModel()
        {
            this.Angle = Config.MyHelicopterInfo.InitYaw;
            this.IsNotOutofRoute = false;
            this.MyMapPosition = new Point()
            {
                X = Config.MyHelicopterInfo.InitMyPointX,
                Y = Config.MyHelicopterInfo.InitMyPointY
            };
            this.Name = Config.StringResource.HelicopterName;
            this.WswAngleWithLocation = Config.WswData.HelicopterInitInfo;
            this.IsJudgeRoute = Config.TestTrailRouteConfig.OutOfRouteTestSwitch == 1 ||
                Config.TestTrailRouteConfig.OutOfRouteTestSwitch >= 2;
        }
    }

}
