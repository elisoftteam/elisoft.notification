using Elisoft.Notificator.Core.Models;
using Elisoft.Notificator.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Elisoft.Notificator.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class NotificationController : ControllerBase
  {
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
      _notificationService = notificationService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromBody] NotificationDto request)
    {
      if (request == null)
      {
        return BadRequest("Body cannot be empty.");
      }

      try
      {
        await _notificationService.DispatchNotificationAsync(request);
        return Ok(new { Status = "Succes", Channel = request.Channel.ToString() });
      } 
      catch (ArgumentException ex)
      {
        return BadRequest($"Validation Error: {ex.Message}");
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Internal Error: {ex.Message}");
      }
    }
  }
}