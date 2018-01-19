using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Mvvm;

using DuGu.NetFramework.Logs;
using DuGu.NetFramework.Services;

using TeachingPlatformApp.Communications;
using TeachingPlatformApp.Validations;
using TeachingPlatformApp.Utils;
using TeachingPlatformApp.Converters;

namespace TeachingPlatformApp.Models
{
    public abstract class FlightExperiment : BindableBase, IFlightProcess
    {

        string _name = "";
        int _index = 0;
        bool _isStart;
        bool _isStop;
        bool _isValid;
        Point _nowLocation;
        Point _sixPlatformLocation;
        AngleValidatableObject _720pitch;
        AngleValidatableObject _720roll;
        AngleValidatableObject _720yaw;

        AngleValidatableObject _sixPlatformPitch;
        AngleValidatableObject _sixPlatformRoll;
        AngleValidatableObject _sixPlatformYaw;

        ValidatableObject<float> _speed;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

		public int Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }

        public AngleValidatableObject Pitch
        {
            get => _720pitch;
            set => SetProperty(ref _720pitch, value);
        }

        public AngleValidatableObject Roll
        {
            get => _720roll;
            set => SetProperty(ref _720roll, value);
        }

        public AngleValidatableObject Yaw
        {
            get => _720yaw;
            set => SetProperty(ref _720yaw, value);
        }

        public AngleValidatableObject SixPlatformPitch
        {
            get => _sixPlatformPitch;
            set => SetProperty(ref _720pitch, value);
        }

        public AngleValidatableObject SixPlatformRoll
        {
            get => _sixPlatformRoll;
            set => SetProperty(ref _720roll, value);
        }

        public AngleValidatableObject SixPlatformYaw
        {
            get => _sixPlatformYaw;
            set => SetProperty(ref _720yaw, value);
        }

        public bool IsStart
        {
            get => _isStart;
            set => SetProperty(ref _isStart,value);
        }

        public bool IsStop
        {
            get => _isStop;
            set => SetProperty(ref _isStop, value);
        }

        public bool IsValid
        {
            get
            {
                Roll.Validate();
                Yaw.Validate();
                Pitch.Validate();
                return Roll.IsValid && Yaw.IsValid && Pitch.IsValid;
            }
            set => SetProperty(ref _isValid, value);
        }

        public Point NowLocation
        {
            get => _nowLocation;
            set => SetProperty(ref _nowLocation, value);
        }

        public Point SixPlatformNowLocation
        {
            get => _sixPlatformLocation;
            set => SetProperty(ref _sixPlatformLocation, value);
        }

        public void SetPara(string name,int index)
        {
            Name = name;
            Index = index;
        }

        public virtual Task EndAsync()
        {
            IsStart = false;
            IsStop = true;
            return Task.FromResult(false);
        }

        public virtual async Task StartAsync()
        {
            if (IsStart == true)
                await Task.FromResult(false);
            IsStart = true;
            IsStop = false;
            await Task.Run(async () =>
            {
                await Task.Delay(100);
                while (IsStart == true)
                {
                    var isValid = this.IsValid;
                    LogManager.GetCurrentLogger().
                        Info($"{DateTime.Now}:{Name}实验检测结果:{isValid}\r\n");
                    await Task.Delay(300);
                    if (isValid == false)
                    {
                        break;
                    }
                }
            });
        }

        public virtual string SetPointsConfigName { get; } = nameof(FlightExperiment);

        protected SetPointsToStringConverter pointsConverter;

        protected ObservableRangeCollection<Point> _setPoints;
        public ObservableRangeCollection<Point> SetPoints
        {
            get => _setPoints;
            set
            {
                SetProperty(ref _setPoints, value);
                LogAndConfig.Config.SetProperty(SetPointsConfigName + nameof(SetPoints),
                    pointsConverter.Convert(value, null, null, null));
            }
        }

        public bool HasSetPoints { get; set; } = false;

        public FlightExperiment()
        {
            _nowLocation = new Point(0.01, 0.01);
            _sixPlatformLocation = new Point(0.01, 0.01);
            _720roll = new AngleValidatableObject(0.01f);
            _720yaw = new AngleValidatableObject(0.01f);
            _720pitch = new AngleValidatableObject(0.01f);
            _sixPlatformRoll = new AngleValidatableObject(0.01f);
            _sixPlatformYaw = new AngleValidatableObject(0.01f);
            _sixPlatformPitch = new AngleValidatableObject(0.01f);
            _speed = new ValidatableObject<float>();
        }

    }
}
