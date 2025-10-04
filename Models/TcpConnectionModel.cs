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
        public IPEndPoint? LocalEndPoint { get; init; }
        public IPEndPoint? RemoteEndPoint { get; init; }
        public TcpState StateTcpConnection { get; init; }
    }
}
