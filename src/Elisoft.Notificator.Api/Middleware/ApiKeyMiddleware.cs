using Elisoft.Notificator.Configuration.Configuration;
using System.Net;

namespace Elisoft.Notificator.Api.Middleware
{
  public class ApiKeyMiddleware
  {
    private const string ApiKeyHeaderName = "X-API-KEY";
    private readonly RequestDelegate _next;

    public ApiKeyMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IConfig config)
    {
      if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedKey))
      {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        await context.Response.WriteAsync("API Key missing");
        return;
      }

      if (!string.Equals(extractedKey, config.ApiKey))
      {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        await context.Response.WriteAsync("Invalid API Key");
        return;
      }

      await _next(context);
    }
  }
}
