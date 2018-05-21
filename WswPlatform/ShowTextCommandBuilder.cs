using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using DuGu.NetFramework.Services;

using TeachingPlatformApp.Communications;
using TeachingPlatformApp.Utils;

namespace TeachingPlatformApp.WswPlatform
{
    public class ShowTextCommandBuilder
    {
        public const int TextMaxLength = 128;

        VSPFlightVisualCommand _command;
        WswModelKind _kind = WswModelKind.Missile;
        int _port;
        byte[] _textBytes = new byte[TextMaxLength];

        private IPEndPoint SendIp()
        {
            return new IPEndPoint(WswHelper.WswModelKindToIp(WswModelKind.Helicopter), _port);
        }

        private void CommonConstrctor()
        {
            _command = new VSPFlightVisualCommand
            {
                MessageType = (int)WswMessageType.ShowText,
                Position0 = default
            };
            _port = JsonFileConfig.Instance.ComConfig.WswModelShowTextUdpPort;
        }

        protected ShowTextCommandBuilder()
        {
            CommonConstrctor();
        }

        public ShowTextCommandBuilder(WswModelKind kind)
        {
            CommonConstrctor();
            _command.Fire0 = (int)kind;
            _kind = kind;
        }

        public ShowTextCommandBuilder(WswModelKind kind, int showTime = 3)
        {
            CommonConstrctor();
            _command.Fire0 = (int)kind;
            _command.Fire1 = showTime;
            _kind = kind;
        }

        public ShowTextCommandBuilder(WswModelKind kind, string text ,int showTime = 3)
        {
            CommonConstrctor();
            _command.Fire0 = (int)kind;
            _command.Fire1 = showTime;
            _kind = kind;
            _textBytes = SetTextAndBuildBytes(text);
        }

        private byte[] SetTextAndBuildBytes(string text)
        {
            var textBytes = Encoding.Unicode.GetBytes(text);
            var totalBytes = new List<byte>();
            totalBytes.AddRange(textBytes);
            if (totalBytes.Count > TextMaxLength)
            {
                throw new Exception("Too many characters");
            }
            for (var i = totalBytes.Count; i < TextMaxLength; ++i)
            {
                totalBytes.Add('\0' - 0);
            }
            return totalBytes.ToArray();
        }

        public ShowTextCommandBuilder SetWswModelKind(WswModelKind kind)
        {
            _kind = kind;
            _command.Fire0 = (int)kind;
            return this;
        }

        public ShowTextCommandBuilder SetShowTextTime(int time = 3)
        {
            _command.Fire1 = time;
            return this;
        }

        public ShowTextCommandBuilder SetShowText(string text)
        {
            SetTextAndBuildBytes(text);
            return this;
        }

        public void Send()
        {
            var bytes = BuildCommandTotalBytes();
            Ioc.Get<ITranslateData>().SendTo(bytes, SendIp());
        }

        public VSPFlightVisualCommand Build() => _command;

        public byte[] BuildCommandBytes() => StructHelper.StructToBytes(_command);

        public byte[] BuildCommandTotalBytes()
        {
            var list = new List<byte>();
            list.AddRange(BuildCommandBytes());
            list.AddRange(_textBytes);
            return list.ToArray();
        }

        public static void SetShowTextTo(WswModelKind kind, string text, int showTime = 3)
        {
            if (kind == WswModelKind.Missile)
                return;
            new ShowTextCommandBuilder(kind, text, showTime).Send();
        }
    }
}
