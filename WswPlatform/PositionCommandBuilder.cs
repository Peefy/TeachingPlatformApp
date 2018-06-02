
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;

using DuGu.NetFramework.Services;

using TeachingPlatformApp.Communications;
using TeachingPlatformApp.Utils;

namespace TeachingPlatformApp.WswPlatform
{
    /// <summary>
    /// 战斗机，直升机初始坐标设置通信Packet 构造器
    /// </summary>
    public class PositionCommandBuilder
    {
        VSPFlightVisualCommand _command;
        WswModelKind _kind = WswModelKind.Missile;
        int _port;

        private IPEndPoint KindToIp()
        {
            return new IPEndPoint(WswHelper.WswModelKindToIp(_kind), _port);
        }

        private void CommonConstrctor()
        {
            _command = new VSPFlightVisualCommand
            {
                MessageType = (int)WswMessageType.ResetFlightStatusMornitor,
                Position0 = default
            };
            _port = JsonFileConfig.Instance.ComConfig.WswModelPositionUdpPort;
        }

        protected PositionCommandBuilder()
        {
            CommonConstrctor();
        }

        public PositionCommandBuilder(WswModelKind kind)
        {
            CommonConstrctor();
            _kind = kind;
        }

        public PositionCommandBuilder SetWswModelKind(WswModelKind kind)
        {
            _kind = kind;
            return this;
        }

        private double XYZToLon(double x, double y)
        {
            var lon = Math.Atan2(y, x);
            return lon * 180.0 / Math.PI;
        }

        private double XYZToLat(double x, double y, double z)
        {
            var earth_r = 6378137.0;
            var c = earth_r;
            var lon = Math.Atan2(y, x);
            double t0, t, tOld, dt;
            dt = 1.0;
            t0 = z / Math.Sqrt(x * x + y * y);
            tOld = t0;
            double p = 0;
            double k = 1;
            t = 0.0;
            while(dt > 0.001)
            {
                t = t0 + p * tOld / Math.Sqrt(k + tOld * tOld);
                dt = t - tOld;
                tOld = t;
            }
            var lat = Math.Atan(t);
            return lat * 180.0 / Math.PI;
        }

        public PositionCommandBuilder SetInitialPosition(double x, double y, double z)
        {
            var anglePosition = WswHelper.DealMyMapDataToWswAngle(x, y, z, _kind);
            var data = WswHelper.KindToWswInitData(_kind);
            _command.Lat = XYZToLat(anglePosition.X, anglePosition.Y, data.Z);
            _command.Lon = XYZToLon(anglePosition.X, anglePosition.Y);
            return this;
        }

        public PositionCommandBuilder SetInitialLonLan(double lon, double lat)
        {
            _command.Lat = lat;
            _command.Lon = lon;
            return this;
        }

        public PositionCommandBuilder SetAngleWithLocation(AngleWithLocation angleWithLocation)
        {
            _command.Lat = XYZToLat(angleWithLocation.X, angleWithLocation.Y, angleWithLocation.Z);
            _command.Lon = XYZToLon(angleWithLocation.X, angleWithLocation.Y);
            return this;
        }

        public void Send()
        {
            Ioc.Get<ITranslateData>().SendTo(BuildCommandBytes(), KindToIp());
        }

        public VSPFlightVisualCommand Build() => _command;

        public byte[] BuildCommandBytes()
        {
            var bytes = StructHelper.StructToBytes(_command);
            var sendBytes = new List<byte>();
            sendBytes.AddRange(bytes);
            for(var i = 0;i < ShowTextCommandBuilder.TextMaxLength ;++i)
            {
                sendBytes.Add(0);
            }
            return sendBytes.ToArray();
        }
        

        /// <summary>
        /// 设置模型在视景中的坐标位置
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="point"></param>
        public static void SendPositionTo(WswModelKind kind, Point point)
        {
            if (kind == WswModelKind.Missile || point == null)
                return;
            new PositionCommandBuilder().
                SetWswModelKind(kind).
                SetInitialPosition(point.X, point.Y, 0).
                Send();
        }

        /// <summary>
        /// 设置模型在视景中的经纬度位置
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="point"></param>
        public static void SendPositionLonLatTo(WswModelKind kind, float lon, float lat)
        {
            if (kind == WswModelKind.Missile)
                return;
            new PositionCommandBuilder().
                SetWswModelKind(kind).
                SetInitialLonLan(lon, lat).
                Send();
        }

        /// <summary>
        /// 设置模型在视景中的坐标位置
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void SendPositionTo(WswModelKind kind, double x, double y)
        {
            SendPositionTo(kind, new Point(x, y));
        }

        /// <summary>
        /// 设置模型在视景中的初始默认坐标位置
        /// </summary>
        /// <param name="kind"></param>
        public static void SetDefaultPositionTo(WswModelKind kind)
        {
            if (kind == WswModelKind.Missile)
                return;
            var config = JsonFileConfig.Instance;
            var data = WswHelper.KindToWswInitData(kind);
            new PositionCommandBuilder().
                SetWswModelKind(kind).
                SetAngleWithLocation(data).
                Send();
        }

    }

}
