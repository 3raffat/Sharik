using FluentAssertions;
using Sharik.Domain.Notifications;
using Sharik.Domain.Notifications.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Domain.UnitTests.Notifications
{
    public sealed class NotificationTests
    {
        [Fact]
        public void Create_WithValidDate_ShouldSuccess()
        {
            var userId = Guid.NewGuid();
            var type = NotificationType.NewExchangeRequest;
            var message = "You have a new exchange request.";

            var result = Notification.Create(userId , type , message);

            result.IsSuccess.Should().BeTrue();
            result.Value.UserId.Should().Be(userId);
            result.Value.Type.Should().Be(type);
            result.Value.Message.Should().Be(message);
            result.Value.IsRead.Should().BeFalse();
        }

        [Fact]
        public void Create_WhenUserIdIsEmpty_ShouldFail()
        {
            var result = Notification.Create(Guid.Empty , NotificationType.NewExchangeRequest , "test");

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == NotificationErrors.UserIdRequired.Code);
        }

        [Fact]
        public void Create_WhenInvalidType_ShouldFail()
        {
            var result = Notification.Create(Guid.NewGuid() , (NotificationType)999 , "test");

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == NotificationErrors.InvalidNotificationType.Code);
        }

        [Fact]
        public void Create_WhenMessageIsWhitespace_ShouldFail()
        {
            var result = Notification.Create(Guid.NewGuid() , NotificationType.NewExchangeRequest , "   ");

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == NotificationErrors.MassageRequired.Code);
        }

        [Fact]
        public void MarkAsRead_Called_IsReadSetToTrue()
        {
            var notification = Notification.Create(Guid.NewGuid() , NotificationType.NewExchangeRequest , "test").Value;

            notification.MarkAsRead();

            notification.IsRead.Should().BeTrue();
        }
    }
}
