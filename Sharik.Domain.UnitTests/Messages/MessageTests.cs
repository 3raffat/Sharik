using FluentAssertions;
using Sharik.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Domain.UnitTests.Messages
{
    public sealed class MessageTests
    {
        [Fact]
        public void Create_WithValidData_ShouldSuccess()
        {
            var exchangeId = Guid.NewGuid();
            var senderId = Guid.NewGuid();
            var content = "Hello, this is a message.";

            var result = Message.Create(exchangeId , senderId , content);

            result.IsSuccess.Should().BeTrue();
            result.Value.ExchangeId.Should().Be(exchangeId);
            result.Value.SenderId.Should().Be(senderId);
            result.Value.Content.Should().Be(content);
            result.Value.SentAt.Should().BeBefore(DateTime.UtcNow.AddSeconds(1));
        }

        [Fact]
        public void Create_WhenExchangeIdIsEmpty_ShouldFail()
        {
            var senderId = Guid.NewGuid();
            var content = "Hello";

            var result = Message.Create(Guid.Empty , senderId , content);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == MessageErrors.ExchangeIdRequired.Code);
        }

        [Fact]
        public void Create_WhenSenderIdIsEmpty_ShouldFail()
        {
            var exchangeId = Guid.NewGuid();
            var content = "Hello";
            
            var result = Message.Create(exchangeId , Guid.Empty , content);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == MessageErrors.SenderIdRequired.Code);
        }

        [Fact]
        public void Create_WhenContentIsWhitespace_ShouldFail()
        {
            var exchangeId = Guid.NewGuid();
            var senderId = Guid.NewGuid();
            var content = "  ";

            var result = Message.Create(exchangeId , senderId , content);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == MessageErrors.MassageRequired.Code);
        }
    }
}
