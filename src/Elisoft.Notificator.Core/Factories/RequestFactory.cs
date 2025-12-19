using Elisoft.Notificator.Core.Enums;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.Json;

namespace Elisoft.Notificator.Core.Factories
{
  public class RequestFactory : IRequestFactory
  {
    private readonly ILogger<RequestFactory> _logger;

    public RequestFactory(ILogger<RequestFactory> logger)
    {
      _logger = logger;
    }

    public object CreateRequest(NotificationEnumChannel channel, JsonElement jsonPayload)
    {
      var requestTypeName = $"{channel}NotificationRequest";

      var targetType = Assembly.GetExecutingAssembly()
          .GetTypes()
          .FirstOrDefault(t => t.Name.Equals(requestTypeName, StringComparison.InvariantCultureIgnoreCase)
                               && !t.IsInterface
                               && !t.IsAbstract);

      if (targetType == null)
      {
        _logger.LogError($"Request class not found for channel: {channel}");
        throw new InvalidOperationException($"Channel {channel} is not supported (missing Request class).");
      }

      try
      {
        var command = JsonSerializer.Deserialize(jsonPayload.GetRawText(), targetType, new JsonSerializerOptions
        {
          PropertyNameCaseInsensitive = true
        });

        if (command == null) throw new ArgumentException("Payload is null");
        return command;
      }
      catch (JsonException ex)
      {
        throw new ArgumentException($"Invalid payload structure for {targetType.Name}", ex);
      }
    }
  }
}