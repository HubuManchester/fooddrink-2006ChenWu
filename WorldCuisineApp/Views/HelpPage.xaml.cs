using WorldCuisineApp.ViewModels;

namespace WorldCuisineApp.Views;

public partial class HelpPage : ContentPage
{
    public HelpPage(HelpViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnBackClicked(object? sender, EventArgs e) =>
        await Shell.Current.GoToAsync("..");
}
