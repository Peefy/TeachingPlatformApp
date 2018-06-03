using System;
using System.Net;

using TeachingPlatformApp.Utils.JsonModels;
using TeachingPlatformApp.Utils;

namespace TeachingPlatformApp.WswPlatform
{
    public static class WswHelper
    {
        /// <summary>
        /// Wsw数据ToString()
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static string AngleWithLocationToString(AngleWithLocation angle) => 
            $"x:{angle.X} y:{angle.Y} z:{angle.Z} roll:{angle.Roll} yaw:{angle.Yaw} pitch:{angle.Pitch}";

        /// <summary>
        /// Wsw数据四舍五入digit位
        /// </summary>
        /// <param name="angleWithLocation"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static AngleWithLocation MathRound(AngleWithLocation angleWithLocation, int digit)
        {
            return new AngleWithLocation
            {
                X = (float)Math.Round(angleWithLocation.X, digit),
                Y = (float)Math.Round(angleWithLocation.Y, digit),
                Z = (float)Math.Round(angleWithLocation.Z, digit),
                Roll = (float)Math.Round(angleWithLocation.Roll, digit),
                Yaw = (float)Math.Round(angleWithLocation.Yaw, digit),
                Pitch = (float)Math.Round(angleWithLocation.Pitch, digit),
            };
        }

        /// <summary>
        /// 将Wsw坐标变换为地图坐标
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="wswAirplane"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static AngleWithLocation DealWswAngleToMyMapData(AngleWithLocation angle, WswModelKind wswAirplane, int digit = 2)
        {
            var config = JsonFileConfig.Instance;
            var wswInitData = config.WswData;
            var angleNew = new AngleWithLocation();
            if(wswAirplane == WswModelKind.Flighter)
            {
                var myFlighterInfo = config.MyFlighterInfo;
                var yawSign = (myFlighterInfo.YawSign == true) ? 1 : -1;
                angleNew.X = (angle.X - wswInitData.FlighterInitInfo.X) *
                    myFlighterInfo.PointScaleFactorX + myFlighterInfo.InitMyPointX;
                angleNew.Y = (angle.Y - wswInitData.FlighterInitInfo.Y) *
                    myFlighterInfo.PointScaleFactorY + myFlighterInfo.InitMyPointY;
                angleNew.Z = (angle.Z - wswInitData.FlighterInitInfo.Z) *
                    myFlighterInfo.PointScaleFactorZ + myFlighterInfo.InitMyPointZ;
                angleNew.Roll = angle.Roll;
                angleNew.Pitch = angle.Pitch;              
                angleNew.Yaw = angle.Yaw * yawSign - myFlighterInfo.InitYaw;

            };
            if(wswAirplane == WswModelKind.Helicopter)
            {
                var myHelicopterInfo = config.MyHelicopterInfo;
                var yawSign = myHelicopterInfo.YawSign == true ? 1 : -1;
                angleNew.X = (angle.X - wswInitData.HelicopterInitInfo.X) *
                    myHelicopterInfo.PointScaleFactorX + myHelicopterInfo.InitMyPointX;
                angleNew.Y = (angle.Y - wswInitData.HelicopterInitInfo.Y) *
                    myHelicopterInfo.PointScaleFactorY + myHelicopterInfo.InitMyPointY;
                angleNew.Z = (angle.Z - wswInitData.HelicopterInitInfo.Z) *
                    myHelicopterInfo.PointScaleFactorZ + myHelicopterInfo.InitMyPointZ;
                angleNew.Roll = angle.Roll;
                angleNew.Pitch = angle.Pitch;
                angleNew.Yaw = angle.Yaw * yawSign - myHelicopterInfo.InitYaw;
            }
            if(wswAirplane == WswModelKind.Flighter2)
            {
                var myFlighter2Info = config.MyFlighter2Info;
                var yawSign = (myFlighter2Info.YawSign == true) ? 1 : -1;
                angleNew.X = (angle.X - wswInitData.Flighter2InitInfo.X) *
                    myFlighter2Info.PointScaleFactorX + myFlighter2Info.InitMyPointX;
                angleNew.Y = (angle.Y - wswInitData.Flighter2InitInfo.Y) *
                    myFlighter2Info.PointScaleFactorY + myFlighter2Info.InitMyPointY;
                angleNew.Z = (angle.Z - wswInitData.Flighter2InitInfo.Z) *
                    myFlighter2Info.PointScaleFactorZ + myFlighter2Info.InitMyPointZ;
                angleNew.Roll = angle.Roll;
                angleNew.Pitch = angle.Pitch;
                angleNew.Yaw = angle.Yaw * yawSign - myFlighter2Info.InitYaw;
            }
            if(wswAirplane == WswModelKind.Missile)
            {

            }
            angleNew.X = Math.Round(angleNew.X, digit);
            angleNew.Y = Math.Round(angleNew.Y, digit);
            angleNew.Z = Math.Round(angleNew.Z, digit);
            angleNew.Roll = Math.Round(angleNew.Roll, digit);
            angleNew.Yaw = Math.Round(angleNew.Yaw, digit);
            angleNew.Pitch = Math.Round(angleNew.Pitch, digit);

            angleNew.Yaw = NumberUtil.PutAngleIn(angleNew.Yaw);

            return angleNew;
        }

