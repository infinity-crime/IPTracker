using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace IPTracker.Services
{
    public class NetworkStatsDto
    {
        public long TotalPacketsReceived { get; set; }
        public long TotalPacketsSent { get; set; }
        public double ReceivedPerSecond { get; set; } = 0;
        public double SentPerSecond { get; set; } = 0;
    }
    public interface INetworkStatisticService : IDisposable
    {
        void SetNetworkInterface(NetworkInterface networkInterface);

        Task StartAsync(CancellationToken cancellationToken = default);
        Task StopAsync();

        event EventHandler<NetworkStatsDto> NetworkStatsUpdated;
    }
}
