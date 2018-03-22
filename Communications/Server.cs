using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

using TeachingPlatformApp.Utils;
using TeachingPlatformApp.Models;

namespace TeachingPlatformApp.Communications
{
    /// <summary>
    /// 通信接口UdpClient实现
    /// </summary>
    public class Server : ITranslateData, IDisposable
    {
        /// <summary>
        /// 默认的自身PC Udp监听端口号
        /// </summary>
        public const int DefaultPort = 16000;

        /// <summary>
        /// C# UDP UdpClient
        /// </summary>
        UdpClient _server;
        
        /// <summary>
        /// 720PC Udp监听端口号
        /// </summary>
        int _udp720Port = 15000;

        /// <summary>
        /// Wsw视景软件监听端口号
        /// </summary>
        int _wswUdpPort = 14000;

        /// <summary>
        /// 720Unity控制软件实验接收数据监听端口
        /// </summary>
        int _udp720TechingPort = 12000;

        /// <summary>
        /// 720控制台测试软件实验接收数据监听端口
        /// </summary>
        int _udp720TestConsolePort = 11000;

        /// <summary>
        /// 自身监听的端口号
        /// </summary>
        int _port = DefaultPort;

        /// <summary>
        /// 自身ip，通过配置文件可修改
        /// </summary>
        string _ip = "192.168.0.135"; //本机Ip;

        /// <summary>
        /// 六自由度平台Ip
        /// </summary>
        string _ipSixPlatform = "192.168.0.131";

        /// <summary>
        /// 第2个720度平台Ip
        /// </summary>
        string _ip720Platform2 = "192.168.0.132";

        /// <summary>
        /// 单兵炮筒平台Ip
        /// </summary>
        string _ipGunBarrel = "192.168.0.133";

        /// <summary>
        /// 720度平台Ip
        /// </summary>
        string _ip720Platform = "192.168.0.134";

        /// <summary>
        /// 发送一次数据后的延时
        /// </summary>
        int _sendAfterDelay = 10;

        /// <summary>
        /// 自己电脑IP
        /// </summary>
        public IPAddress IpAddressLocal => IPAddress.Parse(_ip);
        /// <summary>
        /// 六自由度平台Ip
        /// </summary>
        public IPAddress IpAddressSixPlatform => IPAddress.Parse(_ipSixPlatform);
        /// <summary>
        /// 单兵电脑Ip
        /// </summary>
        public IPAddress IpAddressGunBarrel => IPAddress.Parse(_ipGunBarrel);
        /// <summary>
        /// 720度电脑Ip
        /// </summary>
        public IPAddress IpAddress720Platform => IPAddress.Parse(_ip720Platform);

        /// <summary>
        /// 第二个720度电脑Ip
        /// </summary>
        public IPAddress IpAddress720Platform2 => IPAddress.Parse(_ip720Platform2);

        /// <summary>
        /// 六自由度平台Ip And Port
        /// </summary>
        public IPEndPoint IpEndPointSixPlatform { get; set; }

        /// <summary>
        /// 单兵电脑Ip And Port
        /// </summary>
        public IPEndPoint IpEndPointGunBarrel { get; set; }

        /// <summary>
        /// 720度电脑Ip And Port
        /// </summary>
        public IPEndPoint IpEndPoint720Platform { get; set; }

        /// <summary>
        /// 第二个720度电脑Ip And Port
        /// </summary>
        public IPEndPoint IpEndPoint720Platform2 { get; set; }

        /// <summary>
        /// 传输信息公用接口
        /// </summary>
        public TranslateInfo TranslateInfo { get; set; }

        /// <summary>
        /// 构造函数，数据初始化，Udp开始监听端口号
        /// </summary>
        public Server()
        {
            TranslateInfo = new TranslateInfo();
            var config = JsonFileConfig.Instance.ComConfig;
            _port = config.SelfPort;
            _udp720Port = config.Udp720Port;
            _wswUdpPort = config.WswUdpPort;
            _ip = config.IpSelf;
            _ipSixPlatform = config.IpWswUdpServer;
            _ipGunBarrel = config.IpGunBarrel;
            _ip720Platform = config.Ip720Platform;
            _ip720Platform2 = config.Ip720Platform2;
            _udp720TechingPort = config.Udp720TechingPort;
            _udp720TestConsolePort = config.Udp720TestConsolePort;           
            _server = new UdpClient(_port);
            IpEndPoint720Platform = new IPEndPoint(IpAddress720Platform, _udp720Port);
            IpEndPointSixPlatform = new IPEndPoint(IpAddressSixPlatform, _wswUdpPort);
            IpEndPointGunBarrel = new IPEndPoint(IpAddressGunBarrel, _wswUdpPort);
            IpEndPoint720Platform2 = new IPEndPoint(IpAddress720Platform2, _wswUdpPort);
        }

