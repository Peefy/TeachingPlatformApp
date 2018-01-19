using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using TeachingPlatformApp.Utils;
using TeachingPlatformApp.Converters;

namespace TeachingPlatformApp.Models
{
    public class OnlyInstrumencs : FlightExperiment
    {

        float _setPitch = Convert.ToSingle(LogAndConfig.Config.
            GetProperty(nameof(SetPitch), 30.0f));
        public float SetPitch
        {
            get => _setPitch;
            set
            {
                SetProperty(ref _setPitch, value);
                LogAndConfig.Config.SetProperty(nameof(SetPitch), value);
            }
        }

        public override string SetPointsConfigName => nameof(OnlyInstrumencs);

        public OnlyInstrumencs()
        {
            Name = "仪表飞行";
            Index = 6;
            HasSetPoints = true;
            _setPoints = new ObservableRangeCollection<Point>();
            pointsConverter = new SetPointsToStringConverter();
            var pointsStr = LogAndConfig.Config.GetProperty(SetPointsConfigName + nameof(SetPoints),
                "(11,11),(22,22),(33,33),(44,44),(55,55)").ToString();
            _setPoints = pointsConverter.ConvertBack(pointsStr, null, null, null)
                as ObservableRangeCollection<Point>;
            
        }

    }
}
