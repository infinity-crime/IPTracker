using IPTracker.Services;
using IPTracker.ViewModels;
using IPTracker.Views;
using MaterialDesignThemes.Wpf;
using System.Configuration;
using System.Data;
using System.Windows;

namespace IPTracker
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var networkService = new NetworkInterfaceService();
            var networkInterfaceVM = new NetworkInterfaceViewModel(networkService);
            var mainWindow = new MainView 
            { 
                DataContext = networkInterfaceVM 
            };

            mainWindow.Show();
        }
    }
}
