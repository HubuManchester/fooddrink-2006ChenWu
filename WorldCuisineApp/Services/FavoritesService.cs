using WorldCuisineApp.Constants;

namespace WorldCuisineApp.Services;

public class FavoritesService : IFavoritesService
{
    private readonly ISettingsService _settings;
    private HashSet<string> _ids = [];
    private string _loadedFor = string.Empty;

    public FavoritesService(ISettingsService settings)
    {
        _settings = settings;
    }

    public IReadOnlyCollection<string> GetFavoriteIds() { EnsureLoaded(); return _ids.ToList(); }

    public bool IsFavorite(string cuisineId) { EnsureLoaded(); return _ids.Contains(cuisineId); }

    public void ToggleFavorite(string cuisineId)
    {
        if (string.IsNullOrWhiteSpace(cuisineId))
            return;

        EnsureLoaded();

        if (!_ids.Add(cuisineId))
            _ids.Remove(cuisineId);

        Save();
    }

    private void EnsureLoaded()
    {
        var currentUser = _settings.Username;
        if (_loadedFor == currentUser)
            return;

        _loadedFor = currentUser;
        var key = string.IsNullOrWhiteSpace(currentUser)
            ? AppConstants.PrefFavorites
            : $"{AppConstants.PrefFavorites}_{currentUser}";

        var raw = Preferences.Default.Get(key, string.Empty);
        _ids = string.IsNullOrWhiteSpace(raw)
            ? []
            : raw.Split(',', StringSplitOptions.RemoveEmptyEntries).ToHashSet();
    }

    private void Save()
    {
        var key = string.IsNullOrWhiteSpace(_loadedFor)
            ? AppConstants.PrefFavorites
            : $"{AppConstants.PrefFavorites}_{_loadedFor}";

        Preferences.Default.Set(key, string.Join(',', _ids));
    }
}
