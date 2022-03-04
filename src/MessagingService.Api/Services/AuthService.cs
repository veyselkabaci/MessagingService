using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MessagingService.Api.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MessagingService.Api.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly string key;

    public AuthService(
        IUserService userService,
        IOptions<JwtSettings> jwtSettings)
    {
        _userService = userService;
        key = jwtSettings.Value.SecretKey;
    }

    public async Task<string> Authenticate(User authUser)
    {
        var user = await _userService.GetAsyncWithUsernamePassword(authUser.Username, authUser.Password);
        if (user is null)
            return string.Empty;

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.ASCII.GetBytes(key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[] { new Claim("username", authUser.Username) }),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}