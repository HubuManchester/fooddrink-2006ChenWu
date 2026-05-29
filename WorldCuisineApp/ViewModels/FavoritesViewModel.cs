using System.Collections.ObjectModel;
using System.Windows.Input;
using WorldCuisineApp.Models;
using WorldCuisineApp.Services;

namespace WorldCuisineApp.ViewModels;

public class FavoritesViewModel : BaseViewModel
{
    private readonly ICuisineService _cuisineService;
    private readonly IFavoritesService _favorites;

    public FavoritesViewModel(ICuisineService cuisineService, IFavoritesService favorites)
    {
        _cuisineService = cuisineService;
        _favorites = favorites;
        Title = "Favorites";
        Items = [];
        RefreshCommand = new Command(async () => await LoadAsync());
        OpenItemCommand = new Command<CuisineItem>(async item =>
        {
            if (item is null) return;
            await Shell.Current.GoToAsync($"{nameof(Views.CuisineDetailPage)}?id={item.Id}");
        });
    }

    public ObservableCollection<CuisineItem> Items { get; }
    public ICommand RefreshCommand { get; }
    public ICommand OpenItemCommand { get; }

    public async Task LoadAsync()
    {
        try
        {
            IsBusy = true;
            ClearMessages();
            var all = await _cuisineService.GetCuisinesAsync();
            var ids = _favorites.GetFavoriteIds();

            Items.Clear();
            foreach (var item in all.Where(c => ids.Contains(c.Id)))
                Items.Add(item);

            StatusMessage = Items.Count == 0
                ? "No favorites yet. Tap ♥ on a dish to save it."
                : $"{Items.Count} favorite dish(es).";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
