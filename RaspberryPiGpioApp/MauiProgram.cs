using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Logging;
using RaspberryPiGpioApp.Services;
using RaspberryPiGpioApp.ViewModels;
using RaspberryPiGpioApp.Views;
using Serilog;

namespace RaspberryPiGpioApp;
public static class MauiProgram
{

    #region Private methods
    private static MauiAppBuilder RegisterRoutes(this MauiAppBuilder builder)
    {
        builder.Services.AddTransientWithShellRoute<HomePage, HomeViewModel>("Home");
        builder.Services.AddTransientWithShellRoute<BoardPage, BoardViewModel>("Board");
        return builder;
    }
    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<RpiService>();
        return builder;
    }
    #endregion

    #region Public methods
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Debug().CreateLogger();

        builder
            .UseMauiApp<App>()
            // Initialize the .NET MAUI Community Toolkit by adding the below line of code
            .UseMauiCommunityToolkit()
            .RegisterRoutes()
            .RegisterServices()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Logging.AddSerilog(dispose: true);
        Ioc.Default.ConfigureServices(builder.Services.BuildServiceProvider());
        return builder.Build();
    }
    #endregion

}
