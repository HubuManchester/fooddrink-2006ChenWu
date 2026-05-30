using System.Windows.Input;
using WorldCuisineApp.Services;

namespace WorldCuisineApp.ViewModels;

public class RegisterViewModel : BaseViewModel
{
    private readonly IUserService _userService;
    private string _username = string.Empty;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _confirmPassword = string.Empty;

    public RegisterViewModel(IUserService userService)
    {
        _userService = userService;
        Title = "Create Account";
        RegisterCommand = new Command(async () => await RegisterAsync(), () => !IsBusy);
        GoToLoginCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
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

    public string ConfirmPassword
    {
        get => _confirmPassword;
        set => SetProperty(ref _confirmPassword, value);
    }

    public ICommand RegisterCommand { get; }
    public ICommand GoToLoginCommand { get; }

    private async Task RegisterAsync()
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
            ErrorMessage = "Please enter a password.";
            return;
        }

        if (Password.Length < 6)
        {
            ErrorMessage = "Password must be at least 6 characters.";
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Passwords do not match.";
            return;
        }

        try
        {
            IsBusy = true;
            var (success, error) = await _userService.RegisterAsync(Username.Trim(), Email.Trim(), Password);

            if (success)
            {
                StatusMessage = $"Account created ({_userService.LastDataSource}). You can now sign in.";
                Username = string.Empty;
                Email = string.Empty;
                Password = string.Empty;
                ConfirmPassword = string.Empty;
            }
            else
            {
                ErrorMessage = error;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Registration failed: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
