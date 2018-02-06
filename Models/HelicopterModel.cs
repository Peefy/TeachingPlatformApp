
using DuGu.NetFramework.Services;
using System;
using System.Windows;
using TeachingPlatformApp.Communications;

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
            var str = "";
            var heli = Ioc.Get<ITranslateData>().TranslateInfo.Helicopter;
            var digit = Config.DataShowConfig.PointShowDigit;
            var x = Math.Round(heli.X, digit);
            var y = Math.Round(heli.Y, digit);
            var z = Math.Round(heli.Z, digit);
            str = $"坐标:({x},{y},{z})";

            if (HasSetPoints == false)
                return str;
            if (IsJudgeRoute == true && Config.TestTrailRouteConfig.IsShowNowSetPointsIndex == true)
                return str + $";航路点: {NowSetPointsIndex + 1}";
            return str;
        }

    }

}
