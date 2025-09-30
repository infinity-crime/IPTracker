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
        private DateTime _lastTime;

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
            if(_networkInterface is not null)
            {
                _lastStatistic = _networkInterface.GetIPStatistics();
                _lastTime = DateTime.UtcNow;
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
                        var now = DateTime.UtcNow;
                        var seconds = (now - _lastTime).TotalSeconds;

                        if (seconds <= 0) seconds = 1;

                        var deltaRec = currentStats.UnicastPacketsReceived - _lastStatistic.UnicastPacketsReceived;
                        var deltaSent = currentStats.UnicastPacketsSent - _lastStatistic.UnicastPacketsSent;

                        if (deltaRec <= 0) deltaRec = currentStats.UnicastPacketsReceived;
                        if(deltaSent <= 0) deltaSent = currentStats.UnicastPacketsSent;

                        var dto = new NetworkStatsDto
                        {
                            TotalPacketsReceived = currentStats.UnicastPacketsReceived,
                            TotalPacketsSent = currentStats.UnicastPacketsSent,
                            ReceivedPerSecond = deltaRec / seconds,
                            SentPerSecond = deltaSent / seconds
                        };

                        _lastStatistic = currentStats;
                        _lastTime = now;

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
