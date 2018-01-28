using System;
using System.Linq;
using System.Windows;

using Prism.Mvvm;

using DuGu.NetFramework.Services;

using TeachingPlatformApp.Speech;
using TeachingPlatformApp.Utils;
using TeachingPlatformApp.WswPlatform;

namespace TeachingPlatformApp.Models
{
    public abstract class WswBaseModel : BindableBase, IWswDataStringShow, IRouteJudge, IDisposable
    {

        static protected private ISpeek _speeker;
        static protected private int _outOfRouteSpeechUpCount = 10;

        public JsonFileConfig Config { get; set; }

        private string _name = "";
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private int _outOfRouteCount = 0;
        public int OutOfRouteCount
        {
            get => _outOfRouteCount;
            set => SetProperty(ref _outOfRouteCount, value);
        }

        private string _locationString = "";
        public string LocationString
        {
            get => _locationString;
            set => SetProperty(ref _locationString, value);
        }

        private float _angle = 0;
        public float Angle
        {
            get => _angle;
            set => SetProperty(ref _angle, value);
        }

        public double AngleWithXAxes => 
            NumberUtil.PutAngleIn(Angle - 90, minAngle: -180, maxAngle: 180);

        private Point _myMapPosition = default;
        public Point MyMapPosition
        {
            get => _myMapPosition;
            set => SetProperty(ref _myMapPosition, value);
        }

        private ObservableRangeCollection<Point> _trail;
        public ObservableRangeCollection<Point> Trail
        {
            get => _trail;
            set => SetProperty(ref _trail, value);
        }

        private bool _isNotOutofRoute = true;
        public bool IsNotOutofRoute
        {
            get => _isNotOutofRoute;
            set => SetProperty(ref _isNotOutofRoute, value);
        }

        private AngleWithLocation _wswAngleWithLocation;
        public AngleWithLocation WswAngleWithLocation
        {
            get => _wswAngleWithLocation;
            set => SetProperty(ref _wswAngleWithLocation, value);
        }

        private bool _isJudgeRoute = true;
        public bool IsJudgeRoute
        {
            get => _isJudgeRoute;
            set => SetProperty(ref _isJudgeRoute, value);
        }

        private int _nowSetPointsIndex = -1;
        public int NowSetPointsIndex
        {
            get => _nowSetPointsIndex;
            set => SetProperty(ref _nowSetPointsIndex, value);
        }

        private int _lastSetPointsIndex = 0;
        public int LastSetPointsIndex
        {
            get => _lastSetPointsIndex;
            set => SetProperty(ref _lastSetPointsIndex, value);
        }

        private RouteState _routeState = RouteState.Normal;
        public RouteState RouteState
        {
            get => _routeState;
            set => SetProperty(ref _routeState, value);
        }

        public WswBaseModel()
        {       
            Config = JsonFileConfig.Instance;
            WswAngleWithLocation = default;
            Trail = new ObservableRangeCollection<Point>();
            _speeker = Ioc.Get<ISpeek>();
            _outOfRouteCount = Config.TestTrailRouteConfig.OutOfRouteSpeechUpCount;
        }

        ~WswBaseModel()
        {
            Dispose(false);
        }

        public virtual string WswModelInfoToString()
        {
            var digit = Config.DataShowConfig.PointShowDigit;
            var x = Math.Round(WswAngleWithLocation.X, digit);
            var y = Math.Round(WswAngleWithLocation.Y, digit);
            var z = Math.Round(WswAngleWithLocation.Z, digit);
            return $"坐标:({x},{y},{z})";
        }

        public virtual string MyMapInfoToString()
        {
            var digit = Config.DataShowConfig.PointShowDigit;
            var x = Math.Round(MyMapPosition.X, digit);
            var y = Math.Round(MyMapPosition.Y, digit);
            return $"坐标:({x},{y})";
        }

