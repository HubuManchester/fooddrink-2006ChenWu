using WorldCuisineApp.Constants;

namespace WorldCuisineApp.Services;

public class FavoritesService : IFavoritesService
{
    private HashSet<string> _ids = [];

    public FavoritesService()
    {
        Load();
    }

    public IReadOnlyCollection<string> GetFavoriteIds() => _ids.ToList();

    public bool IsFavorite(string cuisineId) => _ids.Contains(cuisineId);

    public void ToggleFavorite(string cuisineId)
    {
        if (string.IsNullOrWhiteSpace(cuisineId))
            return;

        if (!_ids.Add(cuisineId))
            _ids.Remove(cuisineId);

        Save();
    }

    private void Load()
    {
        var raw = Preferences.Default.Get(AppConstants.PrefFavorites, string.Empty);
        _ids = string.IsNullOrWhiteSpace(raw)
            ? []
            : raw.Split(',', StringSplitOptions.RemoveEmptyEntries).ToHashSet();
    }

    private void Save() =>
        Preferences.Default.Set(AppConstants.PrefFavorites, string.Join(',', _ids));
}
