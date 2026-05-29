using System.Windows.Input;
using WorldCuisineApp.Models;
using WorldCuisineApp.Services;

namespace WorldCuisineApp.ViewModels;

public class CuisineDetailViewModel : BaseViewModel
{
    private readonly ICuisineService _cuisineService;
    private readonly IFavoritesService _favorites;
    private readonly IHardwareService _hardware;
    private CuisineItem? _cuisine;
    private bool _isFavorite;

    public CuisineDetailViewModel(
        ICuisineService cuisineService,
        IFavoritesService favorites,
        IHardwareService hardware)
    {
        _cuisineService = cuisineService;
        _favorites = favorites;
        _hardware = hardware;
        Title = "Dish Details";

        ToggleFavoriteCommand = new Command(ToggleFavorite);
        SpeakCommand = new Command(async () => await SpeakAsync());
        GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
    }

    public CuisineItem? Cuisine
    {
        get => _cuisine;
        set
        {
            if (SetProperty(ref _cuisine, value))
            {
                OnPropertyChanged(nameof(HasCuisine));
                OnPropertyChanged(nameof(SpiceStars));
            }
        }
    }

    public bool HasCuisine => Cuisine is not null;

    public string SpiceStars => Cuisine is null
        ? string.Empty
        : $"Spice: {Math.Clamp(Cuisine.SpiceLevel, 0, 5)}/5";

    public bool IsFavorite
    {
        get => _isFavorite;
        set => SetProperty(ref _isFavorite, value);
    }

    public ICommand ToggleFavoriteCommand { get; }
    public ICommand SpeakCommand { get; }
    public ICommand GoBackCommand { get; }

    public async Task LoadAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            ErrorMessage = "No dish id was provided.";
            return;
        }

        try
        {
            IsBusy = true;
            ClearMessages();
            Cuisine = await _cuisineService.GetCuisineByIdAsync(id);
            if (Cuisine is null)
            {
                ErrorMessage = "Dish not found.";
                return;
            }

            Title = Cuisine.Name;
            IsFavorite = _favorites.IsFavorite(Cuisine.Id);
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

    private void ToggleFavorite()
    {
        if (Cuisine is null) return;

        try
        {
            _favorites.ToggleFavorite(Cuisine.Id);
            IsFavorite = _favorites.IsFavorite(Cuisine.Id);
            _hardware.VibrateSuccess();
            StatusMessage = IsFavorite ? "Added to favorites." : "Removed from favorites.";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            _hardware.VibrateError();
        }
    }

    private async Task SpeakAsync()
    {
        if (Cuisine is null) return;

        try
        {
            ClearMessages();
            var text = $"{Cuisine.Name} from {Cuisine.Country}. {Cuisine.Description}";
            await _hardware.SpeakAsync(text);
            StatusMessage = "Read aloud with text-to-speech.";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Speech failed: {ex.Message}";
            _hardware.VibrateError();
        }
    }
}
