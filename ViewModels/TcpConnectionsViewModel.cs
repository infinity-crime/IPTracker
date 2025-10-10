using IPTracker.Models;
using IPTracker.Services;
using MVVM_Example.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace IPTracker.ViewModels
{
    public class TcpConnectionsViewModel : BaseViewModel
    {
        private readonly ITcpConnectionsService _tcpConnectionsService;

        private int _countTcpConnections = 0;
        public int CountTcpConnections
        {
            get => _countTcpConnections;
            set
            {
                if (_countTcpConnections == value)
                    return;

                _countTcpConnections = value;
                OnPropertyChanged();
            }
        }

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

            CountTcpConnections = snapshot.Count;
        }

        private void UpdateConnections()
        {
            TcpConnectionsCollection.Clear();
            InitialConnections();
        }
    }
}
