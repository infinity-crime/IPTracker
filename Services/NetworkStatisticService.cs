using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace IPTracker.Services
{
    public class NetworkStatisticService : INetworkStatisticService
    {
        private NetworkInterface _networkInterface;
        private IPInterfaceStatistics _lastStatistic;

        private readonly TimeSpan _interval;

        private CancellationTokenSource? _tokenSource = null;
        private Task? _loop = null;

        public event EventHandler<NetworkStatsDto> NetworkStatsUpdated;

#pragma warning disable CS8618 
        public NetworkStatisticService(TimeSpan? interval)
#pragma warning restore CS8618 
        {
            _interval = interval ?? TimeSpan.FromSeconds(1);
        }

        public void SetNetworkInterface(NetworkInterface networkInterface)
        {
            _networkInterface = networkInterface;
            if (_networkInterface is not null)
            {
                _lastStatistic = _networkInterface.GetIPStatistics();
            }
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            if(_loop != null && !_loop.IsCompleted)
                return Task.CompletedTask;

            _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _loop = Task.Run(async () =>
            {
                var cToken = _tokenSource.Token;
                while(!_tokenSource.IsCancellationRequested)
                {
                    await Task.Delay(_interval, cToken);

                    if (_networkInterface is null)
                        continue;

                    try
                    {
                        var currentStats = _networkInterface.GetIPStatistics();

                        long packetsReceived = currentStats.UnicastPacketsReceived;
                        long packetsSent = currentStats.UnicastPacketsSent;

                        var deltaRec = packetsReceived - _lastStatistic.UnicastPacketsReceived;
                        var deltaSent = packetsSent - _lastStatistic.UnicastPacketsSent;

                        if (deltaRec <= 0) deltaRec = 0;
                        if(deltaSent <= 0) deltaSent = 0;

                        var dto = new NetworkStatsDto
                        {
                            TotalPacketsReceived = packetsReceived,
                            TotalPacketsSent = currentStats.UnicastPacketsSent,
                            ReceivedPerSecond = deltaRec,
                            SentPerSecond = deltaSent
                        };

                        _lastStatistic = currentStats;

                        NetworkStatsUpdated?.Invoke(this, dto);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            });

            return Task.CompletedTask;
        }

        public async Task StopAsync()
        {
            if (_tokenSource == null)
                return;

            _tokenSource.Cancel(); // отменяем задачу (цикл)

            try
            {
                await _loop!; // надо дождаться окончания задачи
            }
            catch { } // любые исключения при ожидании подавляем

            _tokenSource.Dispose();
            _tokenSource = null;
            _loop = null;
        }

        public void Dispose()
        {
            StopAsync().GetAwaiter().GetResult();
        }
    }
}
