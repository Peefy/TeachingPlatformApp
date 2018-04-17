using System;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Windows.Threading;

using Prism.Mvvm;

using DuGu.NetFramework.Services;

using TeachingPlatformApp.Communications;
using TeachingPlatformApp.Speech;
using TeachingPlatformApp.Utils;
using TeachingPlatformApp.WswPlatform;

namespace TeachingPlatformApp.Models
{
    public abstract class WswBaseModel : BindableBase, IWswDataStringShow, IRouteJudge, IDisposable
    {

        static protected private ISpeek _speeker;
        static protected private int _outOfRouteSpeechUpCount = 10;

        protected JsonFileConfig Config { get; set; }

        private string _name = "";
        /// <summary>
        /// 威视微通道模型名称
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private int _outOfRouteCount = 0;
        /// <summary>
        /// 检测是否偏离航路点次数
        /// </summary>
        public int OutOfRouteCount
        {
            get => _outOfRouteCount;
            set => SetProperty(ref _outOfRouteCount, value);
        }

        private string _locationString = "";
        /// <summary>
        /// 坐标显示字符串
        /// </summary>
        public string LocationString
        {
            get => _locationString;
            set => SetProperty(ref _locationString, value);
        }

        private float _angle = 0;
        /// <summary>
        /// 偏航角
        /// </summary>
        public float Angle
        {
            get => _angle;
            set => SetProperty(ref _angle, value);
        }

        /// <summary>
        /// 偏航角(与地图x轴的夹角)
        /// </summary>
        public double AngleWithXAxes => 
            NumberUtil.PutAngleIn(Angle - 90, minAngle: -180, maxAngle: 180);

        private Point _myMapPosition = default;
        /// <summary>
        /// 地图坐标
        /// </summary>
        public Point MyMapPosition
        {
            get => _myMapPosition;
            set => SetProperty(ref _myMapPosition, value);
        }

        private ObservableRangeCollection<Point> _trail;
        /// <summary>
        /// 轨迹
        /// </summary>
        public ObservableRangeCollection<Point> Trail
        {
            get => _trail;
            set => SetProperty(ref _trail, value);
        }

        private bool _isNotOutofRoute = true;
        /// <summary>
        /// 检测是否偏离航路点
        /// </summary>
        public bool IsNotOutofRoute
        {
            get => _isNotOutofRoute;
            set => SetProperty(ref _isNotOutofRoute, value);
        }

        private AngleWithLocation _wswAngleWithLocation;
        /// <summary>
        /// 视景中模型真实的经纬度坐标
        /// </summary>
        public AngleWithLocation WswAngleWithLocation
        {
            get => _wswAngleWithLocation;
            set => SetProperty(ref _wswAngleWithLocation, value);
        }

        private bool _isJudgeRoute = true;
        /// <summary>
        /// 是否检测偏离航路点
        /// </summary>
        public bool IsJudgeRoute
        {
            get => _isJudgeRoute;
            set => SetProperty(ref _isJudgeRoute, value);
        }

        private int _nowSetPointsIndex = 0;
        /// <summary>
        /// 当前经过航路点索引
        /// </summary>
        public int NowSetPointsIndex
        {
            get => _nowSetPointsIndex;
            set => SetProperty(ref _nowSetPointsIndex, value);
        }

        private int _lastSetPointsIndex = 0;
        /// <summary>
        /// 上一次经过航路点索引
        /// </summary>
        public int LastSetPointsIndex
        {
            get => _lastSetPointsIndex;
            set => SetProperty(ref _lastSetPointsIndex, value);
        }

        private RouteState _routeState = RouteState.Normal;
        /// <summary>
        /// 偏离航线的状态
        /// </summary>
        public RouteState RouteState
        {
            get => _routeState;
            set => SetProperty(ref _routeState, value);
        }

        private bool _isSuccess = false;
        /// <summary>
        /// 是否完成飞行实验
        /// </summary>
        public bool IsSuccess
        {
            get => _isSuccess;
            set => SetProperty(ref _isSuccess, value);
        }

        private bool _isVisible = true;
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        /// <summary>
        /// 飞行实验索引
        /// </summary>
        public int? FlightExperimentIndex => 
            Ioc.Get<ITranslateData>()?.TranslateInfo.FlightExperimentIndex;

        /// <summary>
        /// 是否具有航路点
        /// </summary>
        public bool HasSetPoints
        {
            get 
            {
                var index = FlightExperimentIndex ?? 0;
                if (index == 1 || index == 5)
                    return true;
                return false;
            }
        }

        public WswBaseModel()
        {

            Config = JsonFileConfig.Instance;
            WswAngleWithLocation = default;
            Trail = new ObservableRangeCollection<Point>();
            NowSetPointsIndex = Config.TestTrailRouteConfig.InitNowSetPointsIndex;
            _speeker = Ioc.Get<ISpeek>();
            _outOfRouteCount = Config.TestTrailRouteConfig.OutOfRouteSpeechUpCount;
        }

