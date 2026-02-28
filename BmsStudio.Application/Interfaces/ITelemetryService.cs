
using BmsStudio.Core.Models;
namespace BmsStudio.Application.Interfaces;
public interface ITelemetryService {
    event Action<PackInfo> PackUpdated;
    event Action<List<CellVoltage>> CellsUpdated;
    Task StartAsync();
    Task StopAsync();
}
