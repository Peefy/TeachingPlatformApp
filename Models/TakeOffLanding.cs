using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DuGu.NetFramework.Logs;
using DuGu.NetFramework.Services;

using TeachingPlatformApp.Communications;
using TeachingPlatformApp.Validations;
using TeachingPlatformApp.Utils;

namespace TeachingPlatformApp.Models
{
    public class TakeOffLanding : FlightExperiment
    {
        Point _locationT = Point.Parse(Convert.ToString(LogAndConfig.Config.
            GetProperty(nameof(PitchInRange), "10,10")));
        public Point LocationT
        {
            get => _locationT;
            set
            {
                SetProperty(ref _locationT, value);
                LogAndConfig.Config.SetProperty(nameof(LocationT), value.ToString());
            } 
        }

        string _sound = "提示音";
        public string Sound
        {
            get => _sound;
            set => SetProperty(ref _sound, value);
        }

        float _pitchInRange = Convert.ToSingle(LogAndConfig.Config.
            GetProperty(nameof(PitchInRange),30.0f));
        public float PitchInRange
        {
            get => _pitchInRange;
            set
            {
                SetProperty(ref _pitchInRange, value);
                LogAndConfig.Config.SetProperty(nameof(PitchInRange), value);
            }
        }

        public TakeOffLanding()
        {
            Index = 1;
            Name = JsonFileConfig.Instance.StringResource.FlightExperimentNames[Index - 1];
        }

        public override async Task StartAsync()
        {
            await base.StartAsync();
        }

    }
}
