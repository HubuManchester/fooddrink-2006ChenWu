using System.Net.Http.Json;
using System.Text.Json;
using WorldCuisineApp.Constants;
using WorldCuisineApp.Models;

namespace WorldCuisineApp.Services;

/// <summary>
/// Loads cuisine data from MockAPI when configured; falls back to bundled JSON on failure.
/// </summary>
public class CuisineService : ICuisineService
{
    private readonly HttpClient _httpClient;
    private readonly ISettingsService _settings;
    private IReadOnlyList<CuisineItem>? _cache;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public CuisineService(HttpClient httpClient, ISettingsService settings)
    {
        _httpClient = httpClient;
        _settings = settings;
        _httpClient.Timeout = TimeSpan.FromSeconds(12);
    }

    public string LastDataSource { get; private set; } = "Unknown";

    public void ClearCache() => _cache = null;

    public async Task<IReadOnlyList<CuisineItem>> GetCuisinesAsync(CancellationToken cancellationToken = default)
    {
        if (_cache is not null)
            return _cache;

        try
        {
            var apiUrl = _settings.ApiUrl?.Trim();
            if (IsApiConfigured(apiUrl))
            {
                var fromApi = await _httpClient.GetFromJsonAsync<List<CuisineItem>>(apiUrl!, JsonOptions, cancellationToken);
                if (fromApi is { Count: > 0 })
                {
                    _cache = fromApi;
                    LastDataSource = "MockAPI";
                    return _cache;
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"API load failed: {ex.Message}");
        }

        _cache = await LoadFallbackAsync(cancellationToken);
        LastDataSource = "Local fallback";
        return _cache;
    }

    public async Task<CuisineItem?> GetCuisineByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var list = await GetCuisinesAsync(cancellationToken);
        return list.FirstOrDefault(c => c.Id == id);
    }

    private static bool IsApiConfigured(string? url) =>
        !string.IsNullOrWhiteSpace(url)
        && !url.Contains("YOUR_PROJECT_ID", StringComparison.OrdinalIgnoreCase)
        && Uri.TryCreate(url, UriKind.Absolute, out _);

    private static async Task<IReadOnlyList<CuisineItem>> LoadFallbackAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var stream = await FileSystem.OpenAppPackageFileAsync("cuisines_fallback.json");
            var items = await JsonSerializer.DeserializeAsync<List<CuisineItem>>(stream, JsonOptions, cancellationToken);
            if (items is { Count: > 0 })
                return items;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Fallback load failed: {ex.Message}");
        }

        return GetHardcodedFallback();
    }

    private static List<CuisineItem> GetHardcodedFallback() =>
    [
        new CuisineItem
        {
            Id = "offline-1",
            Name = "Margherita Pizza",
            Country = "Italy",
            Region = "Europe",
            Description = "Tomato, mozzarella, and basil on a thin crust.",
            SpiceLevel = 1,
            FunFact = "Named after Queen Margherita of Savoy."
        }
    ];
}
