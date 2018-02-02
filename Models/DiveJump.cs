using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TeachingPlatformApp.Utils;
using TeachingPlatformApp.Validations;

namespace TeachingPlatformApp.Models
{
    public class DiveJump : FlightExperiment
    {
        public DiveJump()
        {          
            Index = 5;
            Name = JsonFileConfig.Instance.StringResource.FlightExperimentNames[Index - 1];
            Pitch.Validations.Add(new AngleValidationRule(60, -60));
        }
    }
}
