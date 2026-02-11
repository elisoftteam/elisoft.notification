using System.Net;
using System.Net.Http.Json;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Elisoft.Slack;
using Elisoft.Notificator.Configuration.Configuration;
using Elisoft.Teams.Services;
using Microsoft.VisualStudio.TestPlatform.TestHost;


namespace Elisoft.Notificator.IntegrationTests
{
    [TestFixture]
    public class NotificationIntegrationTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private ISlackNotificator _slackFake;
        private ITeamsNotificator _teamsFake;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _slackFake = A.Fake<ISlackNotificator>();
            _teamsFake = A.Fake<ITeamsNotificator>();

            var configFake = A.Fake<IConfig>();
            A.CallTo(() => configFake.ApiKey).Returns("test-api-key");

            _factory = new WebApplicationFactory<Program>()
              .WithWebHostBuilder(builder =>
              {
                  builder.ConfigureServices(services =>
                  {
                      services.AddSingleton(_slackFake);
                      services.AddSingleton(_teamsFake);
                      services.AddSingleton(configFake);
                  });
              });

            _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Add("X-API-KEY", "test-api-key");
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task SendNotification_validSlackPayload_returnsOk()
        {
            // Arrange
            var body = new
            {
                channel = "Slack",
                payload = new
                {
                    webhookUrl = "https://hooks.slack.com/test",
                    channelName = "#general",
                    message = "hello"
                }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/notification/send", body);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Test]
        public async Task SendNotification_validSlackPayload_sendsMessageToSlack()
        {
            // Arrange
            var body = new
            {
                channel = "Slack",
                payload = new
                {
                    webhookUrl = "https://hooks.slack.com/test",
                    channelName = "#general",
                    message = "hello"
                }
            };

            // Act
            await _client.PostAsJsonAsync("/api/notification/send", body);

            // Assert
            A.CallTo(() => _slackFake.SendMessageAsync(
              A<string>._,
              A<string>._))
              .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task SendNotification_validTeamsPayload_returnsOk()
        {
            // Arrange
            var body = new
            {
                channel = "Teams",
                payload = new
                {
                    webhookUrl = "https://outlook.office.com/webhook/test",
                    message = "hello"
                }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/notification/send", body);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Test]
        public async Task SendNotification_validTeamsPayload_sendsMessageToTeams()
        {
            // Arrange
            var body = new
            {
                channel = "Teams",
                payload = new
                {
                    webhookUrl = "https://outlook.office.com/webhook/test",
                    message = "hello"
                }
            };

            // Act
            await _client.PostAsJsonAsync("/api/notification/send", body);

            // Assert
            A.CallTo(() => _teamsFake.SendMessageAsync(
                A<string>._,
                A<string>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task SendNotification_missingApiKey_returnsUnauthorized()
        {
            // Arrange
            _client.DefaultRequestHeaders.Remove("X-API-KEY");

            // Act
            var response = await _client.PostAsJsonAsync("/api/notification/send", new { });

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task SendNotification_invalidApiKey_returnsUnauthorized()
        {
            // Arrange
            _client.DefaultRequestHeaders.Remove("X-API-KEY");
            _client.DefaultRequestHeaders.Add("X-API-KEY", "wrong-key");

            // Act
            var response = await _client.PostAsJsonAsync("/api/notification/send", new { });

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task SendNotification_missingPayload_returnsBadRequest()
        {
            // Arrange
            var body = new
            {
                channel = "Slack"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/notification/send", body);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task SendNotification_invalidChannel_returnsBadRequest()
        {
            // Arrange
            var body = new
            {
                channel = "szkola",
                payload = new { any = "value" }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/notification/send", body);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}
