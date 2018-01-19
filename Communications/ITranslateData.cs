using System;
using System.Collections.Generic;
using System.Linq;
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
        void SendToUnity720View(byte[] bytes);
        Task<int> SendAsyncToOne(byte[] data);
        Task<int> SendAsyncToTwo(byte[] data);
        Task<int> SendAsyncToThree(byte[] data);
        Task<int> SendAsync(string str, int num);
        Task<byte[]> RecieveDataAsync();
        Task<UdpReceiveResult> RecieveAsync();

        PlaneInfo PlaneInfo { get; set; }

    }
}
