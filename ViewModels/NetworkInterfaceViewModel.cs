using IPTracker.Models;
using IPTracker.Services;
using MVVM_Example.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace IPTracker.ViewModels
{
    public class NetworkInterfaceViewModel : INotifyPropertyChanged
    {
        private readonly INetworkInterfaceService _networkService;
        private readonly INetworkStatisticService _networkStatisticService;

        private NetworkInterfaceModel? _primaryInterface;

        private readonly Dispatcher _dispatcher = Application.Current.Dispatcher;

        public NetworkInterfaceModel? PrimaryModel
        {
            get => _primaryInterface;
            set
            {
                if (_primaryInterface != value)
                    _primaryInterface = value;

                OnPropertyChanged();
            }
        }

        public ObservableCollection<PropertyRow> PropertyRows { get; } = new();

        public ICommand RefreshCommand { get; }

        public NetworkInterfaceViewModel(INetworkInterfaceService networkInterfaceService, 
            INetworkStatisticService networkStatisticService)
        {
            _networkService = networkInterfaceService;
            _networkStatisticService = networkStatisticService;

            _networkStatisticService.NetworkStatsUpdated += OnStatsUpdated;

            LoadPrimaryInterface();

            RefreshCommand = new RelayCommand(_ => RefreshNetworkInterface());

            BuildRows();
        }

        private void LoadPrimaryInterface()
        {
            PrimaryModel = _networkService.GetPrimaryNetworkInterfaceDto();
            if (PrimaryModel is null)
                MessageBox.Show("Произошла ошибка получения данных сетевого адаптера Wi-Fi", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);

            _networkStatisticService.SetNetworkInterface(_networkService.GetPrimaryNetworkInterface());

            BuildRows();
        }

        private void RefreshNetworkInterface()
        {
            LoadPrimaryInterface();
        }

        private void BuildRows()
        {
            PropertyRows.Clear();

            if (PrimaryModel == null) return;

            #region Set Propery Rows
            PropertyRows.Add(new PropertyRow { Label = "Имя:", Value = PrimaryModel.Name });
            PropertyRows.Add(new PropertyRow { Label = "Описание:", Value = PrimaryModel.Description });
            PropertyRows.Add(new PropertyRow { Label = "Тип:", Value = PrimaryModel.AdapterType.ToString() });
            PropertyRows.Add(new PropertyRow { Label = "Статус:", Value = PrimaryModel.AdapterStatus.ToString() });
            PropertyRows.Add(new PropertyRow { Label = "MAC-адрес:", Value = PrimaryModel.MacAddress });
            PropertyRows.Add(new PropertyRow { Label = "Номинальная скорость:", Value = $"{PrimaryModel.SpeedBitsPerSecond} мбит/с" });
            PropertyRows.Add(new PropertyRow { Label = "Multicast:", Value = PrimaryModel.SupportsMulticast.ToString() });
            PropertyRows.Add(new PropertyRow { Label = "ID карты в системе:", Value = PrimaryModel.AdapterId });
            PropertyRows.Add(new PropertyRow { Label = "Общее количество полученных одноадресных пакетов:", Value = PrimaryModel.TotalPacketsReceived.ToString() });
            PropertyRows.Add(new PropertyRow { Label = "Общее количество отправленных одноадресных пакетов:", Value = PrimaryModel.TotalPacketsSent.ToString() });
            PropertyRows.Add(new PropertyRow { Label = "Получено в данный момент - ", Value = PrimaryModel.ReceivedPerSecond.ToString() });
            PropertyRows.Add(new PropertyRow { Label = "Отправлено в данный момент - ", Value = PrimaryModel.SentPerSecond.ToString() });
            #endregion
        }

        private void OnStatsUpdated(object sender, NetworkStatsDto dto)
        {
            _dispatcher.BeginInvoke(new Action(() => 
            {
                PrimaryModel.TotalPacketsReceived = dto.TotalPacketsReceived;
                PrimaryModel.TotalPacketsSent = dto.TotalPacketsSent;
                PrimaryModel.ReceivedPerSecond = dto.ReceivedPerSecond;
                PrimaryModel.SentPerSecond = dto.SentPerSecond;

                BuildRows();

                OnPropertyChanged(nameof(PrimaryModel));
            }));
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            _networkStatisticService.NetworkStatsUpdated -= OnStatsUpdated;
        }
    }

    public class PropertyRow
    {
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
