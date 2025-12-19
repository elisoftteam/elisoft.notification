using Elisoft.Notificator.Core.Factories;
using Elisoft.Notificator.Core.Models;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Elisoft.Notificator.Core.Services
{
  public interface INotificationService
  {
    Task DispatchNotificationAsync(NotificationDto dto);
  }

  public class NotificationService : INotificationService
  {
    private readonly IAmACommandProcessor _commandProcessor;
    private readonly IRequestFactory _requestFactory;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        IAmACommandProcessor commandProcessor,
        IRequestFactory requestFactory,
        ILogger<NotificationService> logger)
    {
      _commandProcessor = commandProcessor;
      _requestFactory = requestFactory;
      _logger = logger;
    }

    public async Task DispatchNotificationAsync(NotificationDto dto)
    {
      _logger.LogInformation($"Processing a notification for a channel: {dto.Channel}");
      var command = _requestFactory.CreateRequest(dto.Channel, dto.Payload);

      await ((dynamic)_commandProcessor).SendAsync((dynamic)command);
    }
  }
}