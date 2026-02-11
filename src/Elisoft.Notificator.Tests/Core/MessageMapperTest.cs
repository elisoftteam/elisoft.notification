using AutoFixture;
using Elisoft.Notificator.Core.Enums;
using Elisoft.Notificator.Core.Factories;
using Elisoft.Notificator.Core.Models;
using NUnit.Framework;
using Shouldly;
using System;
using System.Text.Json;
using Elisoft.Notificator.Api.Mappers;
using Elisoft.Notificator.Api.Contracts;

namespace Elisoft.Notificator.Tests.Core
{
    [TestFixture]
    public class MessageModelFactoryTests
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void MapToNotification_MessageIsNull_ThrowArgumentException()
        {
            // Arrange
            var sut = new MessageMapper();


            // Act & Assert
            Should.Throw<ArgumentException>(() =>
                sut.MapToNotification(null))
                .Message.ShouldContain("Body cannot be empty");
        }

        [Test]
        public void MapToNotification_ChannelIsEmpty_ThrowArgumentException()
        {
            // Arrange
            var model = _fixture.Build<Message>()
                .With(x => x.Channel, string.Empty)
                .Without(x => x.Payload)
                .Create();

            var sut = new MessageMapper();


            // Act & Assert
            Should.Throw<ArgumentException>(() =>
                sut.MapToNotification(model))
                .Message.ShouldContain("Missing 'channel' property");
        }

        [Test]
        public void MapToNotification_ChannelIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var invalidChannelName = _fixture.Create<string>();
            var model = _fixture.Build<Message>()
                .With(x => x.Channel, invalidChannelName)
                .Without(x => x.Payload)
                .Create();

            var sut = new MessageMapper();


            // Act & Assert
            Should.Throw<ArgumentException>(() =>
                sut.MapToNotification(model))
                .Message.ShouldContain("Invalid channel value");
        }

        [Test]
        public void MapToNotification_PayloadIsUndefined_ThrowArgumentException()
        {
            // Arrange
            var model = new Message
            {
                Channel = NotificationEnumChannel.Slack.ToString(),
                Payload = new JsonElement()
            };

            var sut = new MessageMapper();


            // Act & Assert
            Should.Throw<ArgumentException>(() =>
                sut.MapToNotification(model))
                .Message.ShouldContain("Missing 'payload' property");
        }

        [Test]
        public void MapToNotification_ValidData_ReturnNotificationWithCorrectChannel()
        {
            // Arrange
            var validPayload = JsonSerializer.Deserialize<JsonElement>("{}");
            var model = new Message
            {
                Channel = NotificationEnumChannel.Slack.ToString(),
                Payload = validPayload
            };
            var sut = new MessageMapper();


            // Act
            var result = sut.MapToNotification(model);


            // Assert
            result.Channel.ShouldBe(NotificationEnumChannel.Slack);
        }

        [Test]
        public void MapToNotification_ValidData_ReturnNotificationWithCorrectPayload()
        {
            // Arrange
            var validPayload = JsonSerializer.Deserialize<JsonElement>("{\"key\":\"value\"}");
            var model = new Message
            {
                Channel = NotificationEnumChannel.Slack.ToString(),
                Payload = validPayload
            };
            var sut = new MessageMapper();


            // Act
            var result = sut.MapToNotification(model);


            // Assert
            result.Payload.ToString().ShouldBe(validPayload.ToString());
        }

        [Test]
        public void MapToNotification_CaseInsensitiveChannel_ReturnCorrectEnum()
        {
            // Arrange
            var validPayload = JsonSerializer.Deserialize<JsonElement>("{}");
            var model = new Message
            {
                Channel = "slack",
                Payload = validPayload
            };
            var sut = new MessageMapper();


            // Act
            var result = sut.MapToNotification(model);


            // Assert
            result.Channel.ShouldBe(NotificationEnumChannel.Slack);
        }

        [Test]
        public void MapToNotification_ValidTeamsChannel_ReturnCorrectEnum()
        {
            // Arrange
            var validPayload = JsonSerializer.Deserialize<JsonElement>("{}");
            var model = new Message
            {
                Channel = "Teams",
                Payload = validPayload
            };
            var sut = new MessageMapper();


            // Act
            var result = sut.MapToNotification(model);


            // Assert
            result.Channel.ShouldBe(NotificationEnumChannel.Teams);
        }
    }
}
