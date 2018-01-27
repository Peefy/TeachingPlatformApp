using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using TeachingPlatformApp.Models;

namespace TeachingPlatformApp.Communications
{
    /// <summary>
    /// Communicate with four computers UDP interface
    /// </summary>
    public interface ITranslateData
    {
        /// <summary>
        /// 向六自由度平台PC WswTHUSim 发送消息(异步)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<int> SendToSixPlatformAsync(byte[] data);

        /// <summary>
        /// 向单兵平台PC WswTHUSim 发送消息(异步)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<int> SendToGunBarrelAsync(byte[] data);

        /// <summary>
        /// 向720平台PC WswTHUSim 发送消息(异步)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<int> SendTo720PlatformAsync(byte[] data);

        /// <summary>
        /// 向索引号为num的电脑PC WswTHUSim 发送消息(异步) 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        Task<int> SendStringToAsync(string str, int num);

        /// <summary>
        /// 接收数据(不含IP数据)
        /// </summary>
        /// <returns></returns>
        Task<byte[]> RecieveDataAsync();

        /// <summary>
        /// 从任意IP接收数据
        /// </summary>
        /// <returns></returns>
        Task<UdpReceiveResult> RecieveAsync();
        
        /// <summary>
        /// 向720度平台PC Unity软件发送数据(同步)
        /// </summary>
        /// <param name="bytes"></param>
        void SendToUnity720View(byte[] bytes);

        /// <summary>
        /// 向六自由度平台PC WswTHUSim 发送消息(同步)
        /// </summary>
        /// <param name="bytes"></param>
        void SendToSixPlatformWsw(byte[] bytes);

        /// <summary>
        /// 向单兵平台PC WswTHUSim 发送消息(同步)
        /// </summary>
        /// <param name="bytes"></param>
        void SendToGunBarrelWsw(byte[] bytes);

        /// <summary>
        /// 向720平台PC WswTHUSim 发送消息(同步)
        /// </summary>
        /// <param name="bytes"></param>
        void SendTo720PlatformWsw(byte[] bytes);

        /// <summary>
        /// 通用Udp发送数据(同步)
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="iPEndPoint"></param>
        void SendTo(byte[] bytes, IPEndPoint iPEndPoint);

        /// <summary>
        /// 通用Udp接收数据(同步)
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="iPEndPoint"></param>
        byte[] Recieve(ref IPEndPoint iPEndPoint);

        /// <summary>
        /// Wsw平台所有模型数据汇总
        /// </summary>
        WswModelInfo PlaneInfo { get; set; }

    }
}
