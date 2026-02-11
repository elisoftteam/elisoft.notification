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
            var expectedMessage = _fixture.Create<string>();

            var jsonString = JsonSerializer.Serialize(new
            {
                WebhookUrl = expectedUrl,
                Message = expectedMessage
            });
            var jsonPayload = JsonSerializer.Deserialize<JsonElement>(jsonString);


            // Act
            var result = _sut.CreateRequest(channel, jsonPayload);


            // Assert
            var teamsRequest = result.ShouldBeOfType<TeamsNotificationRequest>();
            teamsRequest.WebhookUrl.ShouldBe(expectedUrl);
            teamsRequest.Message.ShouldBe(expectedMessage);
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
