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

        /// <summary>
        /// 语音接口 (依赖注入获得)
        /// </summary>
        public ISpeek Speeker => Ioc.Get<ISpeek>();

        /// <summary>
        /// 接收RecieveUdpPacketCount次数据刷新一次UI,春哥发太快了.....
        /// </summary>
        public int RecieveUdpPacketCount { get; set; } = 0;

        /// <summary>
        /// 是否检测飞行实验合格
        /// </summary>
        public bool IsJudgeValid { get; set; }

        int _milliSeconds = Convert.ToInt32(LogAndConfig.Config.
            GetProperty(ConfigKeys.UdpPort, 150_000));
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
        /// <summary>
        /// Udp是否连接
        /// </summary>
        public bool IsConnect
        {
            get => _isConnect;
            set => SetProperty(ref _isConnect, value);
        }

        private int _platformRunTime;
        /// <summary>
        /// 平台运行时间
        /// </summary>
        public int PlatformRunTime
        {
            get => _platformRunTime;
            set
            {
                SetProperty(ref _platformRunTime, value);
            }
        }

        /// <summary>
        /// 飞行实验是否开始
        /// </summary>
        public bool IsTotalStart
        {
            get
            {
                var isStart = false;
                foreach (var item in FlightExperiments)
                {
                    isStart = isStart | item.IsStart;
                }
                return isStart;
            }
        }

        private string _statusText = "";
        /// <summary>
        /// 控制台显示文字
        /// </summary>
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

        /// <summary>
        /// 开始实验Command
        /// </summary>
        public DelegateCommand StartCommand { get; set; }

        /// <summary>
        /// 停止实验Command
        /// </summary>
        public DelegateCommand StopCommand { get; set; }

        /// <summary>
        /// 清空显示Command
        /// </summary>
        public DelegateCommand ClearCommand { get; set; }

        /// <summary>
        /// 打开地图Command
        /// </summary>
        public DelegateCommand OpenFlightMapCommand { get; set; }

        /// <summary>
        /// 打开配置窗口
        /// </summary>
        public DelegateCommand OpenConfigWindowCommand { get; set; }

        /// <summary>
        /// 打开语音窗口
        /// </summary>
        public DelegateCommand OpenSpeechWindowCommand {get;set;}

        /// <summary>
        /// test Command
        /// </summary>
        public DelegateCommand TestCommand { get; set; }

        /// <summary>
        /// TreeView Command
        /// </summary>
        public DelegateCommand<TreeView> TreeViewSelectedCommand { get; set; }

        /// <summary>
        /// TreeView Nodes
        /// </summary>
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
                        AppendStatusText(JsonFileConfig.Instance.
                            FlightExperimentConfig.Introductions[SelectIndexTreeNode1]);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            });
            ConfigInit();
            ComAndCommandInit();
        }

        public void AppendStatusText(string str)
        {
            StatusText += str + Environment.NewLine;
        }

        private void ComAndCommandInit()
        {         
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
                                UdpServer.TranslateInfo.IsConnect = true;
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
                            UdpServer.TranslateInfo.IsConnect = true;
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
                    await SendSetPoints(item);
                    StatusText += $"{DateTime.Now}:您开始了{item.Name}实验\r\n";
                    UdpServer.TranslateInfo.IsTest = true;
                    Speeker?.SpeekAsync($"{item.Name}实验开始");
                    //循环检测实验是否合格
                    await Task.Delay(100);
                    await Task.WhenAny(item.StartAsync(), Task.Delay(MilliSeconds));
                    await Task.Delay(100);
                    if(UdpServer.TranslateInfo.IsTest == true)
                    {
                        if (IsJudgeValid == true && item.IsValid == false)
                        {
                            AppendStatusText($"{DateTime.Now}:{item.Name}实验失败，不符合实验要求");
                            Speeker?.SpeekAsync($"{item.Name}实验失败，不符合实验要求");
                        }
                        if (item.IsStop == true)
                        {
                            StatusText += $"{DateTime.Now}:{item.Name}实验结束\r\n";                      
                        }
                        else
                        {
                            StatusText += $"{DateTime.Now}:{item.Name}实验失败，时间超时\r\n";
                            Speeker?.SpeekAsync($"{item.Name}实验失败，时间超时");
                        }
                        UdpServer.TranslateInfo.IsTest = false;
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
                if (SelectIndexTreeNode1 == -1)
                    return;
                var item = FlightExperimentSelected;
                UdpServer.TranslateInfo.FlightExperimentName = item.Name;
                UdpServer.TranslateInfo.FlightExperimentIndex = SelectIndexTreeNode1;
                var flightMapWindow = new FlightMapWindow();
                if (item?.HasSetPoints == true && item.SetPoints != null &&
                    flightMapWindow.DataContext is FlightMapWindowViewModel viewModel)
                {
                    viewModel.SetPoints = item.SetPoints;
                    viewModel.DealHasSetPoints();
                }
                AppendStatusText($"打开了{item.Name}实验的地图界面");
                flightMapWindow.Show();
            });
            OpenConfigWindowCommand = new DelegateCommand(() => 
            {
                new ConfigWindow().Show();
            });
            OpenSpeechWindowCommand = new DelegateCommand(() =>
            {
                new SpeechWindow().Show();
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
            await Task.Delay(5);
        }

        private void ConfigInit()
        {
            var config = JsonFileConfig.Instance;
            RecieveUdpPacketCount = config.ComConfig.RenewUIRecieveCount;
            IsJudgeValid = config.FlightExperimentConfig.IsJudgeValid;
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
                    UdpServer.TranslateInfo.Flighter = angleWithLocation;
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
                    UdpServer.TranslateInfo.Helicopter = angleWithLocation;
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
