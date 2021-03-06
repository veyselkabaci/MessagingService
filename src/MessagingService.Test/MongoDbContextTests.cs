using MessagingService.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace MessagingService.Test;

public class MongoDbContextTests
{
    private Mock<IOptions<DatabaseSettings>> _mockOptions;
    private Mock<IMongoDatabase> _mockDB;
    private Mock<IMongoClient> _mockClient;

    public MongoDbContextTests()
    {
        _mockOptions = new Mock<IOptions<DatabaseSettings>>();
        _mockDB = new Mock<IMongoDatabase>();
        _mockClient = new Mock<IMongoClient>();
    }

    [Fact]
    public void MongoDBContext_Constructor_Success()
    {
        var settings = new DatabaseSettings
        {
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "Messaging"
        };

        _mockOptions.Setup(s => s.Value).Returns(settings);
        _mockClient.Setup(c => c
        .GetDatabase(_mockOptions.Object.Value.DatabaseName, null))
            .Returns(_mockDB.Object);

        //Act 
        var context = new MongoDBContext(_mockOptions.Object);

        //Assert 
        Assert.NotNull(context);
    }

    [Fact]
    public void MongoDBContext_Users_Success()
    {
        //Arrange
        var settings = new DatabaseSettings()
        {
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "Messaging",
            UsersCollectionName = "Users",
            MessagesCollectionName = "Messages"
        };

        _mockOptions.Setup(s => s.Value).Returns(settings);
        _mockClient.Setup(c => c
        .GetDatabase(_mockOptions.Object.Value.DatabaseName, null))
            .Returns(_mockDB.Object);

        //Act 
        var context = new MongoDBContext(_mockOptions.Object);
        var myCollection = context.Users;

        //Assert 
        Assert.NotNull(myCollection);
    }

    [Fact]
    public void MongoDBContext_Messages_Success()
    {
        //Arrange
        var settings = new DatabaseSettings()
        {
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "Messaging",
            UsersCollectionName = "Users",
            MessagesCollectionName = "Messages"
        };

        _mockOptions.Setup(s => s.Value).Returns(settings);
        _mockClient.Setup(c => c
        .GetDatabase(_mockOptions.Object.Value.DatabaseName, null))
            .Returns(_mockDB.Object);

        //Act 
        var context = new MongoDBContext(_mockOptions.Object);
        var myCollection = context.Messages;

        //Assert 
        Assert.NotNull(myCollection);
    }
}