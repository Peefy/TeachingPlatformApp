
using System;
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
        PositionCommand _command;
        WswModelKind _kind = WswModelKind.Missile;
        int _port;

        private IPEndPoint KindToIp()
        {
            return new IPEndPoint(WswHelper.WswModelKindToIp(_kind), _port);
        }

        private void CommonConstrctor()
        {
            _command = new PositionCommand
            {
                MessageType = (int)WswMessageType.DataGlovePosture,
                Engines = new double[4, 2],
                Position = new Glmdvec3[2],
                Yaw = new double[2],
                Pitch = new double[2],
                Roll = new double[2],
                Fire = new int[2]
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

        public PositionCommandBuilder SetInitialPosition(double x, double y, double z)
        {
            var anglePosition = WswHelper.DealMyMapDataToWswAngle(x, y, z, _kind);
            _command.Position[0].X = anglePosition.X;
            _command.Position[0].Y = anglePosition.Y;
            _command.Position[0].Z = anglePosition.Z;
            return this;
        }

        public void Send()
        {
            var a = sizeof(int);
            a = sizeof(double);
            a = StructHelper.GetStructSize<Glmdvec3>();
            var size = StructHelper.GetStructSize<PositionCommand>();
            var length = BuildCommandBytes().Length;
            Ioc.Get<ITranslateData>().SendTo(BuildCommandBytes(), KindToIp());
            Ioc.Get<ITranslateData>().SendTo(BuildCommandBytes(), KindToIp());
        }

        public PositionCommand Build() => _command;

        public byte[] BuildCommandBytes() => StructHelper.StructToBytes(_command);

        public static void SendPositionTo(WswModelKind kind, Point point)
        {
            if (kind == WswModelKind.Missile || point == null)
                return;
            new PositionCommandBuilder().
                SetWswModelKind(kind).
                SetInitialPosition(point.X, point.Y, 0).
                Send();
        }

        public static void SendPositionTo(WswModelKind kind, double x, double y)
        {
            SendPositionTo(kind, new Point(x, y));
        }

    }
}
