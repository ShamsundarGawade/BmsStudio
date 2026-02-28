
using BmsStudio.Application.Interfaces;
using BmsStudio.Core.Entities;
namespace BmsStudio.Infrastructure.Services;
public class TelemetryService : ITelemetryService
{
    private CancellationTokenSource? _cts;
    public event Action<PackInfo>? PackUpdated;
    public event Action<List<CellVoltage>>? CellsUpdated;
    public Task StartAsync()
    {
        _cts = new();
        Task.Run(async () =>
        {
            var r = new Random();
            while (!_cts.IsCancellationRequested)
            {
                PackUpdated?.Invoke(new PackInfo { PackVoltage = 48 + r.NextDouble(), Current = r.NextDouble() * 10, Temperature = 25 + r.NextDouble() * 5 });
                var cells = Enumerable.Range(1, 8).Select(i => new CellVoltage { CellNumber = i, Voltage = 3.5 + r.NextDouble() * 0.1 }).ToList(); CellsUpdated?.Invoke(cells);
                await Task.Delay(500);
            }
        }); return Task.CompletedTask;
    }
    public Task StopAsync()
    {
        _cts?.Cancel();
        return Task.CompletedTask;
    }
}
