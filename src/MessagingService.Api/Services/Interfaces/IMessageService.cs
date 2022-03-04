using MessagingService.Api.Models;

namespace MessagingService.Api.Services;

public interface IMessageService
{
    Task<List<Message>> GetAsync(string username);
    Task CreateAsync(MessageRequest newMessage);
}