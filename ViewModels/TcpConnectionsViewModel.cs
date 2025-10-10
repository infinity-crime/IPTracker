using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using IPTracker.Models;
using IPTracker.Services;
using MVVM_Example.ViewModel.Commands;
using System.Windows.Input;
using System.Windows.Navigation;

namespace IPTracker.ViewModels
{
    public class TcpConnectionsViewModel
    {
        private readonly ITcpConnectionsService _tcpConnectionsService;

        public TcpConnectionsViewModel(ITcpConnectionsService tcpConnectionsService)
        {
            _tcpConnectionsService = tcpConnectionsService;

            UpdateCommand = new RelayCommand(_ => UpdateConnections());

            InitialConnections();
        }

        public ObservableCollection<TcpConnectionModel> TcpConnectionsCollection { get; } = new();

        public ICommand UpdateCommand { get; }

        private void InitialConnections()
        {
            var snapshot = _tcpConnectionsService.GetSnapshotTcpConnections();
            foreach (var connection in snapshot)
                TcpConnectionsCollection.Add(connection);
        }

        private void UpdateConnections()
        {
            TcpConnectionsCollection.Clear();
            InitialConnections();
        }
    }
}
