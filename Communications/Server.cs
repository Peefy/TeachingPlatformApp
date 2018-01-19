using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using TeachingPlatformApp.Utils;
using TeachingPlatformApp.Models;

namespace TeachingPlatformApp.Communications
{
    public class Server : ITranslateData, IDisposable
    {

        public const int DefaultPort = 15000;

        UdpClient server;
        
        int _udp720Port = 16000;
        int _wswUdpPort = 15000;

        int _port = DefaultPort;
        string _ip = "192.168.0.133"; //本机Ip;

        string _ipOne = "192.168.0.131";
        string _ipTwo = "192.168.0.132";
        string _ipThree = "192.168.0.134";

        public IPAddress IpAddressLocal => IPAddress.Parse(_ip);
        public IPAddress IpAddressOne => IPAddress.Parse(_ipOne);
        public IPAddress IpAddressTwo => IPAddress.Parse(_ipTwo);
        public IPAddress IpAddressThree => IPAddress.Parse(_ipThree);

        public PlaneInfo PlaneInfo { get; set; }

        public Server()
        {
            PlaneInfo = new PlaneInfo();
            var config = JsonFileConfig.Instance.ComConfig;
            _port = config.SelfPort;
            _udp720Port = config.Udp720Port;
            _wswUdpPort = config.WswUdpPort;
            _ip = config.IpSelf;
            _ipOne = config.IpWswUdpServer;
            _ipTwo = config.IpGunBarrel;
            _ipThree = config.Ip720Platform;
            server = new UdpClient(_port);         
        }

        public async Task<int> SendAsyncToOne(byte[] data)
        {
            var result = await server.SendAsync(data, data.Length,
                new IPEndPoint(IPAddress.Parse(_ipOne), _wswUdpPort));
            await Task.Delay(10);
            return result;
        }

        public async Task<int> SendAsyncToTwo(byte[] data)
        {
            var result = await server.SendAsync(data, data.Length,
                new IPEndPoint(IPAddress.Parse(_ipTwo), _wswUdpPort));
            await Task.Delay(10);
            return result;
        }

        public async Task<int> SendAsyncToThree(byte[] data)
        {
            var result = await server.SendAsync(data, data.Length,
                new IPEndPoint(IPAddress.Parse(_ipTwo), _wswUdpPort));
            await Task.Delay(10);
            return result;
        }
        
        public async Task<int> SendAsync(string str,int num)
        {
            var bytes = Encoding.Default.GetBytes(str);
            await server.SendAsync(bytes, bytes.Length);
            switch(num)
            {
                case 0:
                    return await SendAsyncToOne(bytes);
                case 1:
                    return await SendAsyncToTwo(bytes);
                case 2:
                    return await SendAsyncToThree(bytes);
                default:
                    return await Task.FromResult(-1);
            }
        }

        public void SendToUnity720View(byte[] bytes)
        {
            server.Send(bytes, bytes.Length, new IPEndPoint(IPAddress.Broadcast, _udp720Port));
        }

        public async Task<byte[]> RecieveDataAsync()
        {
            return await Task.Run(() =>
            {
                var ipEndPointnew = new IPEndPoint(IPAddress.Any, 0);
                return server.Receive(remoteEP: ref ipEndPointnew);
            });
        }

        public async Task<UdpReceiveResult> RecieveAsync()
        {
            return await server.ReceiveAsync();
        }

        ~Server()
        {
            Dispose();
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }
                if(server != null)
                {
                    server.Close();
                    server = null;
                }
                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~Server() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
