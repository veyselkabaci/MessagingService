using MessagingService.Api.Models;

namespace MessagingService.Api.Services;

public interface IAuthService
{
    Task<string> Authenticate(User authUser);
}