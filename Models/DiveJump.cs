using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TeachingPlatformApp.Validations;

namespace TeachingPlatformApp.Models
{
    public class DiveJump : FlightExperiment
    {

        public DiveJump()
        {
            Name = "俯冲跃升";
            Index = 5;
            Pitch.Validations.Add(new AngleValidationRule(60, -60));
        }

    }
}
