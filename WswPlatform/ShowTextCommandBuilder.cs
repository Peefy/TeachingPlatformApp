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

        /// <summary>
        /// 设置显示文字构造器
        /// </summary>
        /// <param name="kind"></param>
        public ShowTextCommandBuilder(WswModelKind kind)
        {
            CommonConstrctor();
            _command.Fire0 = (int)kind;
            _kind = kind;
        }

        /// <summary>
        /// 设置显示文字构造器
        /// </summary>
        /// <param name="kind">模型种类</param>
        /// <param name="showTime">显示时间</param>
        public ShowTextCommandBuilder(WswModelKind kind, int showTime = 3)
        {
            CommonConstrctor();
            _command.Fire0 = (int)kind;
            _command.Fire1 = showTime;
            _kind = kind;
        }

        /// <summary>
        /// 设置显示文字构造器
        /// </summary>
        /// <param name="kind">模型种类</param>
        /// <param name="text">显示文字</param>
        /// <param name="showTime">显示时间</param>
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

        /// <summary>
        /// 设置显示文字的模型
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public ShowTextCommandBuilder SetWswModelKind(WswModelKind kind)
        {
            _kind = kind;
            _command.Fire0 = (int)kind;
            return this;
        }

        /// <summary>
        /// 设置显示文字的时长
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public ShowTextCommandBuilder SetShowTextTime(int time = 3)
        {
            _command.Fire1 = time;
            return this;
        }

        /// <summary>
        /// 显示文字的内容
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 构造通信的结构体
        /// </summary>
        /// <returns></returns>
        public VSPFlightVisualCommand Build() => _command;

        /// <summary>
        /// 构造通信结构体的字节(不包含显示文字字节)
        /// </summary>
        /// <returns></returns>
        public byte[] BuildCommandBytes() => StructHelper.StructToBytes(_command);

        /// <summary>
        /// 构造通信结构体的全部字节
        /// </summary>
        /// <returns></returns>
        public byte[] BuildCommandTotalBytes()
        {
            var list = new List<byte>();
            list.AddRange(BuildCommandBytes());
            list.AddRange(_textBytes);
            return list.ToArray();
        }

        /// <summary>
        /// 设置显示文字
        /// </summary>
        /// <param name="kind">模型种类</param>
        /// <param name="text">显示文字</param>
        /// <param name="showTime">显示时长</param>
        public static void SetShowTextTo(WswModelKind kind, string text, int showTime = 3)
        {
            if (kind == WswModelKind.Missile)
                return;
            new ShowTextCommandBuilder(kind, text, showTime).Send();
        }
    }
}
