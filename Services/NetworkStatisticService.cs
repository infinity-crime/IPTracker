using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace IPTracker.Services
{
    public class NetworkStatisticService : INetworkStatisticService
    {
        public event EventHandler<NetworkStatsDto> NetworkStatsUpdated;

        public void SetNetworkInterface(NetworkInterface networkInterface)
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
