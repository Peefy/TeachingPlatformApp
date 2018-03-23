using System;
using System.Windows;

using DuGu.NetFramework.Services;

using TeachingPlatformApp.Communications;

namespace TeachingPlatformApp.Models
{
    public class Flighter2Model : WswBaseModel
    {
        public Flighter2Model()
        {
            this.Angle = Config.MyFlighter2Info.InitYaw;
            this.IsNotOutofRoute = false;
            this.MyMapPosition = new Point()
            {
                X = Config.MyFlighter2Info.InitMyPointX,
                Y = Config.MyFlighter2Info.InitMyPointY
            };
            this.Name = Config.StringResource.Flighter2Name;
            this.WswAngleWithLocation = Config.WswData.Flighter2InitInfo;
            this.IsJudgeRoute = false;
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
            var flighter = Ioc.Get<ITranslateData>().TranslateInfo.Flighter2;
            var digit = Config.DataShowConfig.PointShowDigit;
            var x = Math.Round(flighter.X, digit);
            var y = Math.Round(flighter.Y, digit);
            var z = Math.Round(flighter.Z, digit);
            str = $"坐标:({x},{y},{z})";

            if (HasSetPoints == false)
                return str;
            if (IsJudgeRoute == true && Config.TestTrailRouteConfig.IsShowNowSetPointsIndex == true)
                return str + $";航路点: {NowSetPointsIndex + 1}";
            return str;
        }

    }
}