        /// <summary>
        /// 将地图坐标变换为Wsw坐标
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="wswAirplane"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static AngleWithLocation DealMyMapDataToWswAngle(double x, double y, double z, WswModelKind wswAirplane, int digit = 2)
        {
            var config = JsonFileConfig.Instance;
            var wswInitData = config.WswData;
            var angleNew = new AngleWithLocation();
            WswModelInfo myInfo = default;
            AngleWithLocation wswInfo = default;
            if (wswAirplane == WswModelKind.Flighter)
            {
                myInfo = config.MyFlighterInfo;
                wswInfo = wswInitData.FlighterInitInfo;
            };
            if (wswAirplane == WswModelKind.Helicopter)
            {
                myInfo = config.MyHelicopterInfo;
                wswInfo = wswInitData.HelicopterInitInfo;
            }
            if (wswAirplane == WswModelKind.Flighter2)
            {
                myInfo = config.MyFlighter2Info;
                wswInfo = wswInitData.Flighter2InitInfo;               
            }
            if (wswAirplane == WswModelKind.Missile)
            {
                myInfo = config.MyMissileInfo;
                wswInfo = wswInitData.MissileInitInfo;
            }
            angleNew.X = (x - myInfo.InitMyPointX) / myInfo.PointScaleFactorX
                    + wswInfo.X;
            angleNew.Y = (y - myInfo.InitMyPointY) / myInfo.PointScaleFactorY
                + wswInfo.Y;
            angleNew.Z = (z - myInfo.InitMyPointZ) / myInfo.PointScaleFactorZ
                + wswInfo.Z;
            angleNew.X = Math.Round(angleNew.X, digit);
            angleNew.Y = Math.Round(angleNew.Y, digit);
            angleNew.Z = Math.Round(angleNew.Z, digit);
            return angleNew;
        }

        /// <summary>
        /// 计算Wsw数据坐标的增量
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="angleWithLocation"></param>
        /// <returns></returns>
        public static string GetWswAngleWithLocationDeltaXY(string ip, AngleWithLocation angleWithLocation)
        {
            var deltaX = 0.0;
            var deltaY = 0.0;
            var config = JsonFileConfig.Instance;
            if(ip.StartsWith(config.ComConfig.Ip720Platform))
            {
                deltaX = angleWithLocation.X - config.WswData.FlighterInitInfo.X;
                deltaY = angleWithLocation.Y - config.WswData.FlighterInitInfo.Y;
            }
            else if(ip.StartsWith(config.ComConfig.IpWswUdpServer))
            {
                deltaX = angleWithLocation.X - config.WswData.HelicopterInitInfo.X;
                deltaY = angleWithLocation.Y - config.WswData.HelicopterInitInfo.Y;
            }
            return $"deltaX:{deltaX}; deltaY:{deltaY}";
        }

        /// <summary>
        /// 获取Wsw模型对应PC的局域网IP地址
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static IPAddress WswModelKindToIp(WswModelKind kind)
        {
            var comConfig = JsonFileConfig.Instance.ComConfig;
            switch (kind)
            {
                case WswModelKind.Flighter:
                    return IPAddress.Parse(comConfig.Ip720Platform);
                case WswModelKind.Flighter2:
                    return IPAddress.Parse(comConfig.Ip720Platform2);
                case WswModelKind.Helicopter:
                    return IPAddress.Parse(comConfig.IpWswUdpServer);
                case WswModelKind.Missile:
                    return IPAddress.Parse(comConfig.IpGunBarrel);
            }
            return null;
        }

        /// <summary>
        /// 获取第三方视角对应PC的局域网IP地址
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static IPAddress WswViewMoniorIp(WswModelKind kind)
        {
            var comConfig = JsonFileConfig.Instance.ComConfig;
            return IPAddress.Parse(comConfig.IpViewMonitor);
        }

        /// <summary>
        /// 获取wsw模型在教研台地图上的配置文件
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static WswModelInfo KindToMyInfo(WswModelKind kind)
        {
            WswModelInfo info = default;
            if (kind == WswModelKind.Flighter)
                info = JsonFileConfig.Instance.MyFlighterInfo;
            else if (kind == WswModelKind.Flighter2)
                info = JsonFileConfig.Instance.MyFlighter2Info;
            else if (kind == WswModelKind.Helicopter)
                info = JsonFileConfig.Instance.MyHelicopterInfo;
            else
                info = JsonFileConfig.Instance.MyMissileInfo;
            return info;
        }

        /// <summary>
        /// 获取wsw模型在Wsw平台上的原始配置信息
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static AngleWithLocation KindToWswInitData(WswModelKind kind)
        {
            AngleWithLocation info = default;
            WswData data = new WswData();
            if (kind == WswModelKind.Flighter)
                info = data.FlighterInitInfo;
            else if (kind == WswModelKind.Flighter2)
                info = data.Flighter2InitInfo;
            else if (kind == WswModelKind.Helicopter)
                info = data.HelicopterInitInfo;
            else
                info = data.MissileInitInfo;
            return info;
        }

    }
}
