using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using TeachingPlatformApp.Converters;
using TeachingPlatformApp.Models;
using TeachingPlatformApp.Utils;

namespace TeachingPlatformApp.Models
{
    public class AirlineFlight : FlightExperiment
    {

        public override string SetPointsConfigName => nameof(AirlineFlight);

        public AirlineFlight()
        {
            Name = "航线飞行";
            Index = 2;
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
