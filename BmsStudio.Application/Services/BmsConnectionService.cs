using BmsStudio.Application.DTOs;
using BmsStudio.Application.Interfaces;
using BmsStudio.Core.Entities;
using BmsStudio.Core.Enums;
using BmsStudio.HardwareAbstractions.Interfaces;

namespace BmsStudio.Application.Services
{
    public class BmsConnectionService : IBmsConnectionService
    {
        private readonly IDeviceClient _deviceClient;

        public ConnectionState State { get; private set; } = ConnectionState.Disconnected;
        
        public BmsConnectionService(IDeviceClient deviceClient)
        {
            _deviceClient = deviceClient;
        }
        public async Task<ConnectionStatusDto> ConnectAsync(CanBaudRate baudRate)
        {
            bool isConnected = await _deviceClient.ConnectAsync();
            return new ConnectionStatusDto { IsConnected = isConnected , Message = isConnected ? "Successfull" : "Failed"  };
        }

        public Task DisconnectAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<BatteryStatusDto> GetBatteryStatusAsync()
        {
            var data = await _deviceClient.ReadAsync();

            return new BatteryStatusDto
            {
                Voltage = data.Voltage,
                Current = data.Current,
                Temperature = data.Temperature
            };
        }

        public Task<BmsDevice> ReadDeviceInfoAsync()
        {
            throw new NotImplementedException();
        }

        Task<BmsDevice> IBmsConnectionService.ReadDeviceInfoAsync()
        {
            throw new NotImplementedException();
        }
    }
}
