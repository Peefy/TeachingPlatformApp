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
            Name = "盘旋";
            Index = 4;
            _hoverAngle = new AngleValidatableObject(45)
            {
                Value = Convert.ToSingle(LogAndConfig.Config.
                GetProperty(nameof(HoverAngle), 45.0f))
            };
        }

    }
}
