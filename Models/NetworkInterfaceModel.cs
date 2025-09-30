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

        public long TotalPacketsReceived { get; set; }
        public long TotalPacketsSent { get; set; }
        public long ReceivedPerSecond { get; set; }
        public long SentPerSecond { get; set; }
    }
}
