using MessagingService.Api.Models;
using MessagingService.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace MessagingService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IAuthService _authService;

    public AuthController(
        ILogger<UsersController> logger,
        IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] User user)
    {
        var token = await _authService.Authenticate(user);

        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized();
        }

        _logger.LogInformation($"Kullanıcı girişi yapıldı: {user.Username}");

        return Ok(new TokenResponse { Username = user.Username, Token = token });
    }
}