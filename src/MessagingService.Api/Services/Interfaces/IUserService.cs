using MessagingService.Api.Models;

namespace MessagingService.Api.Services;

public interface IUserService
{
    Task<List<User>> GetAsync();
    Task<User?> GetAsync(string id);
    Task<User?> GetAsyncWithUsername(string username);
    Task<User?> GetAsyncWithUsernamePassword(string username, string password);
    Task CreateAsync(User newUser);
}