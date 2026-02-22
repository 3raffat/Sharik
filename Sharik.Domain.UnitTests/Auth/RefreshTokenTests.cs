using FluentAssertions;
using Sharik.Domain.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Domain.UnitTests.Auth
{
    public sealed class RefreshTokenTests
    {
        [Fact]
        public void Create_ValidArguments_ReturnsSuccess()
        {

            var token = "abc-123-token";
            var userId = Guid.NewGuid().ToString();
            var expiry = DateTimeOffset.UtcNow.AddDays(7);


            var result = RefreshToken.Create(token , userId , expiry);


            result.IsSuccess.Should().BeTrue();
            result.Value.Token.Should().Be(token);
            result.Value.UserId.Should().Be(userId);
            result.Value.ExpiresOnUtc.Should().Be(expiry);
        }

        [Fact]
        public void Create_TokenIsEmpty_ReturnsFailure()
        {

            var token = "";
            var userId = "user-123";
            var expiry = DateTimeOffset.UtcNow.AddDays(1);


            var result = RefreshToken.Create(token , userId , expiry);


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == RefreshTokenErrors.TokenRequired.Code);
        }

        [Fact]
        public void Create_TokenIsWhitespace_ReturnsFailure()
        {

            var token = "   ";
            var userId = "user-123";
            var expiry = DateTimeOffset.UtcNow.AddDays(1);


            var result = RefreshToken.Create(token , userId , expiry);


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == RefreshTokenErrors.TokenRequired.Code);
        }

        [Fact]
        public void Create_UserIdIsEmpty_ReturnsFailure()
        {

            var token = "token";
            var userId = "";
            var expiry = DateTimeOffset.UtcNow.AddDays(1);


            var result = RefreshToken.Create(token , userId , expiry);


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == RefreshTokenErrors.UserIdRequired.Code);
        }

        [Fact]
        public void Create_UserIdIsWhitespace_ReturnsFailure()
        {

            var token = "token";
            var userId = "   ";
            var expiry = DateTimeOffset.UtcNow.AddDays(1);


            var result = RefreshToken.Create(token , userId , expiry);


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == RefreshTokenErrors.UserIdRequired.Code);
        }

        [Fact]
        public void Create_ExpiryIsInPast_ReturnsFailure()
        {

            var token = "token";
            var userId = "user-123";
            var expiry = DateTimeOffset.UtcNow.AddDays(-1);


            var result = RefreshToken.Create(token , userId , expiry);


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == RefreshTokenErrors.ExpiryInvalid.Code);
        }

        [Fact]
        public void Create_ExpiryIsNow_ReturnsFailure()
        {

            var token = "token";
            var userId = "user-123";
            var expiry = DateTimeOffset.UtcNow;


            var result = RefreshToken.Create(token , userId , expiry);


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == RefreshTokenErrors.ExpiryInvalid.Code);
        }
    }
}
