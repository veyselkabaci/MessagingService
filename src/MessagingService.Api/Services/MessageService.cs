using AutoMapper;
using MessagingService.Api.Models;
using MongoDB.Driver;

namespace MessagingService.Api.Services;

public class MessageService : IMessageService
{
    private readonly MongoDBContext _context;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public MessageService(
        MongoDBContext context,
        IHttpContextAccessor contextAccessor,
        IUserService userService,
        IMapper mapper)
    {
        _context = context;
        _contextAccessor = contextAccessor;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<List<Message>> GetAsync(string username)
    {
        var user = _contextAccessor.HttpContext?.Items["User"] as User;
        return await _context.Messages
            .Find(x =>
                x.SenderUsername == user.Username && x.ReceiverUsername == username
                || x.SenderUsername == username && x.ReceiverUsername == user.Username)
            .ToListAsync();
    }

    public async Task CreateAsync(MessageRequest newMessage)
    {
        var user = _contextAccessor.HttpContext?.Items["User"] as User;
        var receiver = await _userService.GetAsyncWithUsername(newMessage.ReceiverUsername);
        if (receiver is null)
            throw new NotificationException("Alıcı bulunamadı.");

        var message = _mapper.Map<Message>(newMessage);
        message.SenderUsername = user.Username;
        message.Date = DateTime.UtcNow;

        await _context.Messages.InsertOneAsync(message);
    }
}