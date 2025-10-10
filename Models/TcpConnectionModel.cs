using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net;

namespace IPTracker.Models
{
    public class TcpConnectionModel
    {
        public IPAddress? LocalAddress { get; init; }
        public int LocalPort { get; init; }
        public IPAddress? RemoteAddress { get; init; }
        public int RemotePort { get; init; }
        public TcpState StateTcpConnection { get; init; }
    }
}
