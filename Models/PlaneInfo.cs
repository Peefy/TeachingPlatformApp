using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TeachingPlatformApp.WswPlatform;

namespace TeachingPlatformApp.Models
{
    public class PlaneInfo
    {
        public AngleWithLocation Flighter { get; set; }

        public AngleWithLocation Helicopter { get; set; }

        public PlaneInfo()
        {
            Flighter = new AngleWithLocation();
            Helicopter = new AngleWithLocation();
        }

    }
}
