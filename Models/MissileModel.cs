
using System.Windows;

namespace TeachingPlatformApp.Models
{
    public class MissileModel : WswBaseModel
    {



        public MissileModel()
        {
            this.Angle = Config.MyMissileInfo.InitYaw;
            this.IsOutofRoute = false;
            this.MyMapPosition = new Point()
            {
                X = Config.MyMissileInfo.InitMyPointX,
                Y = Config.MyMissileInfo.InitMyPointY
            };
            this.Name = Config.StringResource.MissileName;
            this.WswAngleWithLocation = Config.WswData.MissileInitInfo;
            this.IsJudgeRoute = false;
        }
    }

}
