using MessagingService.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MessagingService.Api.Services;

public class UserService : IUserService
{
    private readonly MongoDBContext _context;

    public UserService(
        MongoDBContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAsync() =>
        await _context.Users.Find(_ => true).ToListAsync();

    public async Task<User?> GetAsync(string id) =>
        await _context.Users.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<User?> GetAsyncWithUsername(string username) =>
        await _context.Users.Find(x => x.Username == username).FirstOrDefaultAsync();

    public async Task<User?> GetAsyncWithUsernamePassword(string username, string password) =>
        await _context.Users.Find(x => x.Username == username && x.Password == password).FirstOrDefaultAsync();

    public async Task CreateAsync(User newUser)
    {
        var user = await GetAsyncWithUsername(newUser.Username);
        if (user != null)
            throw new NotificationException("Kullanıcı daha önce oluşturulmuş");
        await _context.Users.InsertOneAsync(newUser);
    }
}