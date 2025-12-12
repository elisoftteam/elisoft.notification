using elisoft.notification.Slack.Interfaces;
using Elisoft.Notification.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Elisoft.Notification.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ISlackNotificationService _slackService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(ISlackNotificationService slackService, ILogger<NotificationController> logger)
        {
            _slackService = slackService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
            {
                _logger.LogWarning("pusta wiadomosc");
                return BadRequest("tresc jest wymagana");
            }

            _logger.LogInformation("otrzymano zadanie wyslania powiadomienia");

            var result = await _slackService.SendMessageAsync(request.Message);

            if (result)
            {
                return Ok(new { status = "Wyslano", timestamp = DateTime.UtcNow });
            }
            else
            {
                return StatusCode(500, "nie udalo sie wyslac wiadomosci do Slacka");
            }
        }
    }
}