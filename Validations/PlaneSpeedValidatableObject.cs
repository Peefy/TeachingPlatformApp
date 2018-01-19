namespace TeachingPlatformApp.Validations
{
    public class PlaneSpeedValidatableObject : ValidatableObject<float>
    {
        public PlaneSpeedValidatableObject(float speed)
        {
            this.Value = speed;
        }
    }

}
