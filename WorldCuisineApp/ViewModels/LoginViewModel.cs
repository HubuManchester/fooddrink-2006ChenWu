using System.Windows.Input;
using WorldCuisineApp.Services;

namespace WorldCuisineApp.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly ISettingsService _settings;
    private string _username = string.Empty;
    private string _email = string.Empty;
    private string _password = string.Empty;

    public LoginViewModel(ISettingsService settings)
    {
        _settings = settings;
        Title = "Sign In";
        LoginCommand = new Command(async () => await LoginAsync(), () => !IsBusy);
    }

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public ICommand LoginCommand { get; }

    public async Task LoginAsync()
    {
        ClearMessages();

        if (string.IsNullOrWhiteSpace(Username))
        {
            ErrorMessage = "Please enter your display name.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            ErrorMessage = "Please enter your email address.";
            return;
        }

        if (!Email.Contains('@') || !Email.Contains('.'))
        {
            ErrorMessage = "Please enter a valid email (must include @ and a domain).";
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Please enter your password.";
            return;
        }

        if (Password.Length < 6)
        {
            ErrorMessage = "Password must be at least 6 characters.";
            return;
        }

        try
        {
            IsBusy = true;
            await Task.Delay(400);

            _settings.Username = Username.Trim();
            _settings.IsLoggedIn = true;
            StatusMessage = $"Welcome, {_settings.Username}!";

            await Shell.Current.GoToAsync("//home");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Login failed: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
