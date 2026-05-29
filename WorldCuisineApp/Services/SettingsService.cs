using WorldCuisineApp.Constants;

namespace WorldCuisineApp.Services;

/// <summary>
/// Persists accessibility and API settings using Preferences.
/// </summary>
public class SettingsService : ISettingsService
{
    public string ApiUrl
    {
        get => Preferences.Default.Get(AppConstants.PrefApiUrl, AppConstants.DefaultApiUrl);
        set => Preferences.Default.Set(AppConstants.PrefApiUrl, value ?? AppConstants.DefaultApiUrl);
    }

    public bool IsLoggedIn
    {
        get => Preferences.Default.Get(AppConstants.PrefLoggedIn, false);
        set => Preferences.Default.Set(AppConstants.PrefLoggedIn, value);
    }

    public string Username
    {
        get => Preferences.Default.Get(AppConstants.PrefUsername, string.Empty);
        set => Preferences.Default.Set(AppConstants.PrefUsername, value ?? string.Empty);
    }

    public bool IsDarkMode
    {
        get => Preferences.Default.Get(AppConstants.PrefDarkMode, false);
        set => Preferences.Default.Set(AppConstants.PrefDarkMode, value);
    }

    public double FontScale
    {
        get => Preferences.Default.Get(AppConstants.PrefFontScale, 1.0);
        set => Preferences.Default.Set(AppConstants.PrefFontScale, Math.Clamp(value, 0.85, 1.5));
    }

    public void ApplyThemeAndFont()
    {
        if (Application.Current is null)
            return;

        Application.Current.UserAppTheme = IsDarkMode ? AppTheme.Dark : AppTheme.Light;

        const double baseBody = 16;
        const double baseTitle = 22;
        Application.Current.Resources["BodyFontSize"] = baseBody * FontScale;
        Application.Current.Resources["TitleFontSize"] = baseTitle * FontScale;
    }
}
