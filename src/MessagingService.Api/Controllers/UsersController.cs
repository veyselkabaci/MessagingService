using MessagingService.Api.Helpers;
using MessagingService.Api.Models;
using MessagingService.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace MessagingService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUserService _userService;

    public UsersController(
        ILogger<UsersController> logger,
        IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [Authorize]
    [HttpGet]
    public async Task<List<User>> Get()
    {
        var list = await _userService.GetAsync();
        _logger.LogInformation("Kullanıcılar listelendi.");
        return list;
    }

    [Authorize]
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<User>> Get(string id)
    {
        var user = await _userService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost]
    public async Task<IActionResult> Create(User newUser)
    {
        await _userService.CreateAsync(newUser);
        _logger.LogInformation($"Kullanıcı oluşturuldu: {newUser.Username}");

        return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
    }
}