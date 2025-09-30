using IPTracker.Services;
using IPTracker.ViewModels;
using IPTracker.Views;
using MaterialDesignThemes.Wpf;
using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace IPTracker
{
    public partial class App : Application
    {
        private ServiceProvider? _provider;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            services.AddSingleton<INetworkInterfaceService, NetworkInterfaceService>();
            services.AddSingleton<INetworkStatisticService, NetworkStatisticService>(sp => new NetworkStatisticService(TimeSpan.FromSeconds(1)));

            services.AddSingleton<NetworkInterfaceViewModel>(sp =>
            {
                var s1 = sp.GetRequiredService<INetworkInterfaceService>();
                var s2 = sp.GetRequiredService<INetworkStatisticService>();
                return new NetworkInterfaceViewModel(s1, s2);
            });

            services.AddSingleton<MainView>(sp =>
            {
                var vm = sp.GetRequiredService<NetworkInterfaceViewModel>();
                return new MainView { DataContext = vm };
            });

            _provider = services.BuildServiceProvider();

            var interfaceService = _provider.GetRequiredService<INetworkInterfaceService>();
            var statsService = _provider.GetRequiredService<INetworkStatisticService>();

            await statsService.StartAsync();

            var mainWindow = _provider.GetRequiredService<MainView>();
            mainWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            var statsService = _provider!.GetRequiredService<INetworkStatisticService>();
            await statsService.StopAsync();
        }
    }
}
