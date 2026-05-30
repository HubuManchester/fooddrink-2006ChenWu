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
        PickCommand = new Command(async () => await PickAsync());
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
    public ICommand PickCommand { get; }
    public ICommand GoBackCommand { get; }

    private async Task CaptureAsync()
    {
        await LoadPhotoAsync(() => _hardware.CapturePhotoAsync());
    }

    private async Task PickAsync()
    {
        await LoadPhotoAsync(() => _hardware.PickPhotoAsync());
    }

    private async Task LoadPhotoAsync(Func<Task<FileResult?>> getFile)
    {
        try
        {
            ClearMessages();
            IsBusy = true;
            var file = await getFile();
            if (file is null)
                return; // User cancelled, no error

            var localPath = Path.Combine(FileSystem.CacheDirectory, $"{Guid.NewGuid():N}.jpg");
            using (var sourceStream = await file.OpenReadAsync())
            using (var destStream = File.OpenWrite(localPath))
            {
                await sourceStream.CopyToAsync(destStream);
            }

            PhotoPath = localPath;
            Photo = ImageSource.FromFile(localPath);
            _hardware.VibrateSuccess();
            StatusMessage = "Photo loaded!";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
            _hardware.VibrateError();
        }
        finally
        {
            IsBusy = false;
        }
    }
}
