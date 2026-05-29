namespace WorldCuisineApp.Services;

public interface ISettingsService
{
    string ApiUrl { get; set; }
    bool IsLoggedIn { get; set; }
    string Username { get; set; }
    bool IsDarkMode { get; set; }
    double FontScale { get; set; }
    void ApplyThemeAndFont();
}
