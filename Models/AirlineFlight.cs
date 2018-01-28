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
            Index = 2;
            Name = JsonFileConfig.Instance.StringResource.FlightExperimentNames[Index - 1];
            HasSetPoints = true;
            _setPoints = new ObservableRangeCollection<Point>();
            pointsConverter = new SetPointsToStringConverter();
            var pointsStr = LogAndConfig.Config.GetProperty(SetPointsConfigName + nameof(SetPoints),
                "(10,10),(10,50),(50,50),(60,20),(40,5)").ToString();
            _setPoints = pointsConverter.ConvertBack(pointsStr, null, null, null)
                as ObservableRangeCollection<Point>;
        }
    }
}
