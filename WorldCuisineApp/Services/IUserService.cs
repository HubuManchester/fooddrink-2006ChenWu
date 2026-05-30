using WorldCuisineApp.Models;

namespace WorldCuisineApp.Services;

public interface IUserService
{
    Task<(bool Success, string Error)> RegisterAsync(string username, string email, string password);
    Task<(bool Success, string Error)> LoginAsync(string email, string password);
    string LastDataSource { get; }
}
