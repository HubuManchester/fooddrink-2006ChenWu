using WorldCuisineApp.Models;

namespace WorldCuisineApp.Services;

public interface ICuisineService
{
    Task<IReadOnlyList<CuisineItem>> GetCuisinesAsync(CancellationToken cancellationToken = default);
    Task<CuisineItem?> GetCuisineByIdAsync(string id, CancellationToken cancellationToken = default);
    string LastDataSource { get; }
    void ClearCache();
}
