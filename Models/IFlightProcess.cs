using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachingPlatformApp.Models
{
    public interface IFlightProcess
    {
        Task StartAsync();
        Task EndAsync();
    }
}
