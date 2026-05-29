using System.Windows.Input;
using WorldCuisineApp.Constants;
using WorldCuisineApp.Services;

namespace WorldCuisineApp.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private readonly ISettingsService _settings;
    private readonly ICuisineService _cuisineService;
    private string _apiUrl = string.Empty;
    private bool _isDarkMode;
    private double _fontScale = 1.0;

    public SettingsViewModel(ISettingsService settings, ICuisineService cuisineService)
    {
        _settings = settings;
        _cuisineService = cuisineService;
        Title = "Settings";

        SaveCommand = new Command(Save);
        LogoutCommand = new Command(async () => await LogoutAsync());
    }

    public string ApiUrl
    {
        get => _apiUrl;
        set => SetProperty(ref _apiUrl, value);
    }

    public bool IsDarkMode
    {
        get => _isDarkMode;
        set => SetProperty(ref _isDarkMode, value);
    }

    public double FontScale
    {
        get => _fontScale;
        set
        {
            if (SetProperty(ref _fontScale, value))
                OnPropertyChanged(nameof(FontScaleLabel));
        }
    }

    public string FontScaleLabel => $"Text size: {FontScale:P0}";

    public ICommand SaveCommand { get; }
    public ICommand LogoutCommand { get; }

    public void Load()
    {
        ApiUrl = _settings.ApiUrl;
        IsDarkMode = _settings.IsDarkMode;
        FontScale = _settings.FontScale;
        OnPropertyChanged(nameof(FontScaleLabel));
    }

    private void Save()
    {
        try
        {
            ClearMessages();
            _settings.ApiUrl = string.IsNullOrWhiteSpace(ApiUrl) ? AppConstants.DefaultApiUrl : ApiUrl.Trim();
            _settings.IsDarkMode = IsDarkMode;
            _settings.FontScale = FontScale;
            _settings.ApplyThemeAndFont();

            _cuisineService.ClearCache();

            StatusMessage = "Settings saved. Dark mode and text size applied.";
            OnPropertyChanged(nameof(FontScaleLabel));
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    private async Task LogoutAsync()
    {
        _settings.IsLoggedIn = false;
        await Shell.Current.GoToAsync("//login");
    }
}
