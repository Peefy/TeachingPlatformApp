
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

        public override string MyMapInfoToString()
        {
            if (HasSetPoints == false)
                return base.MyMapInfoToString();
            if (IsJudgeRoute == true && Config.TestTrailRouteConfig.IsShowNowSetPointsIndex == true)
                return base.MyMapInfoToString() + $";航路点: {NowSetPointsIndex + 1}";
            return base.MyMapInfoToString();
        }

        public override string WswModelInfoToString()
        {
            if (HasSetPoints == false)
                return base.WswModelInfoToString();
            if (IsJudgeRoute == true && Config.TestTrailRouteConfig.IsShowNowSetPointsIndex == true)
                return base.WswModelInfoToString() + $";航路点: {NowSetPointsIndex + 1}";
            return base.WswModelInfoToString();
        }

    }

}
