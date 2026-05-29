using WorldCuisineApp.Services;
using WorldCuisineApp.ViewModels;

namespace WorldCuisineApp.Views;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel;
    private readonly IHardwareService _hardware;

    public HomePage(HomeViewModel viewModel, IHardwareService hardware)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _hardware = hardware;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
        _hardware.StartShakeDetection(async () => await _viewModel.OnShakeAsync());
    }

    protected override void OnDisappearing()
    {
        _hardware.StopShakeDetection();
        base.OnDisappearing();
    }
}
