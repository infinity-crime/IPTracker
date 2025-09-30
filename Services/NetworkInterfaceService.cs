using IPTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace IPTracker.Services
{
    public class NetworkInterfaceService : INetworkInterfaceService
    {
        public IEnumerable<NetworkInterfaceModel>? GetAllNetworkInterfaces()
        {
            try
            {
                var adapters = NetworkInterface.GetAllNetworkInterfaces();

                return adapters.Select(x => MapToModel(x));
            }
            catch(NetworkInformationException)
            {
                return null;
            }
        }

        public NetworkInterfaceModel? GetPrimaryNetworkInterface()
        {
            var adapter = NetworkInterface.GetAllNetworkInterfaces()
                .FirstOrDefault(a => a.OperationalStatus == OperationalStatus.Up
                && a.NetworkInterfaceType == NetworkInterfaceType.Wireless80211);

            if(adapter != null)
                return MapToModel(adapter);

            return null;
        }

        private NetworkInterfaceModel MapToModel(NetworkInterface ni)
        {
            return new NetworkInterfaceModel
            {
                AdapterId = ni.Id,
                Description = ni.Description,
                Name = ni.Name,
                AdapterType = ni.NetworkInterfaceType,
                AdapterStatus = ni.OperationalStatus,
                MacAddress = ni.GetPhysicalAddress().ToString(),
                SpeedBitsPerSecond = ni.Speed / 1024 / 1024,
                SupportsMulticast = ni.SupportsMulticast
            };
        }
    }
}
