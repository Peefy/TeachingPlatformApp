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
        public const int TextMaxLength = 40;

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
            var builder = new ShowTextCommandBuilder();
            var command = new ShowTextCommandHeader()
            {
                MessageType = (int)WswMessageType.ShowText,
                Kind = (byte)kind,
                ShowTime = (byte)showTime
            };
            var textBytes = Encoding.Unicode.GetBytes(text);
            command.TextBytesLength = (byte)textBytes.Length;
            var commandHeaderBytes = builder.BuildCommandBytes(command);
            var totalBytes = new List<byte>();
            totalBytes.AddRange(commandHeaderBytes);
            totalBytes.AddRange(textBytes);
            var headerSize = StructHelper.GetStructSize<ShowTextCommandHeader>();
            var totalSize = headerSize + TextMaxLength;
            var lastByte = totalBytes.LastOrDefault();
            if(totalBytes.Count > totalSize)
            {
                throw new Exception("more charactor count!");
            }
            for(var i = totalBytes.Count; i < TextMaxLength + headerSize; ++i)
            {
                totalBytes.Add('\n' - 0);
            }
            var sendBytes = totalBytes.ToArray();
            Ioc.Get<ITranslateData>().SendTo(sendBytes, builder.SendIp());
        }
    }

}
