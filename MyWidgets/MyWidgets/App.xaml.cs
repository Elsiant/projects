using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using MyWidgets.Common;
using MyWidgets.ViewModels;
using MyWidgets.Pages;

namespace MyWidgets;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public IServiceProvider ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Services
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddHttpClient<HttpClientService>();

        // Main Window
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowViewModel>();

        // ViewModels
        services.AddTransient<WeatherViewModel>();

        // Pages
        services.AddTransient<WeatherPage>();
    }
}

