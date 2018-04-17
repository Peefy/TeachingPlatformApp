using System;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;

using Prism.Mvvm;

using DuGu.NetFramework.Services;

using TeachingPlatformApp.Speech;
using TeachingPlatformApp.Extensions;
using TeachingPlatformApp.Utils;
using TeachingPlatformApp.Models;
using TeachingPlatformApp.Communications;

namespace TeachingPlatformApp.ViewModels
{
    public class FlightMapWindowViewModel : BindableBase
    {

        /// <summary>
        /// 传递数据和Udp通信接口
        /// </summary>
        protected ITranslateData _translateData;    
        
        /// <summary>
        /// 配置
        /// </summary>
        protected JsonFileConfig _config;

        /// <summary>
        /// 地图刷新周期(单位毫秒)
        /// </summary>
        int _mapRefreshInterval = 30;

        /// <summary>
        /// 选择的飞行实验的索引
        /// </summary>
        int _flightTaskIndex = 0;

        /// <summary>
        /// 选择的飞行实验的名称
        /// </summary>
        string _flightTaskName = "";
   
        private string _title = "地图";
        /// <summary>
        /// 地图界面标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private FlighterModel _flighter;
        /// <summary>
        /// 战斗机
        /// </summary>
        public FlighterModel Flighter
        {
            get => _flighter;
            set => SetProperty(ref _flighter, value);
        }

        private Flighter2Model _flighter2;
        /// <summary>
        /// 第2个战斗机
        /// </summary>
        public Flighter2Model Flighter2
        {
            get => _flighter2;
            set => SetProperty(ref _flighter2, value);
        }

        private HelicopterModel _helicopter;
        /// <summary>
        /// 直升机
        /// </summary>
        public HelicopterModel Helicopter
        {
            get => _helicopter;
            set => SetProperty(ref _helicopter, value);
        }

        private MissileModel _missile;
        /// <summary>
        /// 导弹
        /// </summary>
        public MissileModel Missile
        {
            get => _missile;
            set => SetProperty(ref _missile, value);
        }

        protected ObservableRangeCollection<Point> _setPoints;
        /// <summary>
        /// 设置航路点
        /// </summary>
        public ObservableRangeCollection<Point> SetPoints
        {
            get => _setPoints;
            set => SetProperty(ref _setPoints, value);
        }

        protected ObservableRangeCollection<bool> _hasSetPoints;
        /// <summary>
        /// 是否有第几个航路点
        /// </summary>
        public ObservableRangeCollection<bool> HasSetPoints
        {
            get => _hasSetPoints;
            set => SetProperty(ref _hasSetPoints, value);
        }

        protected string _figurePathString = "M 20,20 L 120,120 320,420";
        /// <summary>
        /// 轨迹坐标连线
        /// </summary>
        public string FigurePathString
        {
            get => _figurePathString;
            set => SetProperty(ref _figurePathString, value);
        }

        private double _setPointsFontSize = 20;
        /// <summary>
        /// 航路点字符大小
        /// </summary>
        public double SetPointsFontSize
        {
            get => _setPointsFontSize;
            set => SetProperty(ref _setPointsFontSize, value);
        }

        private double _setPointsEllipseRadius = 6;
        /// <summary>
        /// 航路点圆半径
        /// </summary>
        public double SetPointsEllipseRadius
        {
            get => _setPointsEllipseRadius;
            set => SetProperty(ref _setPointsEllipseRadius, value);
        }

        private double _locationStringFontSize = 22;
        /// <summary>
        /// 位置字符串的字体大小
        /// </summary>
        public double LocationStringFontSize
        {
            get => _locationStringFontSize;
            set => SetProperty(ref _locationStringFontSize, value);
        }

        private double _setPointsLineWidth = 3;
        /// <summary>
        /// 航路点连线线段粗细
        /// </summary>
        public double SetPointsLineWidth
        {
            get => _setPointsLineWidth;
            set => SetProperty(ref _setPointsLineWidth, value);
        }

