using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace MessagingService.Test;

public class TestClientProvider
{
    public HttpClient Client { get; }
    public TestClientProvider()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                // ... Configure test services
            });

        Client = application.CreateClient();
    }
}