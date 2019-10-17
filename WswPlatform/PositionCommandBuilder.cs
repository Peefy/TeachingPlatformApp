
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

        /// <summary>
        /// 设置坐标构造器
        /// </summary>
        protected PositionCommandBuilder()
        {
            CommonConstrctor();
        }

        /// <summary>
        /// 设置坐标构造器
        /// </summary>
        /// <param name="kind">模型的类型</param>
        public PositionCommandBuilder(WswModelKind kind)
        {
            CommonConstrctor();
            _kind = kind;
        }

        /// <summary>
        /// 设置需要设置的模型
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public PositionCommandBuilder SetWswModelKind(WswModelKind kind)
        {
            _kind = kind;
            return this;
        }

        /// <summary>
        /// 地心坐标系坐标转经纬度坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected double XYZToLon(double x, double y)
        {
            var lon = Math.Atan2(y, x);
            return lon * 180.0 / Math.PI;
        }

        /// <summary>
        /// 地球坐标系坐标转经纬度坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        protected double XYZToLat(double x, double y, double z)
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

        /// <summary>
        /// 设置模型初始的在显示地图中的坐标
        /// </summary>
        /// <param name="x">显示地图中的x坐标</param>
        /// <param name="y">显示地图中的y坐标</param>
        /// <param name="z">显示地图中的z坐标</param>
        /// <returns></returns>
        public PositionCommandBuilder SetInitialPosition(double x, double y, double z)
        {
            var anglePosition = WswHelper.DealMyMapDataToWswAngle(x, y, z, _kind);
            var data = WswHelper.KindToWswInitData(_kind);
            _command.Lat = XYZToLat(anglePosition.X, anglePosition.Y, data.Z);
            _command.Lon = XYZToLon(anglePosition.X, anglePosition.Y);
            return this;
        }

        /// <summary>
        /// 设置模型初始的经纬度坐标
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public PositionCommandBuilder SetInitialLonLan(double lon, double lat)
        {
            _command.Lat = lat;
            _command.Lon = lon;
            return this;
        }

        /// <summary>
        /// 设置模型初始的地心坐标
        /// </summary>
        /// <param name="angleWithLocation"></param>
        /// <returns></returns>
        public PositionCommandBuilder SetAngleWithLocation(AngleWithLocation angleWithLocation)
        {
            _command.Lat = XYZToLat(angleWithLocation.X, angleWithLocation.Y, angleWithLocation.Z);
            _command.Lon = XYZToLon(angleWithLocation.X, angleWithLocation.Y);
            return this;
        }

        /// <summary>
        /// Udp发送数据
        /// </summary>
        public void Send()
        {
            Ioc.Get<ITranslateData>().SendTo(BuildCommandBytes(), KindToIp());
        }

        /// <summary>
        /// 获得构造的通信数据结构体
        /// </summary>
        /// <returns></returns>
        public VSPFlightVisualCommand Build() => _command;

        /// <summary>
        /// 通信数据结构体构造通信字节
        /// </summary>
        /// <returns></returns>
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
            var data = WswHelper.KindToWswInitData(kind);
            new PositionCommandBuilder().
                SetWswModelKind(kind).
                SetAngleWithLocation(data).
                Send();
        }

    }

}
