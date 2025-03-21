using System.Windows;
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
        services.AddHttpClient();   // IHttpClientFactory 등록됨
        services.AddTransient<HttpClientService>(); // IHttpClientFactory를 주입받기 떄문에 Transient로 등록

        // Main Window
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowViewModel>();

        // ViewModels
        services.AddTransient<WeatherViewModel>();

        // Pages
        services.AddTransient<WeatherPage>();
    }
}

