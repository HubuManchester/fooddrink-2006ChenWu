namespace WorldCuisineApp.Services;

public interface IFavoritesService
{
    IReadOnlyCollection<string> GetFavoriteIds();
    bool IsFavorite(string cuisineId);
    void ToggleFavorite(string cuisineId);
}
