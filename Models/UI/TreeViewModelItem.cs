using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TeachingPlatformApp.Models;

namespace TeachingPlatformApp.Models.UI
{
    public class TreeViewModelItem
    {
        public string Icon { get; set; }
        public string EditIcon { get; set; }
        public string DisplayName { get; set;  }
        public string Name { get; set; }

        public ObservableRangeCollection<FlightExperiment> Children { get; set; }
        public TreeViewModelItem()
        {
            Children = new ObservableRangeCollection<FlightExperiment>();
        }
    }
}
