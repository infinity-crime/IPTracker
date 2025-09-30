using IPTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPTracker.Services
{
    public interface INetworkInterfaceService
    {
        NetworkInterfaceModel? GetPrimaryNetworkInterface();
        IEnumerable<NetworkInterfaceModel>? GetAllNetworkInterfaces();
    }
}
