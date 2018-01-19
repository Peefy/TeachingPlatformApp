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
            this.Validations.Add(new AngleValidationRule(180.0f, -180.0f));
            this.Value = angle;
        }
    }

}
