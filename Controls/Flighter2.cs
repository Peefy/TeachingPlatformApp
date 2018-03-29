using System.Windows.Media;

namespace TeachingPlatformApp.Controls
{
    public class Flighter2 : Flighter
    {
        public Flighter2()
        {
            this.UpdateColor(Colors.DarkBlue);
        }

        public override WswPlatform.WswModelKind Kind => WswPlatform.WswModelKind.Flighter2;

    }

}
