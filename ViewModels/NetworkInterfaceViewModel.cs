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

namespace IPTracker.ViewModels
{
    public class NetworkInterfaceViewModel : INotifyPropertyChanged
    {
        private readonly INetworkInterfaceService _networkService;
        private NetworkInterfaceModel? _primaryInterface;

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

        public NetworkInterfaceViewModel(INetworkInterfaceService networkInterfaceService)
        {
            _networkService = networkInterfaceService;

            LoadPrimaryInterface();

            RefreshCommand = new RelayCommand(_ => RefreshNetworkInterface());

            BuildRows();
        }

        public ICommand RefreshCommand { get; }

        private void LoadPrimaryInterface()
        {
            PrimaryModel = _networkService.GetPrimaryNetworkInterface();
            if (PrimaryModel is null)
                MessageBox.Show("Произошла ошибка получения данных сетевого адаптера Wi-Fi", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
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
            #endregion
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class PropertyRow
    {
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
