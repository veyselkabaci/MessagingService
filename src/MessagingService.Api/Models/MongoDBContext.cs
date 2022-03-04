using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MessagingService.Api.Models;

public class MongoDBContext
{
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IOptions<DatabaseSettings> _databaseSettings;

    public MongoDBContext(
        IOptions<DatabaseSettings> databaseSettings)
    {
        var client = new MongoClient(databaseSettings.Value.ConnectionString);
        _mongoDatabase = client.GetDatabase(databaseSettings.Value.DatabaseName);
        _databaseSettings = databaseSettings;
    }

    public IMongoCollection<User> Users => _mongoDatabase.GetCollection<User>(_databaseSettings.Value.UsersCollectionName);
    public IMongoCollection<Message> Messages => _mongoDatabase.GetCollection<Message>(_databaseSettings.Value.MessagesCollectionName);
}