        /// <summary>
        /// 地图绘制页变距
        /// </summary>
        private Thickness _drawMargin = new Thickness(0);
        public Thickness DrawMargin
        {
            get => _drawMargin;
            set => SetProperty(ref _drawMargin, value);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FlightMapWindowViewModel()
        {
            _config = JsonFileConfig.ReadFromFile();
            _flightTaskIndex = Ioc.Get<ITranslateData>().TranslateInfo.FlightExperimentIndex;
            _flightTaskName = Ioc.Get<ITranslateData>().TranslateInfo.FlightExperimentName;
            Flighter = new FlighterModel();
            Flighter2 = new Flighter2Model();
            Helicopter = new HelicopterModel();
            Missile = new MissileModel();
            Title = _config.StringResource.FlightMapTitle;
            _mapRefreshInterval = _config.DataShowConfig.MapUiRefreshMs;
            _translateData = Ioc.Get<ITranslateData>();
            _hasSetPoints = new ObservableRangeCollection<bool>
            {
                false,false,false,false,false,false,false
            };
            SetDrawPara();
            BuildWswModelLocationString();
            InfoRenewInit();
        }

        public void SetDrawPara()
        {
            SetPointsEllipseRadius = JsonFileConfig.Instance.DataShowConfig.SetPointsEllipseRadius;
            SetPointsFontSize = JsonFileConfig.Instance.DataShowConfig.SetPointsFontSize;
            LocationStringFontSize = JsonFileConfig.Instance.DataShowConfig.LocationStringFontSize;
            SetPointsLineWidth = JsonFileConfig.Instance.DataShowConfig.SetPointsLineWidth;
        }

        /// <summary>
        /// 处理是否具有航路点
        /// </summary>
        public void DealHasSetPoints()
        {
            for (var i = 0; i < _hasSetPoints.Count; ++i)
            {
                try
                {
                    HasSetPoints[i] = SetPoints[i] != null;
                }
                catch
                {
                    
                }
            }
        }

        /// <summary>
        /// 坐标更新函数
        /// </summary>
        protected virtual void InfoRenewInit()
        {
            Task.Run(() =>
            {
                while(true)
                {
                    BuildWswModelLocationString();
                    Thread.Sleep(_mapRefreshInterval);
                }
            });
            //如果UDP没有接收到来自六自由度和720度平台的姿态坐标数据，就展示示例动作
            if(_translateData.TranslateInfo.IsConnect == false)
            {
                var index = _flightTaskIndex;
                if(index == 0)
                {
                    Task.Run(async () =>
                    {
                        var speed = 0.2;
                        Helicopter.MyMapPosition = new Point(10, 15);
                        Flighter.MyMapPosition = new Point(0, 0);
                        Flighter.Angle = 0;
                        Helicopter.Angle = 90;
                        await Task.Delay(100);
                        Ioc.Get<ISpeek>()?.SpeekAsync($"{_flightTaskName}实验开始飞行第一阶段");
                        while (true)
                        {
                            if (Helicopter.MyMapPosition.X >= 45)
                                break;
                            Helicopter.MyMapPosition = VectorPointHelper.PointOffset(Helicopter.MyMapPosition,
                                speed, 0);
                            Flighter.LocationString = $"{Flighter.Name}" +
                                 $"  {Flighter.MyMapInfoToString()};";
                            Helicopter.LocationString = $"{Helicopter.Name}" +
                                  $"  {Helicopter.MyMapInfoToString()};";
                            Missile.MyMapPosition = new Point(30, 30);
                            Thread.Sleep(_mapRefreshInterval);
                        }
                        Helicopter.Angle += 90;
                        Ioc.Get<ISpeek>()?.
                                     SpeekAsync($"{_flightTaskName}实验开始飞行第二阶段");
                        while (true)
                        {
                            if (Helicopter.MyMapPosition.Y >= 45)
                                break;
                            Helicopter.MyMapPosition = VectorPointHelper.PointOffset(Helicopter.MyMapPosition,
                                0, speed);
                            Flighter.LocationString = $"{Flighter.Name}" +
                                 $"  {Flighter.MyMapInfoToString()};";
                            Helicopter.LocationString = $"{Helicopter.Name}" +
                                  $"  {Helicopter.MyMapInfoToString()};";
                            Thread.Sleep(_mapRefreshInterval);
                        }
                        Helicopter.Angle += 90;
                        Ioc.Get<ISpeek>()?.
                                     SpeekAsync($"{_flightTaskName}实验开始飞行第三阶段");
                        while (true)
                        {
                            if (Helicopter.MyMapPosition.X <= 10)
                                break;
                            Helicopter.MyMapPosition = VectorPointHelper.PointOffset(Helicopter.MyMapPosition,
                                -speed, 0);
                            Flighter.LocationString = $"{Flighter.Name}" +
                                 $"  {Flighter.MyMapInfoToString()};";
                            Helicopter.LocationString = $"{Helicopter.Name}" +
                                  $"  {Helicopter.MyMapInfoToString()};";
                            Thread.Sleep(_mapRefreshInterval);
                        }
                        Helicopter.Angle += 90;
                        Ioc.Get<ISpeek>()?.
                                     SpeekAsync($"{_flightTaskName}实验开始飞行第四阶段");
                        while (true)
                        {
                            if (Helicopter.MyMapPosition.Y <= 18)
                                break;
                            Helicopter.MyMapPosition = VectorPointHelper.PointOffset(Helicopter.MyMapPosition,
                                0, -speed);
                            Flighter.LocationString = $"{Flighter.Name}" +
                                 $"  {Flighter.MyMapInfoToString()};";
                            Helicopter.LocationString = $"{Helicopter.Name}" +
                                  $"  {Helicopter.MyMapInfoToString()};";
                            Thread.Sleep(_mapRefreshInterval);
                        }
                        Helicopter.Angle += 90;
                        Ioc.Get<ISpeek>()?.
                                     SpeekAsync($"{_flightTaskName}实验开始飞行第五阶段");
                        while (true)
                        {
                            if (Helicopter.MyMapPosition.X >= 50)
                            {
                                Ioc.Get<ISpeek>()?.
                                    SpeekAsync($"{_helicopter.Name}成功完成{_flightTaskName}实验");
                                Ioc.Get<ITranslateData>().TranslateInfo.IsTest = false;
                                break;
                            }
                            Helicopter.MyMapPosition = VectorPointHelper.PointOffset(Helicopter.MyMapPosition,
                                speed, 0);
                            Flighter.LocationString = $"{Flighter.Name}" +
                                 $"  {Flighter.MyMapInfoToString()};";
                            Helicopter.LocationString = $"{Helicopter.Name}" +
                                  $"  {Helicopter.MyMapInfoToString()};";
                            Thread.Sleep(_mapRefreshInterval);
                        }
                    });
                }
                if(index == 1)
                {
                    Task.Run(async () =>
                    {
                        var i = 0.0f;
                        var flightCount = 0.0f;
                        var flighterSpeed = JsonFileConfig.Instance.
                            TestTrailRouteConfig.UnConnectedFlighterRotateSpeed;
                        var point1 = new Point(30, 30);
                        var point2 = new Point(25, 30);
                        var point3 = new Point(0, 0);
                        Helicopter.Angle = 180;
                        Flighter.Angle = 90;
                        await Task.Delay(100);
                        var config = JsonFileConfig.Instance;
                        while (true)
                        {
                            point1.X = config.MyFlighterInfo.InitMyPointX;
                            point1.Y = config.MyFlighterInfo.InitMyPointY;
                            point2.X = config.MyHelicopterInfo.InitMyPointX;
                            point2.Y = config.MyHelicopterInfo.InitMyPointY;
                            point3.X = config.MyFlighter2Info.InitMyPointX;
                            point3.Y = config.MyFlighter2Info.InitMyPointY;
                            Helicopter.Angle += 1;
                            if (Helicopter.Angle >= 360)
                                Helicopter.Angle = 0;
                            Flighter.Angle -= flighterSpeed;
                            if (Flighter.Angle <= 0)
                                Flighter.Angle = 360;
                            var j = NumberUtil.Deg2Rad(i);
                            var flightRad = NumberUtil.Deg2Rad(flightCount);

                            Flighter.MyMapPosition = new Point(point1.X + 28.2843 * Math.Sin(flightRad), point1.Y + 28.2843 * Math.Cos(flightRad));
                            Helicopter.MyMapPosition = new Point(point2.X + 10 * Math.Cos(j), point2.Y + 10 * Math.Sin(j));
                            Flighter.LocationString = $"{Flighter.Name}是否偏离航线：{Flighter.RouteState.ToLeftRightString()}" +
                                 $"  {Flighter.MyMapInfoToString()};";
                            Flighter2.LocationString = $"{Flighter2.Name} " +
                                 $"  {Flighter2.MyMapInfoToString()};";
                            Helicopter.LocationString = $"{Helicopter.Name}是否偏离航线：{Helicopter.RouteState.ToLeftRightString()}" +
                                  $"  {Helicopter.MyMapInfoToString()};";
                            Flighter2.MyMapPosition = point3;
                            Missile.MyMapPosition = new Point(-100, -100);
                            //Missile.LocationString = $"{Missile.Name}" +
                            //      $"  {Missile.MyMapInfoToString()};";

                            Thread.Sleep(_mapRefreshInterval);
                            i += 1;
                            flightCount += flighterSpeed;
                        }
                    });
                }
                if(index == 2)
                {
                    Task.Run(async () =>
                    {
                        var i = 0.0;
                        var point1 = new Point(30, 30);
                        var point2 = new Point(10, 10);
                        Flighter.MyMapPosition = point1;
                        Helicopter.MyMapPosition = point2;
                        Missile.MyMapPosition = new Point(0, 0);
                        Flighter.Angle += 225;
                        var deltax = 0.0;
                        var deltay = 0.0;
                        await Task.Delay(100);
                        while (true)
                        {
                            if (Flighter.MyMapPosition.X >= 69.8)
                            {
                                Ioc.Get<ISpeek>()?.
                                    SpeekAsync($"{_flighter.Name}成功完成{_flightTaskName}实验");
                                break;
                            }
                            else if (Flighter.MyMapPosition.X >= 49.9)
                            {
                                deltax =  40 - 20 * Math.Sin(i);
                                deltay = deltax;
                            }
                            else
                            {
                                deltax = 20 * Math.Sin(i);
                                deltay = deltax;
                            }
                            Flighter.MyMapPosition = new Point(point1.X + deltax, point1.Y + deltay);
                            Flighter.LocationString = $"{Flighter.Name}" +
                                 $"  {Flighter.MyMapInfoToString()};";
                            Helicopter.LocationString = $"{Helicopter.Name}" +
                                  $"  {Helicopter.MyMapInfoToString()};";      
                            Thread.Sleep(_mapRefreshInterval);
                            i += 0.02;
                            
                        }
                    });
                }
                if(index == 3)
                {
                    Task.Run(() =>
                    {
                        var i = 0.0f;
                        var point1 = new Point(35, 35);
                        var point2 = new Point(15, 15);
                        Flighter.Angle -= 90;
                        while (true)
                        {
                            Helicopter.Angle += 1;
                            if (Helicopter.Angle >= 360)
                                Helicopter.Angle = 0;
                            Flighter.Angle += 1;
                            if (Flighter.Angle >= 360)
                                Flighter.Angle = 0;
                            Flighter2.Angle += 1;
                            if (Flighter2.Angle >= 360)
                                Flighter2.Angle = 0;
                            var j = NumberUtil.Deg2Rad(i);
                            if(i > 500)
                            {
                                Ioc.Get<ISpeek>()?.
                                    SpeekAsync($"{_flighter.Name}成功完成{_flightTaskName}实验");
                                break;
                            }
                            Flighter.MyMapPosition = new Point(point1.X + 10 * Math.Cos(j), point1.Y + 10 * Math.Sin(j));
                            Flighter2.MyMapPosition = new Point(point1.X + 10 + 10 * Math.Cos(j), point1.Y + 10 + 10 * Math.Sin(j));
                            Helicopter.MyMapPosition = new Point(point2.X + 10 * Math.Cos(j), point2.Y + 10 * Math.Sin(j));
                            Flighter.LocationString = $"{Flighter.Name}" +
                                 $"  {Flighter.MyMapInfoToString()};";
                            Flighter2.LocationString = $"{Flighter2.Name}" +
                                 $"  {Flighter2.MyMapInfoToString()};";
                            Helicopter.LocationString = $"{Helicopter.Name}" +
                                  $"  {Helicopter.MyMapInfoToString()};";

                            Missile.MyMapPosition = new Point(50, 50);
                            Thread.Sleep(_mapRefreshInterval);
                            i += 1;
                        }
                    });
                }
                if(index == 4)
                {
                    //                     Task.Run(async () =>
                    //                     {
                    //                         var i = 0.0;
                    //                         var point1 = new Point(30, 50);
                    //                         var point2 = new Point(10, 10);
                    //                         Flighter.MyMapPosition = point1;
                    //                         Helicopter.MyMapPosition = point2;
                    //                         Missile.MyMapPosition = new Point(0, 0);
                    //                         Flighter.Angle += 135;
                    //                         var deltax = 0.0;
                    //                         var deltay = 0.0;
                    //                         await Task.Delay(100);
                    //                         while (true)
                    //                         {
                    //                             if (Flighter.MyMapPosition.X >= 69.8)
                    //                             {
                    //                                 Ioc.Get<ISpeek>()?.
                    //                                     SpeekAsync($"{_flighter.Name}成功完成{_flightTaskName}实验");
                    //                                 break;
                    //                             }
                    //                             else
                    //                             {
                    //                                 deltax = Math.Exp(i);
                    //                                 deltay = -deltax;
                    //                             }
                    //                             Flighter.MyMapPosition = new Point(point1.X + deltax, point1.Y + deltay);
                    //                             Flighter.LocationString = $"{Flighter.Name}" +
                    //                                  $"  {Flighter.MyMapInfoToString()};";
                    //                             Helicopter.LocationString = $"{Helicopter.Name}" +
                    //                                   $"  {Helicopter.MyMapInfoToString()};";
                    //                             Thread.Sleep(_mapRefreshInterval);
                    //                             i += 0.02;
                    // 
                    //                         }
                    //                     });
                    Task.Run(() =>
                    {
                        var i = 0.0f;
                        var flightCount = 0.0f;
                        var flighterSpeed = JsonFileConfig.Instance.
                            TestTrailRouteConfig.UnConnectedFlighterRotateSpeed;
                        var point1 = new Point(20, 20);
                        var point2 = new Point(60, 20);
                        Missile.MyMapPosition = new Point(-30, -30);
                        Missile.Angle = 90;
                        Helicopter.MyMapPosition = new Point(60, 30);
                        Flighter.MyMapPosition = point1;
                        Flighter.Angle = 90;
                        Helicopter.Angle = -90;
                        Thread.Sleep(5000);              
                        var x = 20.0;
                        var y = 20.0;
                        while (true)
                        {
                            
                            Helicopter.Angle += 1;
                            if (Helicopter.Angle >= 360)
                                Helicopter.Angle = 0;
                            var j = NumberUtil.Deg2Rad(i);
                            var flightRad = NumberUtil.Deg2Rad(flightCount);
                            Missile.MyMapPosition = new Point(x, y);
                            x += 0.33f;
                            Helicopter.MyMapPosition = new Point(point2.X - 10 * Math.Sin(j), point2.Y + 10 * Math.Cos(j));
                            Helicopter.LocationString = $"" +
                                  $"  {Helicopter.MyMapInfoToString()};";
                            Flighter.LocationString = $"" +
                                  $"  {Flighter.MyMapInfoToString()};";
                            Missile.LocationString = $"{Missile.Name}" +
                                  $"  {Missile.MyMapInfoToString()};";
                            if (Helicopter.MyMapPosition.Y <= 20)
                                break;
                            Thread.Sleep(_mapRefreshInterval);
                            i += 1;
                            flightCount += flighterSpeed;
                        }
                    });
                }
                if(index == 5)
                {
                    Task.Run(() =>
                    {
                        var i = 0.0f;
                        var flightCount = 0.0f;
                        var flighterSpeed = JsonFileConfig.Instance.
                            TestTrailRouteConfig.UnConnectedFlighterRotateSpeed;
                        var point1 = new Point(30, 30);
                        var point2 = new Point(25, 30);
                        Flighter.Angle -= 180;
                        while (true)
                        {
                            Helicopter.Angle += 1;
                            if (Helicopter.Angle >= 360)
                                Helicopter.Angle = 0;
                            Flighter.Angle -= flighterSpeed;
                            if (Flighter.Angle <= 0)
                                Flighter.Angle = 360;
                            var j = NumberUtil.Deg2Rad(i);
                            var flightRad = NumberUtil.Deg2Rad(flightCount);

                            Flighter.MyMapPosition = new Point(point1.X + 28.2843 * Math.Sin(flightRad), point1.Y + 28.2843 * Math.Cos(flightRad));
                            Helicopter.MyMapPosition = new Point(point2.X + 10 * Math.Cos(j), point2.Y + 10 * Math.Sin(j));
                            Flighter.LocationString = $"{Flighter.Name}是否偏离航线：{Flighter.RouteState.ToLeftRightString()}" +
                                 $"  {Flighter.MyMapInfoToString()};";
                            Helicopter.LocationString = $"{Helicopter.Name}是否偏离航线：{Helicopter.RouteState.ToLeftRightString()}" +
                                  $"  {Helicopter.MyMapInfoToString()};";

                            Missile.MyMapPosition = new Point(-100, -100);
                                                 //Missile.LocationString = $"{Missile.Name}" +
                                                 //      $"  {Missile.MyMapInfoToString()};";

                                                 Thread.Sleep(_mapRefreshInterval);
                            i += 1;
                            flightCount += flighterSpeed;
                        }
                    });

                }
                if(index == 6)
                {

                }
            }
                    
        }

        private float GenerateRandomSmallNumber() => (new Random().Next(3) - 1) / 100.0f;

        public void RefreshSetPoints()
        {
            var randx = GenerateRandomSmallNumber();
            var randy = GenerateRandomSmallNumber();
            var point = Flighter.MyMapPosition;
            var info = JsonFileConfig.Instance.MyFlighter2Info;
            Flighter.MyMapPosition = new Point(point.X + randx, point.Y + randy);
            point = Flighter2.MyMapPosition;
            Flighter2.MyMapPosition = new Point(randx + point.X, 
                randy + point.Y);
            point = Helicopter.MyMapPosition;
            Helicopter.MyMapPosition = new Point(point.X + randx, point.Y + randy);
            if (SetPoints == null)
                return;          
            var points = SetPoints.ToArray();
            SetPoints = new ObservableRangeCollection<Point>(points);               
        }

        /// <summary>
        /// 检测是否偏离航线
        /// </summary>
        public virtual void JudgeRouteTask()
        {
            if(_flightTaskIndex == 1 || _flightTaskIndex == 5)
            {
                Flighter.OutOfRouteSpeechControl(SetPoints);
                Helicopter.OutOfRouteSpeechControl(SetPoints);
            }
        }

        /// <summary>
        /// 更新位置和角度信息
        /// </summary>
        public virtual void BuildWswModelLocationString()
        {         
            if(_translateData.TranslateInfo.IsConnect == true)
            {
                var planeInfo = Ioc.Get<ITranslateData>().TranslateInfo;
                var index = planeInfo.FlightExperimentIndex;
                if(index == 5 || index == 1)
                {
                    Flighter.LocationString = $"{Flighter.Name}是否偏离航线：{Flighter.RouteState.ToLeftRightString()}" +
                            $"  {Flighter.WswModelInfoToString()};";
                    Flighter2.LocationString = $"{Flighter2.Name}是否偏离航线：{Flighter2.RouteState.ToLeftRightString()}" +
                            $"  {Flighter2.WswModelInfoToString()};";
                    Helicopter.LocationString = $"{Helicopter.Name}是否偏离航线：{Helicopter.RouteState.ToLeftRightString()}" +
                          $"  {Helicopter.WswModelInfoToString()};";
                }
                else
                {
                    Flighter.LocationString = $"{Flighter.Name}" +
                            $"  {Flighter.WswModelInfoToString()};";
                    Flighter2.LocationString = $"{Flighter2.Name}是否偏离航线：{Flighter2.RouteState.ToLeftRightString()}" +
                            $"  {Flighter2.WswModelInfoToString()};";
                    Helicopter.LocationString = $"{Helicopter.Name}" +
                          $"  {Helicopter.WswModelInfoToString()};";
                }
                Helicopter.Angle = (float)planeInfo.Helicopter.Yaw;
                Helicopter.MyMapPosition = new Point(planeInfo.Helicopter.X, planeInfo.Helicopter.Y);
                Flighter.Angle = (float)planeInfo.Flighter.Yaw;
                Flighter.MyMapPosition = new Point(planeInfo.Flighter.X, planeInfo.Flighter.Y);
                Flighter2.Angle = (float)planeInfo.Flighter2.Yaw;
                Flighter2.MyMapPosition = new Point(planeInfo.Flighter2.X, planeInfo.Flighter2.Y);

            }
        }

        /// <summary>
        /// 运行测试
        /// </summary>
        public void RunTest()
        {
            Title = VectorPointHelper.GetPointsLineVectorAngle(SetPoints).ToListString()
                + "  " + Flighter.RouteState.ToLeftRightString() + "  " + Flighter.AngleWithXAxes
                + $"nowIndex:{Flighter.NowSetPointsIndex}";
        }

    }
}
