using System;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Prism.Commands;
using Prism.Mvvm;

using DuGu.NetFramework.Services;

using TeachingPlatformApp.WswPlatform;
using TeachingPlatformApp.Utils;
using TeachingPlatformApp.Models;
using TeachingPlatformApp.Communications;

namespace TeachingPlatformApp.ViewModels
{
    public class FlightMapWindowViewModel : BindableBase
    {

        ITranslateData _translateData;
        JsonFileConfig _config;

        string _helicopterName = "直升机";
        string _flighterName = "战斗机";
        int _mapRefreshInterval = 30;

        private string _title = "地图";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _flightLocationString;
        public string FlightLocationString
        {
            get { return _flightLocationString; }
            set { SetProperty(ref _flightLocationString, value); }
        }

        private float _helicopterAngle = 180;
        public float HelicopterAngle
        {
            get => _helicopterAngle;
            set => SetProperty(ref _helicopterAngle, value);
        }

        private float _flighterAngle = 90;
        public float FlighterAngle
        {
            get => _flighterAngle;
            set => SetProperty(ref _flighterAngle, value);
        }

        private Point _helicopterPosition;
        public Point HelicopterPosition
        {
            get => _helicopterPosition;
            set => SetProperty(ref _helicopterPosition, value);
        }

        private Point _flighterPosition;
        public Point FlighterPosition
        {
            get => _flighterPosition;
            set => SetProperty(ref _flighterPosition, value);
        }

        public Point LastFlighterPosition;

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

        protected ObservableRangeCollection<Point> _flighterTrail;
        public ObservableRangeCollection<Point> FlighterTrail
        {
            get => _flighterTrail;
            set => SetProperty(ref _flighterTrail, value);
        }

        protected ObservableRangeCollection<Point> _helicopterTrail;
        public ObservableRangeCollection<Point> HelicopterTrail
        {
            get => _helicopterTrail;
            set => SetProperty(ref _helicopterTrail, value);
        }

        public ObservableRangeCollection<Point> HelicopterTrailTempList { get; set; }
        public ObservableRangeCollection<Point> FlighterTrailTempList { get; set; }

        public DelegateCommand ClearTrailCommand { get; set; }

        public FlightMapWindowViewModel()
        {
            _config = JsonFileConfig.ReadFromFile();
            var resource = _config.StringResource;
            Title = resource.FlightMapTitle;
            _helicopterName = resource.HelicopterName;
            _flighterName = resource.FlighterName;
            _mapRefreshInterval = _config.DataShowConfig.MapUiRefreshMs;
            _translateData = Ioc.Get<ITranslateData>();
            _hasSetPoints = new ObservableRangeCollection<bool>
            {
                false,false,false,false,false,false,false
            };
            TrailInit();
            BuildFlightLocationString();
            InfoRenewInit();
            CommandInit();
        }

        private void CommandInit()
        {
            ClearTrailCommand = new DelegateCommand(() =>
            {
                FlighterTrail.Clear();
            });
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
                    BuildFlightLocationString();
                    Thread.Sleep(_mapRefreshInterval);
                }
            });
            if(_translateData.PlaneInfo.IsConnect == false)
            {
                Task.Run(() =>
                {
                    var i = 0.0f;
                    var random = new Random();
                    var point1 = new Point(random.Next(90), random.Next(70));
                    var point2 = new Point(random.Next(90), random.Next(70));
                    while (true)
                    {
                        var ram = new Random();
                        HelicopterAngle += 1;
                        if (HelicopterAngle >= 360)
                            HelicopterAngle = 0;
                        FlighterAngle -= 1;
                        if (FlighterAngle <= 0)
                            FlighterAngle = 360;
                        var j = StructHelper.Deg2Rad(i);
                        FlighterPosition = new Point(point1.X + 10 * Math.Sin(j), point1.Y + 10 * Math.Cos(j));
                        HelicopterPosition = new Point(point2.X + 10 * Math.Cos(j), point2.Y + 10 * Math.Sin(j));
                        FlightLocationString = $"{_flighterName}{PlaneInfoToString(FlighterPosition)};" +
                            $"{_helicopterName}{PlaneInfoToString(HelicopterPosition)}";
                        Thread.Sleep(_mapRefreshInterval);
                        i += 1;
                    }
                });
            }
            if(_setPoints != null)
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                        try
                        {
                            JudgeRouteTask();
                        }
                        catch (Exception ex)
                        {
                            LogAndConfig.Log.Error(ex);
                        }
                        
                    }
                });
            }
        }

        /// <summary>
        /// 检测是否偏离航线
        /// </summary>
        public virtual void JudgeRouteTask()
        {
            var setPointsCount = SetPoints.Count;
        }

        public void TrailInit()
        {
            FlighterTrail = new ObservableRangeCollection<Point>();
            HelicopterTrail = new ObservableRangeCollection<Point>();
            FlighterTrailTempList = new ObservableRangeCollection<Point>();
            HelicopterTrailTempList = new ObservableRangeCollection<Point>();
        }

        public virtual void BuildFlightLocationString()
        {         
            if(_translateData.PlaneInfo.IsConnect == true)
            {
                var planeInfo = Ioc.Get<ITranslateData>().PlaneInfo;
                FlightLocationString = $"{_helicopterName}{PlaneInfoToString(planeInfo.Helicopter)};" +
                        $"{_flighterName}{PlaneInfoToString(planeInfo.Flighter)}";
                HelicopterAngle = (float)planeInfo.Helicopter.Yaw;
                FlighterAngle = (float)planeInfo.Flighter.Yaw;
                FlighterPosition = new Point(planeInfo.Flighter.X, planeInfo.Flighter.Y);
                HelicopterPosition = new Point(planeInfo.Helicopter.X, planeInfo.Helicopter.Y);
            }
        }

        private string PlaneInfoToString(AngleWithLocation angleWithLocation)
        {
            return $"坐标:({angleWithLocation.X},{angleWithLocation.Y},{angleWithLocation.Z})";
        }

        private string PlaneInfoToString(Point point)
        {
            var digit = _config.DataShowConfig.PointShowDigit;
            var x = Math.Round(point.X, digit);
            var y = Math.Round(point.Y, digit);
            return $"坐标:({x},{y})";
        }

    }
}
