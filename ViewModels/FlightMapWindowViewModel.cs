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

        private float _helicopterAngle = 300;
        public float HelicopterAngle
        {
            get => _helicopterAngle;
            set => SetProperty(ref _helicopterAngle, value);
        }

        private float _flighterAngle = 300;
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
            BuildFlightLocationString();
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
                    BuildFlightLocationString();
                    Thread.Sleep(_mapRefreshInterval);
                }
            });
            if(_translateData.PlaneInfo.IsConnect == false)
            {
                Task.Run(() =>
                { 
                    var random = new Random();
                    var point1 = new Point(random.Next(90), random.Next(70));
                    var point2 = new Point(random.Next(90), random.Next(70));
                    while (true)
                    {
                        var ram = new Random();
                        HelicopterAngle += 1;
                        if (HelicopterAngle >= 360)
                            HelicopterAngle = 0;
                        FlighterAngle += 2;
                        if (FlighterAngle >= 360)
                            FlighterAngle = 0;
                        FlighterPosition = point1;
                        HelicopterPosition = point2;
                        Thread.Sleep(_mapRefreshInterval);
                        
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

        public virtual void BuildFlightLocationString()
        {
            var planeInfo = Ioc.Get<ITranslateData>().PlaneInfo;
            FlightLocationString = $"{_helicopterName}{PlaneInfoToString(planeInfo.Helicopter)};" +
                    $"{_flighterName}{PlaneInfoToString(planeInfo.Flighter)}";
            if(_translateData.PlaneInfo.IsConnect == true)
            {
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

    }
}
