using System.Net.Http.Json;
using System.Text.Json;
using WorldCuisineApp.Constants;
using WorldCuisineApp.Models;

namespace WorldCuisineApp.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly ISettingsService _settings;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public UserService(HttpClient httpClient, ISettingsService settings)
    {
        _httpClient = httpClient;
        _settings = settings;
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    public string LastDataSource { get; private set; } = "Unknown";

    public async Task<(bool Success, string Error)> RegisterAsync(string username, string email, string password)
    {
        try
        {
            var usersUrl = GetUsersEndpoint();
            if (usersUrl is not null)
            {
                // Check if email already exists on MockAPI
                var existing = await _httpClient.GetFromJsonAsync<List<User>>($"{usersUrl}?email={Uri.EscapeDataString(email)}", JsonOptions);
                if (existing is { Count: > 0 })
                {
                    LastDataSource = "MockAPI";
                    return (false, "This email is already registered.");
                }

                var newUser = new User { Username = username, Email = email, Password = password };
                var response = await _httpClient.PostAsJsonAsync(usersUrl, newUser, JsonOptions);
                if (response.IsSuccessStatusCode)
                {
                    LastDataSource = "MockAPI";
                    return (true, string.Empty);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"MockAPI register failed: {ex.Message}");
        }

        // Fallback to local storage
        return RegisterLocal(username, email, password);
    }

    public async Task<(bool Success, string Error)> LoginAsync(string email, string password)
    {
        try
        {
            var usersUrl = GetUsersEndpoint();
            if (usersUrl is not null)
            {
                var matches = await _httpClient.GetFromJsonAsync<List<User>>(
                    $"{usersUrl}?email={Uri.EscapeDataString(email)}&password={Uri.EscapeDataString(password)}", JsonOptions);

                if (matches is { Count: > 0 })
                {
                    var user = matches[0];
                    _settings.Username = user.Username;
                    LastDataSource = "MockAPI";
                    return (true, string.Empty);
                }

                // Email exists but password wrong?
                var emailMatches = await _httpClient.GetFromJsonAsync<List<User>>(
                    $"{usersUrl}?email={Uri.EscapeDataString(email)}", JsonOptions);

                if (emailMatches is { Count: > 0 })
                {
                    LastDataSource = "MockAPI";
                    return (false, "Incorrect password.");
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"MockAPI login failed: {ex.Message}");
        }

        // Fallback to local
        return LoginLocal(email, password);
    }

    private string? GetUsersEndpoint()
    {
        var apiUrl = _settings.ApiUrl?.Trim();
        if (string.IsNullOrWhiteSpace(apiUrl) || apiUrl.Contains("YOUR_PROJECT_ID", StringComparison.OrdinalIgnoreCase))
            return null;

        try
        {
            var uri = new Uri(apiUrl);
            var baseUrl = uri.GetLeftPart(UriPartial.Authority);
            return $"{baseUrl}/users";
        }
        catch
        {
            return null;
        }
    }

    private (bool, string) RegisterLocal(string username, string email, string password)
    {
        var users = LoadLocalUsers();

        if (users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
        {
            LastDataSource = "Local";
            return (false, "This email is already registered.");
        }

        users.Add(new User
        {
            Id = Guid.NewGuid().ToString("N")[..8],
            Username = username,
            Email = email,
            Password = password
        });

        SaveLocalUsers(users);
        LastDataSource = "Local storage";
        return (true, string.Empty);
    }

    private (bool, string) LoginLocal(string email, string password)
    {
        var users = LoadLocalUsers();
        var user = users.FirstOrDefault(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        if (user is null)
        {
            LastDataSource = "Local storage";
            return (false, "No account found with this email. Please register first.");
        }

        if (user.Password != password)
        {
            LastDataSource = "Local storage";
            return (false, "Incorrect password.");
        }

        _settings.Username = user.Username;
        LastDataSource = "Local storage";
        return (true, string.Empty);
    }

    private List<User> LoadLocalUsers()
    {
        var raw = Preferences.Default.Get(AppConstants.PrefLocalUsers, string.Empty);
        if (string.IsNullOrWhiteSpace(raw))
            return [];

        try
        {
            return JsonSerializer.Deserialize<List<User>>(raw, JsonOptions) ?? [];
        }
        catch
        {
            return [];
        }
    }

    private void SaveLocalUsers(List<User> users) =>
        Preferences.Default.Set(AppConstants.PrefLocalUsers, JsonSerializer.Serialize(users, JsonOptions));
}