        public virtual bool JudgeIsNotOutOfRoute(ObservableRangeCollection<Point> setPoints)
        {

            var point = this.MyMapPosition;
            var angle = Config.TestTrailRouteConfig.OutOfRouteAngle;
            var distanceLines = VectorPointHelper.
                    GetPointToAllSetPointsLineDistance(point,
                    setPoints.ToList());
            var distancePoints = VectorPointHelper.
                GetPointToAllSetPointsDistance(point,
                setPoints.ToList());
            if (point == null || distanceLines?.Count < 1 && distancePoints.Count < 1)
            {
                return false;
            }
            var minDistance = Config.TestTrailRouteConfig.OutOfRouteDistance;
            var nowMinLineDistance = distanceLines.Min();
            var nowMinPointDistance = distancePoints.Min();
            if (nowMinLineDistance <= minDistance)
            {
                var indexDistanceMin = distanceLines.IndexOf(nowMinLineDistance);
                Point point1;
                Point point2;
                if (indexDistanceMin == setPoints.Count - 1)
                {
                    point1 = setPoints[0];
                    point2 = setPoints[setPoints.Count - 1];
                }
                else
                {
                    point1 = setPoints[indexDistanceMin];
                    point2 = setPoints[indexDistanceMin + 1];
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

        public virtual RouteState JudgeRouteState(ObservableRangeCollection<Point> setPoints, out bool isNotOutOfRoute)
        {
            isNotOutOfRoute = JudgeIsNotOutOfRoute(setPoints);
            var routeState = isNotOutOfRoute == true ? RouteState.Normal : RouteState.OutOfLeft;
            if (setPoints?.Count > 1 && isNotOutOfRoute == false)
            {
                var count = setPoints.Count;                
                var angle = 0.0;
                NowSetPointsIndex = JudgeNowSetPointsIndex(setPoints);
                var nowIndex = NowSetPointsIndex;
                if (NowSetPointsIndex != count)
                    angle = VectorPointHelper.GetThreePointsTwoLineAngle(setPoints[nowIndex], MyMapPosition, setPoints[nowIndex + 1]);
                else
                    angle = VectorPointHelper.GetThreePointsTwoLineAngle(setPoints[nowIndex], MyMapPosition, setPoints[0]);
                if (angle >= 0 && angle <= 180)
                    routeState = RouteState.OutOfLeft;
                if (angle <= 0 && angle >= -180)
                    routeState = RouteState.OutOfRight;
            }
            return routeState;
        }

        public virtual void OutOfRouteSpeechControl(ObservableRangeCollection<Point> setPoints)
        {
            if (setPoints == null)
                return;
            //false代表偏离航线，true代表没有偏离航线.
            RouteState =  JudgeRouteState(setPoints, out var isNotOutofRoute);
            IsNotOutofRoute = isNotOutofRoute;
            if (IsNotOutofRoute == false)
            {
                this.OutOfRouteCount++;
            }
            else
            {
                this.OutOfRouteCount = 0;
                _speeker?.StopSpeek();
            }            
            if (OutOfRouteCount >= _outOfRouteSpeechUpCount)
            {
                OutOfRouteCount = 0;
                if(RouteState != RouteState.Normal)
                {
                    var leftRightStr = RouteState == RouteState.OutOfLeft ? "向左" : "向右";
                    _speeker?.SpeekAsync(Name + leftRightStr + Config.SpeechConfig.SpeechTextOutofRoute);
                }
            }
            else
            {
                _speeker?.StopSpeek();
            }
        }

        public virtual int JudgeNowSetPointsIndex(ObservableRangeCollection<Point> setPoints)
        {
            var index = 0;
            var radius = JsonFileConfig.Instance.TestTrailRouteConfig.JudgeNowSetPointsIndexRadius;
            if (setPoints == null)
                return index;
            for(var i = 0;i < setPoints.Count ;++i)
            {
                var distance = VectorPointHelper.GetTwoPointDistance(MyMapPosition, setPoints[i]);
                if (distance < radius)
                {
                    index = i;
                    if (index - NowSetPointsIndex == 1)
                        NowSetPointsIndex = index;
                    if(NowSetPointsIndex != LastSetPointsIndex)
                    {
                        _speeker.SpeekAsync($"{Name}已经成功通过第{NowSetPointsIndex + 1}个航路点");
                    }
                    else
                    {
                        _speeker.StopSpeek();
                    }
                }
            }
            LastSetPointsIndex = NowSetPointsIndex;
            return NowSetPointsIndex;
        }

        public void RenewLocationInfo(bool isConnect)
        {
            if(isConnect == false)
            {
                LocationString = $"{Name}是否偏离航线：{NumberUtil.BoolToString(IsNotOutofRoute)}" +
                              $"  {MyMapInfoToString()};";
            }
            else
            {
                LocationString = $"{Name}是否偏离航线：{NumberUtil.BoolToString(IsNotOutofRoute)}" +
                      $"  {WswModelInfoToString()};";
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                   
                }
                if (Trail != null)
                {
                    Trail.Clear();
                    Trail = null;
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }

    public interface IWswDataStringShow
    {
        string WswModelInfoToString();
        string MyMapInfoToString();
    }

    public interface IRouteJudge
    {
        void OutOfRouteSpeechControl(ObservableRangeCollection<Point> setPoints);
        bool JudgeIsNotOutOfRoute(ObservableRangeCollection<Point> setPoints);      
        int JudgeNowSetPointsIndex(ObservableRangeCollection<Point> setPoints);
        RouteState JudgeRouteState(ObservableRangeCollection<Point> setPoints, out bool isNotOutOfRoute);
    }

}
