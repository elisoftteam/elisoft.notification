using AutoFixture;
using Elisoft.Email.Services;
using Elisoft.Notificator.Configuration.Configuration;
using Elisoft.Notificator.Core.Handlers;
using Elisoft.Notificator.Core.Requests;
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
    public class EmailNotificationRequestHandlerTests
    {
        private Fixture _fixture;
        private IEmailNotificator _emailNotificatorFake;
        private IConfig _configFake;
        private ILogger<EmailNotificationRequestHandler> _loggerFake;
        private EmailNotificationRequestHandler _sut;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _emailNotificatorFake = A.Fake<IEmailNotificator>();
            _configFake = A.Fake<IConfig>();
            _loggerFake = A.Fake<ILogger<EmailNotificationRequestHandler>>();

            A.CallTo(() => _configFake.EmailSmtpHost).Returns(_fixture.Create<string>());
            A.CallTo(() => _configFake.EmailSmtpPort).Returns(587);
            A.CallTo(() => _configFake.EmailUseSsl).Returns(true);
            A.CallTo(() => _configFake.EmailUsername).Returns(_fixture.Create<string>());
            A.CallTo(() => _configFake.EmailPassword).Returns(_fixture.Create<string>());
            A.CallTo(() => _configFake.EmailFromAddress).Returns(_fixture.Create<string>());
            A.CallTo(() => _configFake.EmailFromName).Returns(_fixture.Create<string>());

            _sut = new EmailNotificationRequestHandler(
                _emailNotificatorFake,
                _configFake,
                _loggerFake
            );
        }

        [Test]
        public async Task HandleAsync_ValidCommand_CallSendMessageAsyncWithCorrectArgs()
        {
            // Arrange
            var command = _fixture.Create<EmailNotificationRequest>();


            // Act
            var result = await _sut.HandleAsync(command, CancellationToken.None);


            // Assert
            A.CallTo(() => _emailNotificatorFake.SendMessageAsync(
                    _configFake.EmailSmtpHost,
                    _configFake.EmailSmtpPort,
                    _configFake.EmailUseSsl,
                    _configFake.EmailUsername,
                    _configFake.EmailPassword,
                    _configFake.EmailFromAddress,
                    _configFake.EmailFromName,
                    command.To,
                    command.Subject,
                    command.Message,
                    command.IsBodyHtml))
                .MustHaveHappenedOnceExactly();

            result.ShouldBe(command);
        }

        [Test]
        public async Task HandleAsync_NotificatorThrowsException_ThrowException()
        {
            // Arrange
            var command = _fixture.Create<EmailNotificationRequest>();
            var expectedException = new HttpRequestException("SMTP unavailable");

            A.CallTo(() => _emailNotificatorFake.SendMessageAsync(
                    A<string>._,
                    A<int>._,
                    A<bool>._,
                    A<string>._,
                    A<string>._,
                    A<string>._,
                    A<string?>._,
                    A<string>._,
                    A<string>._,
                    A<string>._,
                    A<bool>._))
                .Throws(expectedException);


            // Act & Assert
            var exception = await Should.ThrowAsync<HttpRequestException>(async () =>
                await _sut.HandleAsync(command, CancellationToken.None));

            exception.Message.ShouldBe(expectedException.Message);
        }
    }
}
