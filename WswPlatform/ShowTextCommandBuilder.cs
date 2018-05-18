using System.Net;
using System.Windows;
using DuGu.NetFramework.Services;

using TeachingPlatformApp.Communications;
using TeachingPlatformApp.Utils;

namespace TeachingPlatformApp.WswPlatform
{
    public class ShowTextCommandBuilder
    {
        VSPFlightVisualCommand _command;
        WswModelKind _kind = WswModelKind.Missile;
        int _port;

        private IPEndPoint SendIp()
        {
            return new IPEndPoint(WswHelper.WswModelKindToIp(WswModelKind.Flighter), _port);
        }

        private void CommonConstrctor()
        {
            _command = new VSPFlightVisualCommand
            {
                MessageType = (int)WswMessageType.ShowText,
                Position0 = default
            };
            _port = JsonFileConfig.Instance.ComConfig.WswModelPositionUdpPort;
        }

        protected ShowTextCommandBuilder()
        {
            CommonConstrctor();
        }

        public ShowTextCommandBuilder(WswModelKind kind)
        {
            CommonConstrctor();
            _command.Hit = (int)kind;
            _kind = kind;
        }

        public ShowTextCommandBuilder(WswModelKind kind, int showIndex, int showTime = 3)
        {
            CommonConstrctor();
            _command.Hit = (int)kind;
            _command.Fire0 = showIndex;
            _command.Fire1 = showTime;
            _kind = kind;
        }

        public ShowTextCommandBuilder SetWswModelKind(WswModelKind kind)
        {
            _kind = kind;
            _command.Hit = (int)kind;
            return this;
        }

        public ShowTextCommandBuilder SetShowTextIndex(int index)
        {
            _command.Fire0 = index;
            return this;
        }

        public ShowTextCommandBuilder SetShowTextTime(int time = 3)
        {
            _command.Fire1 = time;
            return this;
        }

        public void Send()
        {
            Ioc.Get<ITranslateData>().SendTo(BuildCommandBytes(), SendIp());
        }

        public VSPFlightVisualCommand Build() => _command;

        public byte[] BuildCommandBytes() => StructHelper.StructToBytes(_command);

        public static void SetShowTextTo(WswModelKind kind, int textIndex, int showTime = 3)
        {
            if (kind == WswModelKind.Missile)
                return;
            new ShowTextCommandBuilder(kind, textIndex, showTime).Send();
        }

    }

}
