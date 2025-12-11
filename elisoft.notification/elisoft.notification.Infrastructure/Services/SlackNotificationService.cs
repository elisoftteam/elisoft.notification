using elisoft.notification.Configuration.Configuration;
using elisoft.notification.Slack.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace elisoft.notification.Slack.Services
{
    public class SlackNotificationService : ISlackNotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfig _config;
        private readonly ILogger<SlackNotificationService> _logger;
        public SlackNotificationService(HttpClient httpclient, IConfig config, ILogger<SlackNotificationService> logger)
        {
            _httpClient = httpclient;
            _config = config;
            _logger = logger;
        }

        public async Task<bool> SendMessageAsync(string message)
        {
            var _webhookUrl = _config.SlackWebhookUrl;

            var payload = new { text = message };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(_webhookUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("udalo sie wyslac wiadomosc");
                    return true;
                }
                else
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                   _logger.LogError("nie udalo sie wyslac wiadomosci");
                    return false;
                }
            }
            catch (Exception ex)
            {
              _logger.LogError("wyjatek");
              return false;
            }
        }
    }
}