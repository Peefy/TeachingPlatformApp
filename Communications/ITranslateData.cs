using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using TeachingPlatformApp.Models;

namespace TeachingPlatformApp.Communications
{
    /// <summary>
    /// Communicate with four computers UDP interface
    /// </summary>
    public interface ITranslateData
    {
        
        Task<int> SendToSixPlatformAsync(byte[] data);
        Task<int> SendToGunBarrelAsync(byte[] data);
        Task<int> SendTo720PlatformAsync(byte[] data);
        Task<int> SendStringToAsync(string str, int num);
        Task<byte[]> RecieveDataAsync();
        Task<UdpReceiveResult> RecieveAsync();

        void SendToUnity720View(byte[] bytes);

        void SendTo720PlatformWsw(byte[] bytes);
        void SendToGunBarrelWsw(byte[] bytes);
        void SendToSixPlatformWsw(byte[] bytes);

        void SendTo(byte[] bytes, IPEndPoint iPEndPoint);
        byte[] Recieve(ref IPEndPoint iPEndPoint);

        PlaneInfo PlaneInfo { get; set; }

    }
}
