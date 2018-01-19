using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachingPlatformApp.Validations
{
    public class AngleValidationRule : IValidationRule<float>
    {
        public string ValidationMessage { get; set; } = "角度不在范围内";

        public bool Check(float value)
        {
            if (value <= Up && value >= Down)
                return true;
            return false;
        }

        public float Up { get; set; }
        public float Down { get; set; }

        public AngleValidationRule(float up, float down)
        {
            Up = up;
            Down = down;
        }

    }
}
