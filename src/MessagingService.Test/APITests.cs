using System;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MessagingService.Api.Models;
using Newtonsoft.Json;
using Xunit;

namespace MessagingService.Test;

public class APITests
{
    private User _user;
    public APITests()
    {
        _user = new User
        {
            Username = "test_01",
            Password = "test_01"
        };
    }

    [Fact]
    public async Task Login()
    {
        var client = new TestClientProvider().Client;

        var result = await client.PostAsJsonAsync("api/auth", _user);
        result.EnsureSuccessStatusCode();

        var str = await result.Content.ReadAsStringAsync();
        var obj = JsonConvert.DeserializeObject<TokenResponse>(str);

        Assert.NotNull(obj.Token);
        Assert.NotEmpty(obj.Token);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task GetUsers()
    {
        var client = new TestClientProvider().Client;

        var authResult = await client.PostAsJsonAsync("api/auth", _user);
        authResult.EnsureSuccessStatusCode();
        var str = await authResult.Content.ReadAsStringAsync();
        var obj = JsonConvert.DeserializeObject<TokenResponse>(str);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", obj.Token);
        var result = await client.GetAsync("api/users");
        result.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task CreateUser()
    {
        var client = new TestClientProvider().Client;

        var guid = Guid.NewGuid().ToString();
        var result = await client.PostAsJsonAsync("api/users", new User { Username = guid, Password = guid });
        result.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
    }

    [Fact]
    public async Task GetMessages()
    {
        var client = new TestClientProvider().Client;

        var authResult = await client.PostAsJsonAsync("api/auth", _user);
        authResult.EnsureSuccessStatusCode();
        var str = await authResult.Content.ReadAsStringAsync();
        var obj = JsonConvert.DeserializeObject<TokenResponse>(str);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", obj.Token);
        var result = await client.GetAsync("api/messages?username=test_02");
        result.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task SendMessage()
    {
        var client = new TestClientProvider().Client;

        var authResult = await client.PostAsJsonAsync("api/auth", _user);
        authResult.EnsureSuccessStatusCode();
        var str = await authResult.Content.ReadAsStringAsync();
        var obj = JsonConvert.DeserializeObject<TokenResponse>(str);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", obj.Token);
        var result = await client.PostAsJsonAsync("api/messages",
            new MessageRequest { ReceiverUsername = "test_02", Text = "test text" });
        result.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }
}