using AutoFixture;
using Elisoft.Notificator.Core.Enums;
using Elisoft.Notificator.Core.Factories;
using Elisoft.Notificator.Core.Requests;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Shouldly;
using System;
using System.Text.Json;

namespace Elisoft.Notificator.Tests.Core
{
    [TestFixture]
    public class RequestFactoryTests
    {
        private Fixture _fixture;
        private ILogger<RequestFactory> _loggerFake;
        private RequestFactory _sut;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _loggerFake = A.Fake<ILogger<RequestFactory>>();
            _sut = new RequestFactory(_loggerFake);
        }

        [Test]
        public void CreateRequest_ChannelIsNotSupported_ThrowInvalidOperationException()
        {
            // Arrange
            var unknownChannel = (NotificationEnumChannel)999;
            var jsonPayload = JsonSerializer.Deserialize<JsonElement>("{}");


            // Act & Assert
            Should.Throw<InvalidOperationException>(() =>
                _sut.CreateRequest(unknownChannel, jsonPayload))
                .Message.ShouldContain("is not supported");
        }

        [Test]
        public void CreateRequest_PayloadIsInvalidJson_ThrowArgumentException()
        {
            // Arrange
            var channel = NotificationEnumChannel.Slack;
            var invalidStructurePayload = JsonSerializer.Deserialize<JsonElement>("[]");


            // Act & Assert
            Should.Throw<ArgumentException>(() =>
                _sut.CreateRequest(channel, invalidStructurePayload))
                .Message.ShouldContain("Invalid payload structure");
        }

        [Test]
        public void CreateRequest_ValidSlackPayload_ReturnSlackNotificationRequestObject()
        {
            // Arrange
            var channel = NotificationEnumChannel.Slack;
            var expectedUrl = _fixture.Create<string>();
            var expectedChannelName = _fixture.Create<string>();
            var expectedMessage = _fixture.Create<string>();

            var jsonString = JsonSerializer.Serialize(new
            {
                WebhookUrl = expectedUrl,
                ChannelName = expectedChannelName,
                Message = expectedMessage
            });
            var jsonPayload = JsonSerializer.Deserialize<JsonElement>(jsonString);


            // Act
            var result = _sut.CreateRequest(channel, jsonPayload);


            // Assert
            var slackRequest = result.ShouldBeOfType<SlackNotificationRequest>();
            slackRequest.WebhookUrl.ShouldBe(expectedUrl);
            slackRequest.Message.ShouldBe(expectedMessage);
        }

        [Test]
        public void CreateRequest_ValidTeamsPayload_ReturnTeamsNotificationRequestObject()
        {
            // Arrange
            var channel = NotificationEnumChannel.Teams;
            var expectedUrl = _fixture.Create<string>();
            var expectedTitle = _fixture.Create<string>();
            var expectedMessage = _fixture.Create<string>();

            var jsonString = JsonSerializer.Serialize(new
            {
                WebhookUrl = expectedUrl,
                Title = expectedTitle,
                Message = expectedMessage
            });
            var jsonPayload = JsonSerializer.Deserialize<JsonElement>(jsonString);


            // Act
            var result = _sut.CreateRequest(channel, jsonPayload);


            // Assert
            var teamsRequest = result.ShouldBeOfType<TeamsNotificationRequest>();
            teamsRequest.WebhookUrl.ShouldBe(expectedUrl);
            teamsRequest.Title.ShouldBe(expectedTitle);
            teamsRequest.Message.ShouldBe(expectedMessage);
        }

        [Test]
        public void CreateRequest_ValidPushoverPayload_ReturnPushoverNotificationRequestObject()
        {
            // Arrange
            var channel = NotificationEnumChannel.Pushover;
            var expectedMessage = _fixture.Create<string>();
            var expectedTitle = _fixture.Create<string>();
            var expectedPriority = _fixture.Create<int>();

            var jsonString = JsonSerializer.Serialize(new
            {
                Message = expectedMessage,
                Title = expectedTitle,
                Priority = expectedPriority
            });
            var jsonPayload = JsonSerializer.Deserialize<JsonElement>(jsonString);


            // Act
            var result = _sut.CreateRequest(channel, jsonPayload);


            // Assert
            var pushoverRequest = result.ShouldBeOfType<PushoverNotificationRequest>();
            pushoverRequest.Message.ShouldBe(expectedMessage);
            pushoverRequest.Title.ShouldBe(expectedTitle);
            pushoverRequest.Priority.ShouldBe(expectedPriority);
        }

        [Test]
        public void CreateRequest_ValidTwilioPayload_ReturnTwilioNotificationRequestObject()
        {
            // Arrange
            var channel = NotificationEnumChannel.Twilio;
            var expectedTo = _fixture.Create<string>();
            var expectedMessage = _fixture.Create<string>();

            var jsonString = JsonSerializer.Serialize(new
            {
                To = expectedTo,
                Message = expectedMessage
            });
            var jsonPayload = JsonSerializer.Deserialize<JsonElement>(jsonString);


            // Act
            var result = _sut.CreateRequest(channel, jsonPayload);


            // Assert
            var twilioRequest = result.ShouldBeOfType<TwilioNotificationRequest>();
            twilioRequest.To.ShouldBe(expectedTo);
            twilioRequest.Message.ShouldBe(expectedMessage);
        }

        [Test]
        public void CreateRequest_ValidEmailPayload_ReturnEmailNotificationRequestObject()
        {
            // Arrange
            var channel = NotificationEnumChannel.Email;
            var expectedTo = _fixture.Create<string>();
            var expectedSubject = _fixture.Create<string>();
            var expectedMessage = _fixture.Create<string>();

            var jsonString = JsonSerializer.Serialize(new
            {
                To = expectedTo,
                Subject = expectedSubject,
                Message = expectedMessage,
                IsBodyHtml = true
            });
            var jsonPayload = JsonSerializer.Deserialize<JsonElement>(jsonString);


            // Act
            var result = _sut.CreateRequest(channel, jsonPayload);


            // Assert
            var emailRequest = result.ShouldBeOfType<EmailNotificationRequest>();
            emailRequest.To.ShouldBe(expectedTo);
            emailRequest.Subject.ShouldBe(expectedSubject);
            emailRequest.Message.ShouldBe(expectedMessage);
            emailRequest.IsBodyHtml.ShouldBeTrue();
        }

        [Test]
        public void CreateRequest_PayloadIsNull_ThrowArgumentException()
        {
            // Arrange
            var channel = NotificationEnumChannel.Slack;
            var nullJsonPayload = JsonSerializer.Deserialize<JsonElement>("null");


            // Act & Assert
            Should.Throw<ArgumentException>(() =>
                _sut.CreateRequest(channel, nullJsonPayload))
                .Message.ShouldContain("Payload is null");
        }
    }
}
