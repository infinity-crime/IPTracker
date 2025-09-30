using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace IPTracker.Models
{
    public class NetworkInterfaceModel
    {
        public string AdapterId { get; init; }
        public string Description { get; init; }
        public string Name { get; init; }
        public NetworkInterfaceType AdapterType { get; init; }
        public OperationalStatus AdapterStatus { get; set; }
        public string MacAddress { get; init; }
        public long SpeedBitsPerSecond { get; init; } // Nominal speed

        public bool SupportsMulticast { get; init; }
    }
}
