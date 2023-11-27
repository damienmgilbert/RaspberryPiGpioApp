using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Logging;
using RaspberryPiGpioApp.Services;
using RaspberryPiGpioApp.ViewModels;
using RaspberryPiGpioApp.Views;

namespace RaspberryPiGpioApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
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
#if DEBUG
		builder.Logging.AddDebug();
#endif
        Ioc.Default.ConfigureServices(builder.Services.BuildServiceProvider());
        return builder.Build();
    }

    private static MauiAppBuilder RegisterRoutes(this MauiAppBuilder builder)
    {
        builder.Services.AddTransientWithShellRoute<HomePage, HomeViewModel>("Home");
        return builder;
    }

    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<RpiService>();
        return builder;
    }
}
