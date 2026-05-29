using WorldCuisineApp.ViewModels;

namespace WorldCuisineApp.Views;

[QueryProperty(nameof(CuisineId), "id")]
public partial class CuisineDetailPage : ContentPage
{
    private readonly CuisineDetailViewModel _viewModel;
    private string _cuisineId = string.Empty;

    public CuisineDetailPage(CuisineDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    public string CuisineId
    {
        get => _cuisineId;
        set
        {
            _cuisineId = value;
            _ = _viewModel.LoadAsync(value);
        }
    }

    private async void OnOpenCameraClicked(object? sender, EventArgs e) =>
        await Shell.Current.GoToAsync(nameof(CameraPage));
}
