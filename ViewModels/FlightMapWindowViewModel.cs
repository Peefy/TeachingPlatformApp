using System;
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

        protected ITranslateData _translateData;      
        protected JsonFileConfig _config;

        int _mapRefreshInterval = 30;
        int _flightTaskIndex = 0;
        string _flightTaskName = "";

        private string _title = "地图";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private FlighterModel _flighter;
        public FlighterModel Flighter
        {
            get => _flighter;
            set => SetProperty(ref _flighter, value);
        }

        private HelicopterModel _helicopter;
        public HelicopterModel Helicopter
        {
            get => _helicopter;
            set => SetProperty(ref _helicopter, value);
        }

        private MissileModel _missile;
        public MissileModel Missile
        {
            get => _missile;
            set => SetProperty(ref _missile, value);
        }

        protected ObservableRangeCollection<Point> _setPoints;
        public ObservableRangeCollection<Point> SetPoints
        {
            get => _setPoints;
            set => SetProperty(ref _setPoints, value);
        }

        protected ObservableRangeCollection<bool> _hasSetPoints;
        public ObservableRangeCollection<bool> HasSetPoints
        {
            get => _hasSetPoints;
            set => SetProperty(ref _hasSetPoints, value);
        }

        protected string _figurePathString = "M 20,20 L 120,120 320,420";
        public string FigurePathString
        {
            get => _figurePathString;
            set => SetProperty(ref _figurePathString, value);
        }

        private double _setPointsFontSize = 20;
        public double SetPointsFontSize
        {
            get => _setPointsFontSize;
            set => SetProperty(ref _setPointsFontSize, value);
        }

        private double _setPointsEllipseRadius = 6;
        public double SetPointsEllipseRadius
        {
            get => _setPointsEllipseRadius;
            set => SetProperty(ref _setPointsEllipseRadius, value);
        }

        private double _locationStringFontSize = 22;
        public double LocationStringFontSize
        {
            get => _locationStringFontSize;
            set => SetProperty(ref _locationStringFontSize, value);
        }

        private double _setPointsLineWidth = 3;
        public double SetPointsLineWidth
        {
            get => _setPointsLineWidth;
            set => SetProperty(ref _setPointsLineWidth, value);
        }

        private Thickness _drawMargin = new Thickness(0);
        public Thickness DrawMargin
        {
            get => _drawMargin;
            set => SetProperty(ref _drawMargin, value);
        }

        public FlightMapWindowViewModel()
        {
            _config = JsonFileConfig.ReadFromFile();
            _flightTaskIndex = Ioc.Get<ITranslateData>().TranslateInfo.FlightExperimentIndex;
            _flightTaskName = Ioc.Get<ITranslateData>().TranslateInfo.FlightExperimentName;
            Flighter = new FlighterModel();
            Helicopter = new HelicopterModel();
            Missile = new MissileModel();
            Title = _config.StringResource.FlightMapTitle;
            _mapRefreshInterval = _config.DataShowConfig.MapUiRefreshMs;
            _translateData = Ioc.Get<ITranslateData>();
            _hasSetPoints = new ObservableRangeCollection<bool>
            {
                false,false,false,false,false,false,false
            };
            SetPointsFontSize = _config.DataShowConfig.SetPointsFontSize;
            LocationStringFontSize = _config.DataShowConfig.LocationStringFontSize;
            SetPointsLineWidth = _config.DataShowConfig.SetPointsLineWidth;
            BuildWswModelLocationString();
            InfoRenewInit();
        }

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
                        Flighter.Angle -= 180;
                        Helicopter.Angle -= 90;
                        await Task.Delay(100);
                        Ioc.Get<ISpeek>()?.
                                    SpeekAsync($"{_flightTaskName}实验开始飞行第一边");
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
                        Helicopter.Angle += 45;
                        Flighter.Angle += 45;
                        speed /= 1.4142;
                        Ioc.Get<ISpeek>()?.
                                     SpeekAsync($"{_flightTaskName}实验开始飞行第二边");
                        while (true)
                        {
                            if (Helicopter.MyMapPosition.Y >= 30)
                                break;
                            Helicopter.MyMapPosition = VectorPointHelper.PointOffset(Helicopter.MyMapPosition,
                                speed, speed);
                            Flighter.LocationString = $"{Flighter.Name}" +
                                 $"  {Flighter.MyMapInfoToString()};";
                            Helicopter.LocationString = $"{Helicopter.Name}" +
                                  $"  {Helicopter.MyMapInfoToString()};";
                            Thread.Sleep(_mapRefreshInterval);
                        }
                        Helicopter.Angle += 90;
                        Ioc.Get<ISpeek>()?.
                                     SpeekAsync($"{_flightTaskName}实验开始飞行第三边");
                        while (true)
                        {
                            if (Helicopter.MyMapPosition.X <= 45)
                                break;
                            Helicopter.MyMapPosition = VectorPointHelper.PointOffset(Helicopter.MyMapPosition,
                                -speed, speed);
                            Flighter.LocationString = $"{Flighter.Name}" +
                                 $"  {Flighter.MyMapInfoToString()};";
                            Helicopter.LocationString = $"{Helicopter.Name}" +
                                  $"  {Helicopter.MyMapInfoToString()};";
                            Thread.Sleep(_mapRefreshInterval);
                        }
                        Helicopter.Angle += 45;
                        speed *= 1.4142;
                        Ioc.Get<ISpeek>()?.
                                     SpeekAsync($"{_flightTaskName}实验开始飞行第四边");
                        while (true)
                        {
                            if (Helicopter.MyMapPosition.X <= 15)
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
                                     SpeekAsync($"{_flightTaskName}实验开始飞行第五边");
                        while (true)
                        {
                            if (Helicopter.MyMapPosition.Y <= 10)
                            {
                                Ioc.Get<ISpeek>()?.
                                    SpeekAsync($"{_helicopter.Name}成功完成{_flightTaskName}实验");
                                Ioc.Get<ITranslateData>().TranslateInfo.IsTest = false;
                                break;
                            }
                            Helicopter.MyMapPosition = VectorPointHelper.PointOffset(Helicopter.MyMapPosition,
                                0, -speed);
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
                        Flighter.Angle -= 180;
                        await Task.Delay(100);
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
                            var j = NumberUtil.Deg2Rad(i);
                            if(i > 500)
                            {
                                Ioc.Get<ISpeek>()?.
                                    SpeekAsync($"{_flighter.Name}成功完成{_flightTaskName}实验");
                                break;
                            }
                            Flighter.MyMapPosition = new Point(point1.X + 10 * Math.Cos(j), point1.Y + 10 * Math.Sin(j));
                            Helicopter.MyMapPosition = new Point(point2.X + 10 * Math.Cos(j), point2.Y + 10 * Math.Sin(j));
                            Flighter.LocationString = $"{Flighter.Name}" +
                                 $"  {Flighter.MyMapInfoToString()};";
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
                    Task.Run(async () =>
                    {
                        var i = 0.0;
                        var point1 = new Point(30, 50);
                        var point2 = new Point(10, 10);
                        Flighter.MyMapPosition = point1;
                        Helicopter.MyMapPosition = point2;
                        Missile.MyMapPosition = new Point(0, 0);
                        Flighter.Angle += 135;
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
                            else
                            {
                                deltax = Math.Exp(i);
                                deltay = -deltax;
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
                    Helicopter.LocationString = $"{Helicopter.Name}是否偏离航线：{Helicopter.RouteState.ToLeftRightString()}" +
                          $"  {Helicopter.WswModelInfoToString()};";
                }
                else
                {
                    Flighter.LocationString = $"{Flighter.Name}" +
                            $"  {Flighter.WswModelInfoToString()};";
                    Helicopter.LocationString = $"{Helicopter.Name}" +
                          $"  {Helicopter.WswModelInfoToString()};";
                }
                Helicopter.Angle = (float)planeInfo.Helicopter.Yaw;
                Flighter.Angle = (float)planeInfo.Flighter.Yaw;
                Flighter.MyMapPosition = new Point(planeInfo.Flighter.X, planeInfo.Flighter.Y);
                Helicopter.MyMapPosition = new Point(planeInfo.Helicopter.X, planeInfo.Helicopter.Y);
            }
        }

        public void RunTest()
        {
            Title = VectorPointHelper.GetPointsLineVectorAngle(SetPoints).ToListString()
                + "  " + Flighter.RouteState.ToLeftRightString() + "  " + Flighter.AngleWithXAxes
                + $"nowIndex:{Flighter.NowSetPointsIndex}";
        }

    }
}
