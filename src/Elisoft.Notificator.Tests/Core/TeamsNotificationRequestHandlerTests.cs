using AutoFixture;
using Elisoft.Notificator.Core.Handlers;
using Elisoft.Notificator.Core.Requests;
using Elisoft.Teams.Services;
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
    public class TeamsNotificationRequestHandlerTests
    {
        private Fixture _fixture;
        private ITeamsNotificator _teamsNotificatorFake;
        private ILogger<TeamsNotificationRequestHandler> _loggerFake;
        private TeamsNotificationRequestHandler _sut;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _teamsNotificatorFake = A.Fake<ITeamsNotificator>();
            _loggerFake = A.Fake<ILogger<TeamsNotificationRequestHandler>>();

            _sut = new TeamsNotificationRequestHandler(
                _teamsNotificatorFake,
                _loggerFake
            );
        }

        [Test]
        public async Task HandleAsync_ValidCommand_CallSendMessageAsyncWithCorrectArgs()
        {
            // Arrange
            var command = _fixture.Create<TeamsNotificationRequest>();


            // Act
            var result = await _sut.HandleAsync(command, CancellationToken.None);


            // Assert
            A.CallTo(() => _teamsNotificatorFake.SendMessageAsync(
                    command.WebhookUrl,
                    command.Message))
             .MustHaveHappenedOnceExactly();

            result.ShouldBe(command);
        }

        [Test]
        public async Task HandleAsync_NotificatorThrowsException_ThrowException()
        {
            // Arrange
            var command = _fixture.Create<TeamsNotificationRequest>();
            var expectedException = new HttpRequestException("Teams API unavailable");

            A.CallTo(() => _teamsNotificatorFake.SendMessageAsync(
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
