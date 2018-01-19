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

        string helicopterName = "直升机";
        string flighterName = "战斗机";

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
            var config = JsonFileConfig.ReadFromFile().StringResource;
            Title = config.FlightMapTitle;
            helicopterName = config.HelicopterName;
            flighterName = config.FlighterName;
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
                catch (Exception)
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
                    Thread.Sleep(20);
                }
            });
            Task.Run(() =>
            {
                var i = 0.01f;
                var random = new Random();
                var point1 = new Point(random.Next(90), random.Next(70));
                var point2 = new Point(random.Next(90), random.Next(70));
                while (true)
                {
                    var ram = new Random();
                    HelicopterAngle += 1;
                    if (HelicopterAngle >= 360)
                        HelicopterAngle = 0;
                    FlighterPosition = point1;
                    HelicopterPosition = point2;
                    Thread.Sleep(20);
                    i += 0.01f;
                }
            });
        }

        public void BuildFlightLocationString()
        {
            var planeInfo = Ioc.Get<ITranslateData>().PlaneInfo;
            FlightLocationString = $"{helicopterName}{PlaneInfoToString(planeInfo.Helicopter)};" +
                    $"{flighterName}{PlaneInfoToString(planeInfo.Flighter)}";
            //HelicopterAngle = StructHelper.Rad2Deg(planeInfo.Helicopter.Yaw);
            //FlighterAngle = StructHelper.Rad2Deg(planeInfo.Flighter.Yaw);
            //FlighterPosition = new Point(planeInfo.Flighter.X, planeInfo.Flighter.Y);
            //HelicopterPosition = new Point(planeInfo.Helicopter.X, planeInfo.Helicopter.Y);
        }

        private string PlaneInfoToString(AngleWithLocation angleWithLocation)
        {
            return $"坐标:({angleWithLocation.X},{angleWithLocation.Y},{angleWithLocation.Z})";
        }

    }
}
