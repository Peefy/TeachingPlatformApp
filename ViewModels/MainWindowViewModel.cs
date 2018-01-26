using System;
using System.Speech;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Threading.Tasks;

using Prism.Mvvm;
using Prism.Commands;

using DuGu.NetFramework.Services;
using DuGu.NetFramework.Logs;

using TeachingPlatformApp.WswPlatform;
using TeachingPlatformApp.Speech;
using TeachingPlatformApp.Models;
using TeachingPlatformApp.Views;
using TeachingPlatformApp.Communications;
using TeachingPlatformApp.Models.UI;
using TeachingPlatformApp.Utils;

namespace TeachingPlatformApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        /// <summary>
        /// 日志记录
        /// </summary>
        public ILog Logger => LogManager.GetCurrentLogger();
        /// <summary>
        /// Udp通信接口 (控制反转方式获得)
        /// </summary>
        public ITranslateData UdpServer => Ioc.Get<ITranslateData>();

        public ISpeek Speeker => Ioc.Get<ISpeek>();

        /// <summary>
        /// 接收RecieveUdpPacketCount次数据刷新一次UI,春哥发太快了.....
        /// </summary>
        public int RecieveUdpPacketCount { get; set; } = 0;

        int _milliSeconds = Convert.ToInt32(LogAndConfig.Config.
            GetProperty(ConfigKeys.UdpPort, 100_000));
        /// <summary>
        /// 飞行实验时间：100s
        /// </summary>
        public int MilliSeconds
        {
            get => _milliSeconds;
            set
            {
                SetProperty(ref _milliSeconds, value);
                LogAndConfig.Config.SetProperty(ConfigKeys.MilliSeconds, value);
            } 
        }

        private bool _isConnect;
        public bool IsConnect
        {
            get => _isConnect;
            set => SetProperty(ref _isConnect, value);
        }

        private int _platformRunTime;
        public int PlatformRunTime
        {
            get => _platformRunTime;
            set
            {
                SetProperty(ref _platformRunTime, value);
            }
        }

        public bool IsTotalStart
        {
            get
            {
                var isStart = false;
                foreach(var item in FlightExperiments)
                {
                    isStart = isStart | item.IsStart;
                }
                return isStart;
            }
        }

        private string _statusText = "";
        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        private string _recieveText = "角度大小";
        public string RecieveText
        {
            get => _recieveText;
            set => SetProperty(ref _recieveText, value);
        }

        private string _title = "教研员平台";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _isViewVisible = true;
        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set { SetProperty(ref _isViewVisible, value); }
        }

        private int _selectIndexTreeNode1 = -1;
        public int SelectIndexTreeNode1
        {
            get => _selectIndexTreeNode1;
            set => SetProperty(ref _selectIndexTreeNode1, value);
        }

        private int _selectIndexTreeNode2 = -1;
        public int SelectIndexTreeNode2
        {
            get => _selectIndexTreeNode2;
            set => SetProperty(ref _selectIndexTreeNode2, value);
        }

        private int _selectIndexTreeNode3 = -1;
        public int SelectIndexTreeNode3
        {
            get => _selectIndexTreeNode3;
            set => SetProperty(ref _selectIndexTreeNode3, value);
        }

        public float _time;
        public float Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }

        public DelegateCommand StartCommand { get; set; }
        public DelegateCommand StopCommand { get; set; }
        public DelegateCommand ClearCommand { get; set; }
        public DelegateCommand OpenFlightMapCommand { get; set; }
        public DelegateCommand TestCommand { get; set; }

        public DelegateCommand<TreeView> TreeViewSelectedCommand { get; set; }
        public ObservableRangeCollection<TreeViewModelItem> TreeViewNodes { get; set; }

        public ObservableRangeCollection<FlightExperiment> FlightExperiments 
            => TreeViewNodes[0].Children;

        public FlightExperiment FlightExperimentSelected => 
            SelectIndexTreeNode1 == -1 ? null : FlightExperiments[SelectIndexTreeNode1];

        public MainWindowViewModel()
        {          
            TreeViewNodes = new ObservableRangeCollection<TreeViewModelItem>
            {
                new TreeViewModelItem()
                {
                    Name = "模拟飞行实验",
                    Children = new ObservableRangeCollection<FlightExperiment>()
                    {
                        new TakeOffLanding(),   //起落航线
                        new AirlineFlight(),    //航线飞行
                        new Somersault(),       //斤斗
                        new HoverFlignt(),      //盘旋
                        new DiveJump(),         //俯冲跃升
                        new OnlyInstrumencs(),  //仪表飞行
                    }
                },
                new TreeViewModelItem()
                {
                    Name = "其他实验",
                    Children = new ObservableRangeCollection<FlightExperiment>()
                    {
                        new TakeOffLanding()
                        {
                            Name = "实验1"
                        },   
                    }
                },
            };
            TreeViewSelectedCommand = new DelegateCommand<TreeView>((treeView) =>
            {
                try
                {
                    if (treeView.SelectedItem is FlightExperiment item)
                    {
                        SelectIndexTreeNode1 = TreeViewNodes[0].Children.IndexOf(item);
                    }
                    AppendStatusText(JsonFileConfig.Instance.
                        FlightExperimentIntroduction.Introductions[SelectIndexTreeNode1]);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            });
            ComAndCommandInit();
        }

        public void AppendStatusText(string str)
        {
            StatusText += str + Environment.NewLine;
        }

        private void ComAndCommandInit()
        {
            ConfigInit();
            ///UdpMain
            Task.Run(() =>
            {
                try
                {
                    var realRecieCount = 0;
                    while (true)
                    {
                        var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        var recieveBytes = UdpServer.Recieve(ref ipEndPoint);
                        var ip = ipEndPoint.ToString();
                        var length = recieveBytes.Length;                       
                        Logger.Info($"{ip}:{length}\r\n");                    
                        if(++realRecieCount >= RecieveUdpPacketCount)
                        {
                            realRecieCount = 0;
                            if (length == StructHelper.GetStructSize<AngleWithLocation>())
                            {
                                DealAngleWithLocationData(ip, recieveBytes);
                                UdpServer.PlaneInfo.IsConnect = true;
                            }                      
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            });
            ///UdpExtral
            Task.Run(() => 
            {
                var realRecieCount = 0;
                var config = JsonFileConfig.Instance.ComConfig;
                var port = config.UdpClientExtraPort;
                var udpClientExtra = new UdpClient(port);
                var ipEndPoint = new IPEndPoint(IPAddress.Parse(config.Ip720Platform), config.Udp720Port);
                while(true)
                {
                    var recieveBytes = udpClientExtra.Receive(ref ipEndPoint);
                    var length = recieveBytes.Length;
                    var ip = ipEndPoint.ToString();
                    if (++realRecieCount >= RecieveUdpPacketCount)
                    {
                        realRecieCount = 0;
                        if (length == StructHelper.GetStructSize<AngleWithLocation>())
                        {
                            DealAngleWithLocationData(ip, recieveBytes);
                            UdpServer.PlaneInfo.IsConnect = true;
                        }
                    }
                }
            });
            StartCommand = new DelegateCommand(async () =>
            {
                if (SelectIndexTreeNode1 == -1)
                    return;
                if(IsTotalStart == true)
                {
                    MessageBox.Show("不能同时进行两个实验");
                    return;
                }
                try
                {
                    var item = FlightExperimentSelected;
                    UdpServer?.SendToUnity720View(new TeachingCommandBuilder(SelectIndexTreeNode1, true).
                        BuildCommandBytes());
                    await Task.Delay(5);
                    UdpServer?.SendToUnity720View(new TeachingCommandBuilder(SelectIndexTreeNode1, true).
                        BuildCommandBytes());
                    //发两次
                    await Task.Delay(5);
                    await SendSetPoints(item);
                    await Task.Delay(5);
                    await SendSetPoints(item);
                    await Task.Delay(5);
                    StatusText += $"{DateTime.Now}:您开始了{item.Name}实验\r\n";                  
                    //循环检测实验是否合格
                    await Task.Delay(100);
                    await Task.WhenAny(item.StartAsync(), Task.Delay(MilliSeconds));
                    await Task.Delay(100);
                    if (item.IsValid == false)
                    {
                        AppendStatusText($"{DateTime.Now}:{item.Name}实验失败，不符合实验要求");
                        Speeker?.SpeekAsync($"{item.Name}实验失败，不符合实验要求");
                    }
                    if (item.IsStop == true)
                        StatusText += $"{DateTime.Now}:{item.Name}实验结束\r\n";
                    else
                    {
                        StatusText += $"{DateTime.Now}:{item.Name}实验失败，时间超时\r\n";
                        Speeker?.SpeekAsync($"{item.Name}实验失败，时间超时");
                    }
                        
                    await item.EndAsync();
                }
                catch(Exception ex)
                {
                    StatusText += ex.Message;
                    Logger.Error(ex);
                }            
            });
            StopCommand = new DelegateCommand(async () =>
            {
                try
                {
                    foreach(var item in FlightExperiments)
                    {
                        if(item.IsStart == true)
                        {
                            UdpServer?.SendToUnity720View(new TeachingCommandBuilder(0, false).
                                BuildCommandBytes());
                            await Task.Delay(10);
                            UdpServer?.SendToUnity720View(new TeachingCommandBuilder(0, false).
                                BuildCommandBytes());
                            await Task.Delay(100);
                            await item.EndAsync();
                            StatusText += $"{DateTime.Now}:您结束了{item.Name}实验\r\n";
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    StatusText += ex.Message;
                    Logger.Error(ex);
                }
            });
            ClearCommand = new DelegateCommand(() =>
            {
                StatusText = "";
            });
            OpenFlightMapCommand = new DelegateCommand(() =>
            {
                var item = FlightExperimentSelected;
                var flightMapWindow = new FlightMapWindow();
                if (item?.HasSetPoints == true && item.SetPoints != null &&
                    flightMapWindow.DataContext is FlightMapWindowViewModel viewModel)
                {
                    viewModel.SetPoints = item.SetPoints;
                    viewModel.DealHasSetPoints();
                }
                flightMapWindow.Show();
            });
            TestCommand = new DelegateCommand(() =>
            {
                var strs = Test.Run();
                foreach(var str in strs)
                    AppendStatusText(str);
            });
        }

        private async Task SendSetPoints(FlightExperiment item)
        {
            await Task.Run(() =>
            {
                if (item.SetPoints != null && UdpServer != null)
                {
                    for (var i = 0; i < item.SetPoints.Count; ++i)
                    {
                        UdpServer.SendTo720PlatformWsw(new DataPacketToWswBuilder(i,
                            item.SetPoints[i].X, item.SetPoints[i].Y).BuildBytes());
                    }
                }
            });
        }

        private void ConfigInit()
        {
            var config = JsonFileConfig.Instance;
            RecieveUdpPacketCount = config.ComConfig.RenewUIRecieveCount;
        }

        private void DealAngleWithLocationData(string ip, byte[] recieveBytes)
        {
            var config = JsonFileConfig.Instance;
            var angleWithLocation = StructHelper.BytesToStruct<AngleWithLocation>(recieveBytes);
            var angleDataDigit = config.DataShowConfig.AngleShowDigit;
            if (ip.StartsWith(config.ComConfig.Ip720Platform) == true)
            {             
                angleWithLocation = WswHelper.DealWswAngleToMyMapData(angleWithLocation, 
                    WswModelKind.Flighter, angleDataDigit);
                foreach (var flight in FlightExperiments)
                {
                    UdpServer.PlaneInfo.Flighter = angleWithLocation;
                    flight.Roll.Value = (float)angleWithLocation.Roll;
                    flight.Pitch.Value = (float)angleWithLocation.Pitch;
                    flight.Yaw.Value = (float)angleWithLocation.Yaw;
                    flight.NowLocation = new Point(angleWithLocation.X, angleWithLocation.Y);
                }
            }
            else if(ip.StartsWith(config.ComConfig.IpWswUdpServer) == true)
            {
                angleWithLocation = WswHelper.DealWswAngleToMyMapData(angleWithLocation, 
                    WswModelKind.Helicopter, angleDataDigit);
                foreach (var flight in FlightExperiments)
                {
                    UdpServer.PlaneInfo.Helicopter = angleWithLocation;
                    flight.SixPlatformRoll.Value = (float)angleWithLocation.Roll;
                    flight.SixPlatformPitch.Value = (float)angleWithLocation.Pitch;
                    flight.SixPlatformYaw.Value = (float)angleWithLocation.Yaw;
                    flight.SixPlatformNowLocation = new Point(angleWithLocation.X, angleWithLocation.Y);
                }
            }
            WswDataDebuger.Record(ip, angleWithLocation);
        }
    }
}
