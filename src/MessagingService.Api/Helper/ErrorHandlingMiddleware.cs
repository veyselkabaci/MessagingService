using MessagingService.Api.Models;
using Microsoft.AspNetCore.Http.Extensions;

namespace MessagingService.Api.Helpers;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ErrorHandlingMiddleware> logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        this.next = next ?? throw new ArgumentNullException(nameof(next));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception exception)
        {
            if (exception is NotificationException notificationException)
            {
                context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                await context.Response.WriteAsync(notificationException.Message);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Hata oluştu.");
            }
            logger.LogError(exception
            , "{@Path} url'ine yapılan {@Method} isteği sırasında hata oluştu. Hata: {@Message}."
            , UriHelper.GetDisplayUrl(context.Request)
            , context.Request.Method
            , exception.Message);
        }
    }
}
