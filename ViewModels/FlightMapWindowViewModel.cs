using System;
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
using TeachingPlatformApp.Utils;
using TeachingPlatformApp.Models;
using TeachingPlatformApp.Communications;

namespace TeachingPlatformApp.ViewModels
{
    public class FlightMapWindowViewModel : BindableBase
    {

        ITranslateData _translateData;
        JsonFileConfig _config;
        ISpeek _speeker;

        string _helicopterName = "直升机";
        string _flighterName = "战斗机";
        int _mapRefreshInterval = 30;

        int _flighterOutOfRouteCount = 0;
        int _helicopterOutOfRouteCount = 0;
        int _outOfRouteSpeechUpCount = 5;

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

        private string _helicopterLocationString;
        public string HelicopterLocationString
        {
            get { return _helicopterLocationString; }
            set { SetProperty(ref _helicopterLocationString, value); }
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

        protected RouteState _routeState = RouteState.Normal;
        public RouteState RouteState
        {
            get => _routeState;
            set => SetProperty(ref _routeState, value);
        }

        protected bool _helicopterIsOutofRoute;
        public bool HelicopterIsOutofRoute
        {
            get => _helicopterIsOutofRoute;
            set => SetProperty(ref _helicopterIsOutofRoute, value);
        }

        protected bool _flighterIsOutofRoute;
        public bool FlighterIsOutofRoute
        {
            get => _flighterIsOutofRoute;
            set => SetProperty(ref _flighterIsOutofRoute, value);
        }

        private double _setPointsFontSize = 20;
        public double SetPointsFontSize
        {
            get => _setPointsFontSize;
            set => SetProperty(ref _setPointsFontSize, value);
        }

        public DelegateCommand ClearTrailCommand { get; set; }

        public FlightMapWindowViewModel()
        {
            _config = JsonFileConfig.ReadFromFile();
            _outOfRouteSpeechUpCount = _config.TestTrailRouteConfig.OutOfRouteSpeechUpCount;
            _speeker = Ioc.Get<ISpeek>();
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
            SetPointsFontSize = _config.DataShowConfig.SetPointsFontSize;
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
                    var point1 = new Point(random.Next(80), random.Next(60));
                    var point2 = new Point(random.Next(80), random.Next(60));
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
                        FlightLocationString = $"{_flighterName}是否偏离航线：{NumberUtil.BoolToString(FlighterIsOutofRoute)}" +
                             $"  {PlaneInfoToString(FlighterPosition)};";
                        HelicopterLocationString = $"{_helicopterName}是否偏离航线：{NumberUtil.BoolToString(HelicopterIsOutofRoute)}" +
                              $"  {PlaneInfoToString(HelicopterPosition)};";
                        Thread.Sleep(_mapRefreshInterval);
                        i += 1;
                    }
                });
            }
            Task.Run(() =>
            {
                var interval = _config.TestTrailRouteConfig.OutOfRouteTestIntervalMs;
                while (true)
                {
                    try
                    {
                        JudgeRouteTask();
                        Thread.Sleep(interval);
                    }
                    catch (Exception ex)
                    {
                        LogAndConfig.Log.Error(ex);
                    }
                }
            });
            
        }

        /// <summary>
        /// 检测是否偏离航线
        /// </summary>
        public virtual void JudgeRouteTask()
        {
            if (_setPoints == null)
                return;
            var @switch = _config.TestTrailRouteConfig.TestSwitch;
            var angle = _config.TestTrailRouteConfig.OutOfRouteAngle;
            var distancesFlighterLine = new List<double>();
            var distanceFlighterPoint = new List<double>();
            var distancesHelicopterLine = new List<double>();
            var distancesHelicopterPoint = new List<double>();
            if (@switch == 0)
            {
                distancesFlighterLine = VectorPointHelper.
                    GetPointToAllSetPointsLineDistance(FlighterPosition,
                    SetPoints.ToList());
                distanceFlighterPoint = VectorPointHelper.
                    GetPointToAllSetPointsDistance(FlighterPosition,
                    SetPoints.ToList());
                FlighterIsOutofRoute = JudgeIsOutOfRoute(FlighterPosition, 
                    distancesFlighterLine, distanceFlighterPoint);

                OutOfRouteSpeechControlFlighter();
            }
            else if(@switch == 1)
            {
                distancesHelicopterLine = VectorPointHelper.
                    GetPointToAllSetPointsLineDistance(HelicopterPosition,
                    SetPoints.ToList());
                distancesHelicopterPoint = VectorPointHelper.
                    GetPointToAllSetPointsDistance(FlighterPosition,
                    SetPoints.ToList());
                HelicopterIsOutofRoute = JudgeIsOutOfRoute(HelicopterPosition, 
                    distancesHelicopterLine, distancesHelicopterPoint);

                OutOfRouteSpeechControlHelicopter();

            }
            else
            {
                distancesFlighterLine = VectorPointHelper.
                    GetPointToAllSetPointsLineDistance(FlighterPosition,
                    SetPoints.ToList());
                distanceFlighterPoint = VectorPointHelper.
                    GetPointToAllSetPointsDistance(FlighterPosition,
                    SetPoints.ToList());

                distancesHelicopterLine = VectorPointHelper.
                    GetPointToAllSetPointsLineDistance(HelicopterPosition,
                    SetPoints.ToList());
                distancesHelicopterPoint = VectorPointHelper.
                    GetPointToAllSetPointsDistance(HelicopterPosition,
                    SetPoints.ToList());
                FlighterIsOutofRoute = JudgeIsOutOfRoute(FlighterPosition, 
                    distancesFlighterLine, distanceFlighterPoint);
                HelicopterIsOutofRoute = JudgeIsOutOfRoute(HelicopterPosition,
                    distancesHelicopterLine, distancesHelicopterPoint);

                OutOfRouteSpeechControlFlighter();
                OutOfRouteSpeechControlHelicopter();

            }

        }
        private void OutOfRouteSpeechControlFlighter()
        {
            //false代表偏离航线，true代表没有偏离航线.
            if (FlighterIsOutofRoute == false)
                this._flighterOutOfRouteCount++;
            
            if (_flighterOutOfRouteCount >= _outOfRouteSpeechUpCount)
            {
                _flighterOutOfRouteCount = 0;
                _speeker?.SpeekAsync(_flighterName + _config.SpeechConfig.SpeechTextOutofRoute);
            }
            
        }

        private void OutOfRouteSpeechControlHelicopter()
        {
            //false代表偏离航线，true代表没有偏离航线.
            if (HelicopterIsOutofRoute == false)
                this._helicopterOutOfRouteCount++;
            if (_helicopterOutOfRouteCount >= _outOfRouteSpeechUpCount)
            {
                _helicopterOutOfRouteCount = 0;
                _speeker?.SpeekAsync(_helicopterName + _config.SpeechConfig.SpeechTextOutofRoute);
            }
        }

        public bool JudgeIsOutOfRoute(Point point, IList<double> distanceLines, IList<double> distancePoints)
        {
            if(point == null || distanceLines?.Count < 1 && distancePoints.Count < 1)
            {
                return false;
            }
            var minDistance = _config.TestTrailRouteConfig.OutOfRouteDistance;
            var nowMinLineDistance = distanceLines.Min();
            var nowMinPointDistance = distancePoints.Min();
            if (nowMinLineDistance <= minDistance)
            {
                var indexDistanceMin = distanceLines.IndexOf(nowMinLineDistance);
                Point point1;
                Point point2;
                if(indexDistanceMin == SetPoints.Count - 1)
                {
                    point1 = SetPoints[0];
                    point2 = SetPoints[SetPoints.Count - 1];
                }
                else
                {
                    point1 = SetPoints[indexDistanceMin];
                    point2 = SetPoints[indexDistanceMin + 1];
                }
                if (VectorPointHelper.JudgePointInParallelogram(point, point1, point2)) 
                {
                    return true;
                }
            }
            if (nowMinPointDistance <= minDistance)
                return true;
            return false;
        }

        public virtual void BuildFlightLocationString()
        {         
            if(_translateData.PlaneInfo.IsConnect == true)
            {
                var planeInfo = Ioc.Get<ITranslateData>().PlaneInfo;
                FlightLocationString = $"{_flighterName}是否偏离航线：{NumberUtil.BoolToString(FlighterIsOutofRoute)}" +
                             $"  {PlaneInfoToString(planeInfo.Flighter)};";
                HelicopterLocationString = $"{_helicopterName}是否偏离航线：{NumberUtil.BoolToString(HelicopterIsOutofRoute)}" +
                      $"  {PlaneInfoToString(planeInfo.Helicopter)};";
                HelicopterAngle = (float)planeInfo.Helicopter.Yaw;
                FlighterAngle = (float)planeInfo.Flighter.Yaw;
                FlighterPosition = new Point(planeInfo.Flighter.X, planeInfo.Flighter.Y);
                HelicopterPosition = new Point(planeInfo.Helicopter.X, planeInfo.Helicopter.Y);
            }
        }

        private string PlaneInfoToString(AngleWithLocation angleWithLocation)
        {
            var digit = _config.DataShowConfig.PointShowDigit;
            var x = Math.Round(angleWithLocation.X, digit);
            var y = Math.Round(angleWithLocation.Y, digit);
            var z = Math.Round(angleWithLocation.Z, digit);
            return $"坐标:({x},{y},{z})";
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
