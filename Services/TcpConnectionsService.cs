using IPTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace IPTracker.Services
{
    public class TcpConnectionsService : ITcpConnectionsService
    {
        private readonly List<TcpConnectionModel> tcpConnectionModels = new List<TcpConnectionModel> { Capacity = 50 };

        public List<TcpConnectionModel> GetSnapshotTcpConnections()
        {
            tcpConnectionModels.Clear();
            GetTcpConnections();

            return tcpConnectionModels;
        }

        private void GetTcpConnections()
        {
            var ipProp = IPGlobalProperties.GetIPGlobalProperties();
            var tcpConnections  = ipProp.GetActiveTcpConnections();

            foreach (var tcpConnection in tcpConnections)
            {
                tcpConnectionModels.Add(new TcpConnectionModel
                {
                    LocalEndPoint = tcpConnection.LocalEndPoint,
                    RemoteEndPoint = tcpConnection.RemoteEndPoint,
                    StateTcpConnection = tcpConnection.State
                });
            }
        }
    }
}
