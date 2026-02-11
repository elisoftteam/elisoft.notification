using System.Net;
using System.Net.Http.Json;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Elisoft.Slack;
using Elisoft.Notificator.Configuration.Configuration;
using Elisoft.Pushover.Services;
using Elisoft.Teams.Services;
using Elisoft.Notificator.Twilio.Services;
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
        private IPushoverNotificator _pushoverFake;
        private ITwilioNotificator _twilioFake;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _slackFake = A.Fake<ISlackNotificator>();
            _teamsFake = A.Fake<ITeamsNotificator>();
            _pushoverFake = A.Fake<IPushoverNotificator>();
            _twilioFake = A.Fake<ITwilioNotificator>();

            var configFake = A.Fake<IConfig>();
            A.CallTo(() => configFake.ApiKey).Returns("test-api-key");
            A.CallTo(() => configFake.PushoverApiToken).Returns("test-token");
            A.CallTo(() => configFake.PushoverUserKey).Returns("test-user");
            A.CallTo(() => configFake.TwilioAccountSid).Returns("test-account-sid");
            A.CallTo(() => configFake.TwilioAuthToken).Returns("test-auth-token");
            A.CallTo(() => configFake.TwilioFromNumber).Returns("+15550001111");

            _factory = new WebApplicationFactory<Program>()
              .WithWebHostBuilder(builder =>
              {
                  builder.ConfigureServices(services =>
                  {
                      services.AddSingleton(_slackFake);
                      services.AddSingleton(_teamsFake);
                      services.AddSingleton(_pushoverFake);
                      services.AddSingleton(_twilioFake);
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
        public async Task SendNotification_validPushoverPayload_returnsOk()
        {
            // Arrange
            var body = new
            {
                channel = "Pushover",
                payload = new
                {
                    message = "hello",
                    title = "notification",
                    priority = 0
                }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/notification/send", body);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Test]
        public async Task SendNotification_validPushoverPayload_sendsMessageToPushover()
        {
            // Arrange
            var body = new
            {
                channel = "Pushover",
                payload = new
                {
                    message = "hello",
                    title = "notification",
                    priority = 0
                }
            };

            // Act
            await _client.PostAsJsonAsync("/api/notification/send", body);

            // Assert
            A.CallTo(() => _pushoverFake.SendMessageAsync(
                A<string>._,
                A<string>._,
                A<string>._,
                A<string?>._,
                A<int?>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task SendNotification_validTwilioPayload_returnsOk()
        {
            // Arrange
            var body = new
            {
                channel = "Twilio",
                payload = new
                {
                    to = "+15550002222",
                    message = "hello"
                }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/notification/send", body);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Test]
        public async Task SendNotification_validTwilioPayload_sendsMessageToTwilio()
        {
            // Arrange
            var body = new
            {
                channel = "Twilio",
                payload = new
                {
                    to = "+15550002222",
                    message = "hello"
                }
            };

            // Act
            await _client.PostAsJsonAsync("/api/notification/send", body);

            // Assert
            A.CallTo(() => _twilioFake.SendSmsAsync(
                A<string>._,
                A<string>._,
                A<string>._,
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
