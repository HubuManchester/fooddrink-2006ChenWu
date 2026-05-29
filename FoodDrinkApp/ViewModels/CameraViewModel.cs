using System.Windows.Input;
using WorldCuisineApp.Services;

namespace WorldCuisineApp.ViewModels;

public class CameraViewModel : BaseViewModel
{
    private readonly IHardwareService _hardware;
    private ImageSource? _photo;
    private string _photoPath = string.Empty;

    public CameraViewModel(IHardwareService hardware)
    {
        _hardware = hardware;
        Title = "Food Camera";
        CaptureCommand = new Command(async () => await CaptureAsync());
        GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
    }

    public ImageSource? Photo
    {
        get => _photo;
        set
        {
            if (SetProperty(ref _photo, value))
                OnPropertyChanged(nameof(HasPhoto));
        }
    }

    public bool HasPhoto => Photo is not null;

    public string PhotoPath
    {
        get => _photoPath;
        set => SetProperty(ref _photoPath, value);
    }

    public ICommand CaptureCommand { get; }
    public ICommand GoBackCommand { get; }

    private async Task CaptureAsync()
    {
        try
        {
            ClearMessages();
            var file = await _hardware.CapturePhotoAsync();
            if (file is null)
            {
                ErrorMessage = "Camera unavailable or capture was cancelled.";
                _hardware.VibrateError();
                return;
            }

            PhotoPath = file.FullPath;
            Photo = ImageSource.FromFile(file.FullPath);
            _hardware.VibrateSuccess();
            StatusMessage = "Photo captured! Use this for your food diary demo.";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Camera error: {ex.Message}";
            _hardware.VibrateError();
        }
    }
}