        ~WswBaseModel()
        {
            Dispose(false);
        }

        /// <summary>
        /// 真实坐标显示字符串
        /// </summary>
        /// <returns></returns>
        public virtual string WswModelInfoToString()
        {
            var digit = Config.DataShowConfig.PointShowDigit;
            var x = Math.Round(WswAngleWithLocation.X, digit);
            var y = Math.Round(WswAngleWithLocation.Y, digit);
            var z = Math.Round(WswAngleWithLocation.Z, digit);
            return $"坐标:({x},{y},{z})";
        }

        /// <summary>
        /// 地图坐标显示字符串
        /// </summary>
        /// <returns></returns>
        public virtual string MyMapInfoToString()
        {
            var digit = Config.DataShowConfig.PointShowDigit;
            var x = Math.Round(MyMapPosition.X, digit);
            var y = Math.Round(MyMapPosition.Y, digit);
            return $"坐标:({x},{y})";
        }

        /// <summary>
        /// 检测是否偏离航路点过程函数
        /// </summary>
        /// <param name="setPoints"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 检测航线状态过程函数
        /// </summary>
        /// <param name="setPoints"></param>
        /// <param name="isNotOutOfRoute"></param>
        /// <returns></returns>
        public virtual RouteState JudgeRouteState(ObservableRangeCollection<Point> setPoints, out bool isNotOutOfRoute)
        {
            isNotOutOfRoute = JudgeIsNotOutOfRoute(setPoints);
            var routeState = RouteState.Normal;
            var nowIndex = JudgeNowSetPointsIndex(setPoints);
            if (setPoints?.Count > 1 && isNotOutOfRoute == false)
            {
                var count = setPoints.Count;                
                var angle = 0.0;               
                if (nowIndex != count - 1)
                    angle = VectorPointHelper.GetThreePointsTwoLineAngle(setPoints[nowIndex], MyMapPosition, setPoints[nowIndex + 1]);
                else
                    angle = VectorPointHelper.GetThreePointsTwoLineAngle(setPoints[nowIndex], MyMapPosition, setPoints[0]);
                if (angle > 0 && angle <= 180)
                    routeState = RouteState.OutOfLeft;
                if (angle < 0 && angle >= -180)
                    routeState = RouteState.OutOfRight;
                if (angle == 0)
                    routeState = RouteState.Normal;
            }
            return routeState;
        }

        /// <summary>
        /// 航路点语音提示过程
        /// </summary>
        /// <param name="setPoints"></param>
        public virtual void OutOfRouteSpeechControl(ObservableRangeCollection<Point> setPoints)
        {
            if (setPoints == null || IsSuccess == true || IsJudgeRoute == false)
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

        /// <summary>
        /// 检测正在经过的航路点过程
        /// </summary>
        /// <param name="setPoints"></param>
        /// <returns></returns>
        public virtual int JudgeNowSetPointsIndex(ObservableRangeCollection<Point> setPoints)
        {
            var index = 0;
            var radius = JsonFileConfig.Instance.TestTrailRouteConfig.JudgeNowSetPointsIndexRadius;
            if (setPoints == null)
                return index;
            var count = setPoints.Count;
            for (var i = 0;i < count ;++i)
            {
                var distance = VectorPointHelper.GetTwoPointDistance(MyMapPosition, setPoints[i]);
                if (distance < radius)
                {
                    index = i;
                    if (index - NowSetPointsIndex == 1)
                        NowSetPointsIndex = index;
                    else if(NowSetPointsIndex == count - 1 && index == 0)
                    {
                        NowSetPointsIndex += 1;
                    }
                    if(NowSetPointsIndex != LastSetPointsIndex)
                    {
                        if (NowSetPointsIndex == count)
                        {
                            IsSuccess = true;
                            var flightExName = Ioc.Get<ITranslateData>().TranslateInfo.FlightExperimentName;
                            _speeker?.SpeekAsync($"{Name}成功完成了{flightExName}实验");
                            NowSetPointsIndex = 0;
                            Ioc.Get<ITranslateData>().TranslateInfo.IsTest = false;
                            break;
                        }
                        _speeker?.SpeekAsync($"{Name}已经成功通过第{NumberUtil.IntNumberToChineseString(NowSetPointsIndex + 1)}个航路点");
                    }
                    else
                    {
                        _speeker?.StopSpeek();
                    }                   
                }
            }
            LastSetPointsIndex = NowSetPointsIndex;
            return NowSetPointsIndex;
        }

        /// <summary>
        /// 更新坐标信息
        /// </summary>
        /// <param name="isConnect"></param>
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
