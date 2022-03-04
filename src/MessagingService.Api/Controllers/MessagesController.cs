using MessagingService.Api.Helpers;
using MessagingService.Api.Models;
using MessagingService.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace MessagingService.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IMessageService _messageService;

    public MessagesController(ILogger<UsersController> logger, IMessageService messageService)
    {
        _logger = logger;
        _messageService = messageService;
    }

    [HttpGet]
    public async Task<List<Message>> Get(string username)
    {
        var list = await _messageService.GetAsync(username);
        _logger.LogInformation("Mesajlar listelendi.");
        return list;
    }

    [HttpPost]
    public async Task<IActionResult> Send(MessageRequest newMessage)
    {
        await _messageService.CreateAsync(newMessage);
        _logger.LogInformation($"Mesaj gönderildi.");

        return Ok();
    }
}