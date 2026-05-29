using FoodDrinkApp;
using Microsoft.Extensions.Logging;
using WorldCuisineApp.Services;
using WorldCuisineApp.ViewModels;
using WorldCuisineApp.Views;

namespace WorldCuisineApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<ISettingsService, SettingsService>();
        builder.Services.AddSingleton<IFavoritesService, FavoritesService>();
        builder.Services.AddSingleton<IHardwareService, HardwareService>();
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton<ICuisineService, CuisineService>();

        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<CuisineDetailViewModel>();
        builder.Services.AddTransient<FavoritesViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();
        builder.Services.AddTransient<CameraViewModel>();
        builder.Services.AddTransient<HelpViewModel>();

        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<CuisineDetailPage>();
        builder.Services.AddTransient<FavoritesPage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<CameraPage>();
        builder.Services.AddTransient<HelpPage>();
        builder.Services.AddSingleton<AppShell>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
