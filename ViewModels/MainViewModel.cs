using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPTracker.ViewModels
{
    public class MainViewModel
    {
        public NetworkInterfaceViewModel NetworkInterfaceViewModel { get; }
        public TcpConnectionsViewModel TcpConnectionsViewModel { get; }

        public MainViewModel(NetworkInterfaceViewModel networkInterfaceViewModel, TcpConnectionsViewModel tcpConnectionsViewModel)
        {
            NetworkInterfaceViewModel = networkInterfaceViewModel;
            TcpConnectionsViewModel = tcpConnectionsViewModel;
        }
    }
}
