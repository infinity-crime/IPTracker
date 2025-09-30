using IPTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace IPTracker.Services
{
    public interface INetworkInterfaceService
    {
        NetworkInterfaceModel? GetPrimaryNetworkInterfaceDto();
        NetworkInterface? GetPrimaryNetworkInterface();
        IEnumerable<NetworkInterfaceModel>? GetAllNetworkInterfaces();
    }
}
