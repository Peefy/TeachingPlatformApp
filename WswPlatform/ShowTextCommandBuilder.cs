using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Collections.Generic;

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
        public const int TextMaxLength = 128;

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
            _port = JsonFileConfig.Instance.ComConfig.WswModelPositionUdpPort;
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

        public ShowTextCommandBuilder(WswModelKind kind, int showIndex, int showTime = 3)
        {
            CommonConstrctor();
            _command.Fire0 = (int)kind;
            _command.Fire1 = showTime;
            _kind = kind;
        }

        public byte[] SetText(string text)
        {
            var textBytes = Encoding.Unicode.GetBytes(text);
            var totalBytes = new List<byte>();
            totalBytes.AddRange(textBytes);
            if (totalBytes.Count > TextMaxLength)
            {
                throw new Exception("more charactor count!");
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
            var bytes = BuildCommandBytes();
            Ioc.Get<ITranslateData>().SendTo(bytes, SendIp());
        }

        public VSPFlightVisualCommand Build() => _command;

        public byte[] BuildCommandBytes() => StructHelper.StructToBytes(_command);

        public byte[] BuildCommandBytes(object obj) => StructHelper.StructToBytes(obj);

        public static void SetShowTextTo(WswModelKind kind, int textIndex, int showTime = 3)
        {
            if (kind == WswModelKind.Missile)
                return;
            new ShowTextCommandBuilder(kind, textIndex, showTime).Send();
        }

        public static void SetShowTextTo(WswModelKind kind, string text, int showTime = 3)
        {
            if (kind == WswModelKind.Missile)
                return;
            var builder = new ShowTextCommandBuilder(kind, 0, showTime);
            var textbytes = builder.SetText(text);
            var bytes = builder.BuildCommandBytes();
            var list = new List<byte>();
            list.AddRange(bytes);
            list.AddRange(textbytes);
            var sendBytes = list.ToArray();
            var ip = builder.SendIp();
            Ioc.Get<ITranslateData>().SendTo(sendBytes, ip);
        }
    }

}
