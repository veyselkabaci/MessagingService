using System.Threading.Tasks;
using MessagingService.Api.Models;
using MessagingService.Api.Services;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace MessagingService.Test;

public class AuthTests
{
    private Mock<IOptions<JwtSettings>> _mockOptions;
    private Mock<IUserService> _mockUserService;
    private User _user;
    private readonly string key = "cd4dc76feeeb50aeb22ac7d1aabee1b00591d62d95f6dadc9ba1a9142b2acfe1";

    public AuthTests()
    {
        _mockOptions = new Mock<IOptions<JwtSettings>>();
        _user = new User
        {
            Username = "test",
            Password = "test"
        };
        _mockUserService = new Mock<IUserService>();
    }

    [Fact]
    public async Task AuthService_Authenticate_Success()
    {
        //Arrange
        var jwtSettings = new JwtSettings
        {
            SecretKey = key
        };
        _mockOptions.Setup(s => s.Value).Returns(jwtSettings);

        _mockUserService.Setup(s => s.GetAsyncWithUsernamePassword(_user.Username, _user.Password)).ReturnsAsync(_user);

        var authService = new AuthService(_mockUserService.Object, _mockOptions.Object);

        //Act
        var token = await authService.Authenticate(_user);

        //Assert 
        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public async Task AuthService_Authenticate_Failure()
    {
        //Arrange
        var jwtSettings = new JwtSettings
        {
            SecretKey = key
        };
        _mockOptions.Setup(s => s.Value).Returns(jwtSettings);

        _mockUserService.Setup(s => s.GetAsyncWithUsernamePassword(_user.Username, _user.Password)).ReturnsAsync(value: null);

        var authService = new AuthService(_mockUserService.Object, _mockOptions.Object);

        //Act
        var token = await authService.Authenticate(_user);

        //Assert 
        Assert.Empty(token);
    }
}