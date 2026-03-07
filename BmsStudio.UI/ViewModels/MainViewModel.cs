using BmsStudio.Application.Interfaces;
using BmsStudio.Core.Enums;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BmsStudio.UI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IBmsConnectionService _connectionService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand ConnectCommand { get; }
        public string ConnectionMessage { get; set; } = string.Empty; // Fix 1: Initialize property

        public MainViewModel(IBmsConnectionService connectionService)
        {
            _connectionService = connectionService;
            ConnectCommand = new RelayCommand(async () => await ConnectAsync());
        }

        private async Task ConnectAsync()
        {
            var status = await _connectionService.ConnectAsync(CanBaudRate.Baud500K);
            ConnectionMessage = status.IsConnected ? "Connected!" : "Connection failed.";
            OnPropertyChanged(nameof(ConnectionMessage));
        }

        // Fix 2: Implement OnPropertyChanged method
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        // INotifyPropertyChanged implementation...
    }
}
