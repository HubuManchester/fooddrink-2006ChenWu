using WorldCuisineApp.Services;
using WorldCuisineApp.Views;

namespace WorldCuisineApp;

public partial class AppShell : Shell
{
    private readonly ISettingsService _settings;

    public AppShell(ISettingsService settings)
    {
        _settings = settings;
        InitializeComponent();

        Routing.RegisterRoute(nameof(CuisineDetailPage), typeof(CuisineDetailPage));
        Routing.RegisterRoute(nameof(HelpPage), typeof(HelpPage));
        Routing.RegisterRoute(nameof(CameraPage), typeof(CameraPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));

        Loaded += OnShellLoaded;
    }

    private async void OnShellLoaded(object? sender, EventArgs e)
    {
        Loaded -= OnShellLoaded;
        _settings.ApplyThemeAndFont();

        if (_settings.IsLoggedIn)
            await GoToAsync("//home");
        else
            await GoToAsync("//login");
    }
}
