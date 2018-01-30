using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachingPlatformApp.Utils;
using TeachingPlatformApp.Validations;

namespace TeachingPlatformApp.Models
{
    public class HoverFlignt : FlightExperiment
    {

        AngleValidatableObject _hoverAngle;
        public AngleValidatableObject HoverAngle
        {
            get => _hoverAngle;
            set
            {
                SetProperty(ref _hoverAngle, value);
                LogAndConfig.Config.SetProperty(nameof(HoverAngle), value.Value);
            } 
        }

        public HoverFlignt()
        {
            Index = 4;
            Name = JsonFileConfig.Instance.StringResource.FlightExperimentNames[Index - 1];
            HoverAngle = new AngleValidatableObject(30);
        }

    }
}
