using AutoFixture;
using Elisoft.Notificator.Configuration.Configuration;
using Elisoft.Notificator.Core.Handlers;
using Elisoft.Notificator.Core.Requests;
using Elisoft.Pushover.Services;
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
    public class PushoverNotificationRequestHandlerTests
    {
        private Fixture _fixture;
        private IPushoverNotificator _pushoverNotificatorFake;
        private IConfig _configFake;
        private ILogger<PushoverNotificationRequestHandler> _loggerFake;
        private PushoverNotificationRequestHandler _sut;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _pushoverNotificatorFake = A.Fake<IPushoverNotificator>();
            _configFake = A.Fake<IConfig>();
            _loggerFake = A.Fake<ILogger<PushoverNotificationRequestHandler>>();

            A.CallTo(() => _configFake.PushoverApiToken).Returns(_fixture.Create<string>());
            A.CallTo(() => _configFake.PushoverUserKey).Returns(_fixture.Create<string>());

            _sut = new PushoverNotificationRequestHandler(
                _pushoverNotificatorFake,
                _configFake,
                _loggerFake
            );
        }

        [Test]
        public async Task HandleAsync_ValidCommand_CallSendMessageAsyncWithCorrectArgs()
        {
            // Arrange
            var command = _fixture.Create<PushoverNotificationRequest>();


            // Act
            var result = await _sut.HandleAsync(command, CancellationToken.None);


            // Assert
            A.CallTo(() => _pushoverNotificatorFake.SendMessageAsync(
                    _configFake.PushoverApiToken,
                    _configFake.PushoverUserKey,
                    command.Message,
                    command.Title,
                    command.Priority))
                .MustHaveHappenedOnceExactly();

            result.ShouldBe(command);
        }

        [Test]
        public async Task HandleAsync_NotificatorThrowsException_ThrowException()
        {
            // Arrange
            var command = _fixture.Create<PushoverNotificationRequest>();
            var expectedException = new HttpRequestException("Pushover API unavailable");

            A.CallTo(() => _pushoverNotificatorFake.SendMessageAsync(
                    A<string>._,
                    A<string>._,
                    A<string>._,
                    A<string?>._,
                    A<int?>._))
                .Throws(expectedException);


            // Act & Assert
            var exception = await Should.ThrowAsync<HttpRequestException>(async () =>
                await _sut.HandleAsync(command, CancellationToken.None));

            exception.Message.ShouldBe(expectedException.Message);
        }
    }
}