        /// <summary>
        /// 向六自由度平台PC WswTHUSim 发送消息(异步)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<int> SendToSixPlatformAsync(byte[] data)
        {
            var result = await _server.SendAsync(data, data.Length,
                new IPEndPoint(IpAddressSixPlatform, _wswUdpPort));
            await Task.Delay(_sendAfterDelay);
            return result;
        }

        /// <summary>
        /// 向单兵平台PC WswTHUSim 发送消息(异步)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<int> SendToGunBarrelAsync(byte[] data)
        {
            var result = await _server.SendAsync(data, data.Length,
                new IPEndPoint(IpAddressGunBarrel, _wswUdpPort));
            await Task.Delay(_sendAfterDelay);
            return result;
        }

        /// <summary>
        /// 向720平台PC WswTHUSim 发送消息(异步)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<int> SendTo720PlatformAsync(byte[] data)
        {
            var result = await _server.SendAsync(data, data.Length,
                new IPEndPoint(IpAddress720Platform, _wswUdpPort));
            await Task.Delay(_sendAfterDelay);
            return result;
        }

        /// <summary>
        /// 向索引号为num的电脑PC WswTHUSim 发送消息(异步) 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public async Task<int> SendStringToAsync(string str,int num)
        {
            var bytes = Encoding.Default.GetBytes(str);
            await _server.SendAsync(bytes, bytes.Length);
            switch(num)
            {
                case 0:
                    return await SendToSixPlatformAsync(bytes);
                case 1:
                    return await SendToGunBarrelAsync(bytes);
                case 2:
                    return await SendTo720PlatformAsync(bytes);
                default:
                    return await Task.FromResult(-1);
            }
        }

        /// <summary>
        /// 接收数据(不含IP数据)
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> RecieveDataAsync()
        {
            return await Task.Run(() =>
            {
                var ipEndPointnew = new IPEndPoint(IPAddress.Any, 0);
                return _server.Receive(remoteEP: ref ipEndPointnew);
            });
        }

        /// <summary>
        /// 从任意IP接收数据
        /// </summary>
        /// <returns></returns>
        public async Task<UdpReceiveResult> RecieveAsync()
        {
            return await _server.ReceiveAsync();
        }

        /// <summary>
        /// 向720度平台PC Unity软件发送数据(同步)
        /// </summary>
        /// <param name="bytes"></param>
        public void SendToUnity720View(byte[] bytes)
        {
            _server.Send(bytes, bytes.Length, new IPEndPoint(IpAddress720Platform, _udp720TechingPort));
            _server.Send(bytes, bytes.Length, new IPEndPoint(IpAddress720Platform, _udp720TestConsolePort));
        }

        /// <summary>
        /// 向单兵平台PC WswTHUSim 发送消息(同步)
        /// </summary>
        /// <param name="bytes"></param>
        public void SendToGunBarrelWsw(byte[] bytes)
        {
            _server.Send(bytes, bytes.Length, IpEndPointGunBarrel);
        }

        /// <summary>
        /// 向六自由度平台PC WswTHUSim 发送消息(同步)
        /// </summary>
        /// <param name="bytes"></param>
        public void SendToSixPlatformWsw(byte[] bytes)
        {
            _server.Send(bytes, bytes.Length, IpEndPointSixPlatform);
        }

        /// <summary>
        /// 向720平台PC WswTHUSim 发送消息(同步)
        /// </summary>
        /// <param name="bytes"></param>
        public void SendTo720PlatformWsw(byte[] bytes)
        {
            _server.Send(bytes, bytes.Length, IpEndPoint720Platform);
        }

        /// <summary>
        /// 通用Udp接收数据(同步)
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="iPEndPoint"></param>
        public byte[] Recieve(ref IPEndPoint iPEndPoint)
        {
            return _server.Receive(ref iPEndPoint);
        }

        /// <summary>
        /// 通用Udp发送数据(同步)
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="iPEndPoint"></param>
        public void SendTo(byte[] bytes, IPEndPoint iPEndPoint)
        {
            if(bytes != null && iPEndPoint != null)
            {
                _server.Send(bytes, bytes.Length, iPEndPoint);
            }
        }

        /// <summary>
        /// 析构函数，释放资源
        /// </summary>
        ~Server()
        {
            Dispose(false);
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }
                //释放Udp监听资源
                if(_server != null)
                {
                    _server.Close();
                    _server = null;
                }
                disposedValue = true;
            }
        }

        /// <summary>
        /// 释放Udp资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

    }
}
