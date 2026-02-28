
using BmsStudio.Core.Models;
using BmsStudio.Core.Enums;
namespace BmsStudio.Application.Interfaces;
public interface IBmsConnectionService {
    Task<bool> ConnectAsync(CanBaudRate baudRate);
    Task DisconnectAsync();
    ConnectionState State {get;}
    Task<BmsDevice> ReadDeviceInfoAsync();
}
