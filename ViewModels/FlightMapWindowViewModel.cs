using System;
using System.Windows.Controls;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Prism.Commands;
using Prism.Mvvm;

using DuGu.NetFramework.Services;

using TeachingPlatformApp.Speech;
using TeachingPlatformApp.WswPlatform;
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
            if(_translateData.PlaneInfo.IsConnect == false)
            {
                Task.Run(() =>
                {
                    var i = 0.0f;
                    var random = new Random();
                    //var point1 = new Point(random.Next(80), random.Next(60));
                    var point1 = new Point(30, 30);
                    var point2 = new Point(random.Next(80), random.Next(60));
                    Flighter.Angle -= 180;
                    while (true)
                    {
                        var ram = new Random();
                        Helicopter.Angle += 1;
                        if (Helicopter.Angle >= 360)
                            Helicopter.Angle = 0;
                        Flighter.Angle -= 1;
                        if (Flighter.Angle <= 0)
                            Flighter.Angle = 360;
                        var j = NumberUtil.Deg2Rad(i);
                        Flighter.MyMapPosition = new Point(point1.X + 28.2843 * Math.Sin(j), point1.Y + 28.2843 * Math.Cos(j));
                        Helicopter.MyMapPosition = new Point(point2.X + 10 * Math.Cos(j), point2.Y + 10 * Math.Sin(j));
                        Flighter.LocationString = $"{Flighter.Name}是否偏离航线：{Flighter.RouteState.ToLeftRightString()}" +
                             $"  {Flighter.MyMapInfoToString()};";
                        Helicopter.LocationString = $"{Helicopter.Name}是否偏离航线：{Helicopter.RouteState.ToLeftRightString()}" +
                              $"  {Helicopter.MyMapInfoToString()};";
                        
                        Thread.Sleep(_mapRefreshInterval);
                        i += 1;
                    }
                });
            }
                    
        }

        /// <summary>
        /// 检测是否偏离航线
        /// </summary>
        public virtual void JudgeRouteTask()
        {
            Flighter.OutOfRouteSpeechControl(SetPoints);
            Helicopter.OutOfRouteSpeechControl(SetPoints);
        }

        /// <summary>
        /// 更新位置和角度信息
        /// </summary>
        public virtual void BuildWswModelLocationString()
        {         
            if(_translateData.PlaneInfo.IsConnect == true)
            {
                var planeInfo = Ioc.Get<ITranslateData>().PlaneInfo;
                Flighter.LocationString = $"{Flighter.Name}是否偏离航线：{Flighter.RouteState.ToLeftRightString()}" +
                             $"  {Flighter.WswModelInfoToString()};";
                Helicopter.LocationString = $"{Helicopter.Name}是否偏离航线：{Helicopter.RouteState.ToLeftRightString()}" +
                      $"  {Helicopter.WswModelInfoToString()};";
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
