namespace MessagingService.Api.Models;

public class MessageRequest
{
    public string ReceiverUsername { get; set; } = null!;
    public string Text { get; set; } = null!;
}