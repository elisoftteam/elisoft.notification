using AutoFixture;
using Elisoft.Notificator.Configuration.Configuration;
using Elisoft.Notificator.Core.Handlers;
using Elisoft.Notificator.Core.Requests;
using Elisoft.Notificator.Twilio.Services;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Shouldly;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Elisoft.Notificator.Tests.Core
{
    [TestFixture]
    public class TwilioNotificationRequestHandlerTests
    {
        private Fixture _fixture;
        private ITwilioNotificator _twilioNotificatorFake;
        private IConfig _configFake;
        private ILogger<TwilioNotificationRequestHandler> _loggerFake;
        private TwilioNotificationRequestHandler _sut;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _twilioNotificatorFake = A.Fake<ITwilioNotificator>();
            _configFake = A.Fake<IConfig>();
            _loggerFake = A.Fake<ILogger<TwilioNotificationRequestHandler>>();

            A.CallTo(() => _configFake.TwilioAccountSid).Returns(_fixture.Create<string>());
            A.CallTo(() => _configFake.TwilioAuthToken).Returns(_fixture.Create<string>());
            A.CallTo(() => _configFake.TwilioFromNumber).Returns(_fixture.Create<string>());

            _sut = new TwilioNotificationRequestHandler(
                _twilioNotificatorFake,
                _configFake,
                _loggerFake
            );
        }

        [Test]
        public async Task HandleAsync_ValidCommand_CallSendSmsAsyncWithCorrectArgs()
        {
            // Arrange
            var command = _fixture.Create<TwilioNotificationRequest>();


            // Act
            var result = await _sut.HandleAsync(command, CancellationToken.None);


            // Assert
            A.CallTo(() => _twilioNotificatorFake.SendSmsAsync(
                    _configFake.TwilioAccountSid,
                    _configFake.TwilioAuthToken,
                    _configFake.TwilioFromNumber,
                    command.To,
                    command.Message))
                .MustHaveHappenedOnceExactly();

            result.ShouldBe(command);
        }

        [Test]
        public async Task HandleAsync_NotificatorThrowsException_ThrowException()
        {
            // Arrange
            var command = _fixture.Create<TwilioNotificationRequest>();
            var expectedException = new HttpRequestException("Twilio API unavailable");

            A.CallTo(() => _twilioNotificatorFake.SendSmsAsync(
                    A<string>._,
                    A<string>._,
                    A<string>._,
                    A<string>._,
                    A<string>._))
                .Throws(expectedException);


            // Act & Assert
            var exception = await Should.ThrowAsync<HttpRequestException>(async () =>
                await _sut.HandleAsync(command, CancellationToken.None));

            exception.Message.ShouldBe(expectedException.Message);
        }
    }
}
