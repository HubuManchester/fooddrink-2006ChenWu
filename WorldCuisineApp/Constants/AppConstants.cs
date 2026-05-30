namespace WorldCuisineApp.Constants;

/// <summary>
/// Application-wide constants. Configure MockAPI in Preferences or replace the default URL.
/// </summary>
public static class AppConstants
{
    /// <summary>
    /// Default MockAPI endpoint. Create a resource named "cuisines" on mockapi.io and paste your URL here,
    /// or set it at runtime in Settings (stored in Preferences).
    /// </summary>
    public const string DefaultApiUrl = "https://YOUR_PROJECT_ID.mockapi.io/cuisines";

    public const string PrefApiUrl = "api_url";
    public const string PrefLoggedIn = "is_logged_in";
    public const string PrefUsername = "username";
    public const string PrefDarkMode = "dark_mode";
    public const string PrefFontScale = "font_scale";
    public const string PrefFavorites = "favorite_cuisine_ids";
    public const string PrefLocalUsers = "local_users";

    public const double ShakeThreshold = 1.85;
    public const int ShakeCooldownMs = 1500;
}
