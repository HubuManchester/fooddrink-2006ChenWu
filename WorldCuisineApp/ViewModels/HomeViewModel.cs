using System.Collections.ObjectModel;
using System.Windows.Input;
using WorldCuisineApp.Models;
using WorldCuisineApp.Services;

namespace WorldCuisineApp.ViewModels;

public class HomeViewModel : BaseViewModel
{
    private readonly ICuisineService _cuisineService;
    private readonly IHardwareService _hardware;
    private string _searchText = string.Empty;

    public HomeViewModel(ICuisineService cuisineService, IHardwareService hardware)
    {
        _cuisineService = cuisineService;
        _hardware = hardware;
        Title = "World Cuisines";
        Cuisines = [];

        RefreshCommand = new Command(async () => await LoadAsync());
        OpenHelpCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(Views.HelpPage)));
        OpenCameraCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(Views.CameraPage)));
        RandomDishCommand = new Command(async () => await OnShakeAsync());
        CuisineTappedCommand = new Command<CuisineItem>(async item =>
        {
            if (item is null) return;
            await Shell.Current.GoToAsync($"{nameof(Views.CuisineDetailPage)}?id={item.Id}");
        });
    }

    public ObservableCollection<CuisineItem> Cuisines { get; }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
                _ = FilterAsync();
        }
    }

    public ICommand RefreshCommand { get; }
    public ICommand OpenHelpCommand { get; }
    public ICommand OpenCameraCommand { get; }
    public ICommand RandomDishCommand { get; }
    public ICommand CuisineTappedCommand { get; }

    public async Task LoadAsync()
    {
        try
        {
            IsBusy = true;
            ClearMessages();

            var items = await _cuisineService.GetCuisinesAsync();
            Cuisines.Clear();
            foreach (var item in FilterItems(items, SearchText))
                Cuisines.Add(item);

            StatusMessage = $"{Cuisines.Count} dishes loaded ({_cuisineService.LastDataSource}). Shake device for a random pick!";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not load cuisines: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task OnShakeAsync()
    {
        try
        {
            var items = await _cuisineService.GetCuisinesAsync();
            if (items.Count == 0) return;

            var pick = items[Random.Shared.Next(items.Count)];
            _hardware.VibrateSuccess();
            StatusMessage = $"Shake pick: {pick.Name} ({pick.Country})";
            await Shell.Current.GoToAsync($"{nameof(Views.CuisineDetailPage)}?id={pick.Id}");
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            _hardware.VibrateError();
        }
    }

    private Task FilterAsync()
    {
        return Task.Run(async () =>
        {
            var items = await _cuisineService.GetCuisinesAsync();
            var filtered = FilterItems(items, SearchText).ToList();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Cuisines.Clear();
                foreach (var item in filtered)
                    Cuisines.Add(item);
            });
        });
    }

    private static IEnumerable<CuisineItem> FilterItems(IEnumerable<CuisineItem> items, string search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return items;

        search = search.Trim();
        return items.Where(c =>
            c.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
            || c.Country.Contains(search, StringComparison.OrdinalIgnoreCase)
            || c.Region.Contains(search, StringComparison.OrdinalIgnoreCase));
    }
}
