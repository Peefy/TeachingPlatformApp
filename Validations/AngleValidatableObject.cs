using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachingPlatformApp.Validations
{
    public class AngleValidatableObject : ValidatableObject<float>
    {
        public AngleValidatableObject(float angle)
        {
            this.Validations.Add(new AngleValidationRule(360.0f, -360.0f));
            this.Value = angle;
        }
    }

}
