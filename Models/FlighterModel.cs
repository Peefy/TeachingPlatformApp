using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using DuGu.NetFramework.Services;

using TeachingPlatformApp.Communications;
using TeachingPlatformApp.Models;
using TeachingPlatformApp.Utils;

namespace TeachingPlatformApp.Models
{
    public class FlighterModel : WswBaseModel
    {
        public FlighterModel()
        {           
            this.Angle = Config.MyFlighterInfo.InitYaw;
            this.IsNotOutofRoute = false;
            this.MyMapPosition = new Point()
            {
                X = Config.MyFlighterInfo.InitMyPointX,
                Y = Config.MyFlighterInfo.InitMyPointY
            };
            this.Name = Config.StringResource.FlighterName;
            this.WswAngleWithLocation = Config.WswData.FlighterInitInfo;
            this.IsJudgeRoute = Config.TestTrailRouteConfig.OutOfRouteTestSwitch == 0 ||
                Config.TestTrailRouteConfig.OutOfRouteTestSwitch >= 2;
        }

        public override string MyMapInfoToString()
        {
            if(HasSetPoints == false)
                return base.MyMapInfoToString();
            if (IsJudgeRoute == true && Config.TestTrailRouteConfig.IsShowNowSetPointsIndex == true)
                return base.MyMapInfoToString() + $";航路点: {NowSetPointsIndex + 1}";
            return base.MyMapInfoToString();
        }

        public override string WswModelInfoToString()
        {
            var str = "";
            var flighter = Ioc.Get<ITranslateData>().TranslateInfo.Flighter;
            var digit = Config.DataShowConfig.PointShowDigit;
            var x = Math.Round(flighter.X, digit);
            var y = Math.Round(flighter.Y, digit);
            var z = Math.Round(flighter.Z, digit);
            str =  $"坐标:({x},{y},{z})";

            if (HasSetPoints == false)
                return str;
            if (IsJudgeRoute == true && Config.TestTrailRouteConfig.IsShowNowSetPointsIndex == true)
                return str + $";航路点: {NowSetPointsIndex + 1}";
            return str;
        }

    }

}
