using FluentAssertions;
using Sharik.Domain.Exchanges;
using Sharik.Domain.Exchanges.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Domain.UnitTests.Exchanges
{
    public sealed class ExchangeTests
    {
        [Fact]
        public void CreateSwap_WhenSameSkillOfferedAndRequested_ReturnsFailure()
        {
            var requesterId = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var skillId = Guid.NewGuid();
            var message = "Hello";

            var result = Exchange.CreateSwap(requesterId , providerId , skillId , skillId , message);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == ExchangeErrors.CannotExchangeSameSkill.Code);
        }

        [Fact]
        public void CreateSwap_SkillOfferedIdIsEmpty_ReturnsFailure()
        {
            var requesterId = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var skillRequestedId = Guid.NewGuid();

            var result = Exchange.CreateSwap(requesterId , providerId , Guid.Empty , skillRequestedId , "Test");

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == ExchangeErrors.SkillOfferedIdRequired.Code);
        }

        [Fact]
        public void CreateSwap_RequesterIdIsEmpty_ReturnsFailure()
        {

            var providerId = Guid.NewGuid();
            var skillOfferedId = Guid.NewGuid();
            var skillRequestedId = Guid.NewGuid();


            var result = Exchange.CreateSwap(Guid.Empty , providerId , skillOfferedId , skillRequestedId , "Test");


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == ExchangeErrors.RequesterIdRequired.Code);
        }

        [Fact]
        public void CreateSwap_ProviderIdIsEmpty_ReturnsFailure()
        {

            var requesterId = Guid.NewGuid();
            var skillOfferedId = Guid.NewGuid();
            var skillRequestedId = Guid.NewGuid();


            var result = Exchange.CreateSwap(requesterId , Guid.Empty , skillOfferedId , skillRequestedId , "Test");


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == ExchangeErrors.ProviderIdRequired.Code);
        }

        [Fact]
        public void CreateSwap_SkillRequestedIdIsEmpty_ReturnsFailure()
        {

            var requesterId = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var skillOfferedId = Guid.NewGuid();


            var result = Exchange.CreateSwap(requesterId , providerId , skillOfferedId , Guid.Empty , "Test");


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == ExchangeErrors.SkillRequestedIdRequired.Code);
        }

        [Fact]
        public void CreateSwap_RequesterAndProviderAreSame_ReturnsFailure()
        {

            var userId = Guid.NewGuid();
            var skillOfferedId = Guid.NewGuid();
            var skillRequestedId = Guid.NewGuid();


            var result = Exchange.CreateSwap(userId , userId , skillOfferedId , skillRequestedId , "Test");


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == ExchangeErrors.CannotExchangeWithSelf.Code);
        }

        [Fact]
        public void CreateSwap_MessageTooLong_ReturnsFailure()
        {

            var requesterId = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var skillOfferedId = Guid.NewGuid();
            var skillRequestedId = Guid.NewGuid();
            var longMessage = new string('a' , 1001);


            var result = Exchange.CreateSwap(requesterId , providerId , skillOfferedId , skillRequestedId , longMessage);


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == ExchangeErrors.RequesterMessageTooLong.Code);
        }

        [Fact]
        public void CreateSwap_ValidArguments_ReturnsSuccess()
        {

            var requesterId = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var skillOfferedId = Guid.NewGuid();
            var skillRequestedId = Guid.NewGuid();
            var message = "Hello";


            var result = Exchange.CreateSwap(requesterId , providerId , skillOfferedId , skillRequestedId , message);


            result.IsSuccess.Should().BeTrue();
            result.Value.RequesterId.Should().Be(requesterId);
            result.Value.ProviderId.Should().Be(providerId);
            result.Value.Type.Should().Be(ExchangeType.Swap);
            result.Value.ExchangeStatus.Should().Be(ExchangeStatus.Pending);
        }

        [Fact]
        public void AcceptExchange_StatusIsPendingandProviderMatches_ReturnsSuccess()
        {

            var requesterId = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var skillOfferedId = Guid.NewGuid();
            var skillRequestedId = Guid.NewGuid();
            var exchange = Exchange.CreateSwap(requesterId , providerId , skillOfferedId , skillRequestedId , "Test").Value;


            var result = exchange.AcceptExchange(providerId);


            result.IsSuccess.Should().BeTrue();
            exchange.ExchangeStatus.Should().Be(ExchangeStatus.Accepted);
        }

        [Fact]
        public void AcceptExchange_ProviderIdDoesNotMatch_ReturnsFailure()
        {

            var requesterId = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var otherProviderId = Guid.NewGuid();
            var skillOfferedId = Guid.NewGuid();
            var skillRequestedId = Guid.NewGuid();
            var exchange = Exchange.CreateSwap(requesterId , providerId , skillOfferedId , skillRequestedId , "Test").Value;


            var result = exchange.AcceptExchange(otherProviderId);


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == ExchangeErrors.Unauthorized.Code);
        }

        [Fact]
        public void CreateTeaching_ValidArguments_ReturnsSuccess()
        {

            var requesterId = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var skillRequestedId = Guid.NewGuid();
            var duration = 2;
            var pointsValue = 50;
            var message = "Teach me";


            var result = Exchange.CreateTeaching(requesterId , providerId , skillRequestedId , duration , pointsValue , message);


            result.IsSuccess.Should().BeTrue();
            result.Value.Type.Should().Be(ExchangeType.Teaching);
            result.Value.Duration.Should().Be(duration);
            result.Value.PointsValue.Should().Be(pointsValue);
        }

        [Fact]
        public void CreateTeaching_InvalidDuration_ReturnsFailure()
        {

            var requesterId = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var skillRequestedId = Guid.NewGuid();


            var result = Exchange.CreateTeaching(requesterId , providerId , skillRequestedId , -1 , 50 , "Test");


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == ExchangeErrors.InvalidDuration.Code);
        }

        [Fact]
        public void CreateTeaching_PointsValueInvalid_ReturnsFailure()
        {

            var requesterId = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var skillRequestedId = Guid.NewGuid();


            var result = Exchange.CreateTeaching(requesterId , providerId , skillRequestedId , 2 , -1 , "Test");


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == ExchangeErrors.PointsValueInvalid.Code);
        }

        [Fact]
        public void CancelExchange_StatusIsPending_ReturnsSuccess()
        {

            var requesterId = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var skillRequestedId = Guid.NewGuid();
            var exchange = Exchange.CreateTeaching(requesterId , providerId , skillRequestedId , 1 , 10 , "Test").Value;

            // Mocking the requester to avoid NullReferenceException
            var requester = Sharik.Infrastructure.Auth.AppUser.Create("requester" , "test@test.com").Value;
            exchange.Requester = requester;


            var result = exchange.CancelExchange();


            result.IsSuccess.Should().BeTrue();
            exchange.ExchangeStatus.Should().Be(ExchangeStatus.Cancelled);
        }

        [Fact]
        public void RejectExchange_StatusIsPending_ReturnsSuccess()
        {

            var requesterId = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var skillRequestedId = Guid.NewGuid();
            var exchange = Exchange.CreateTeaching(requesterId , providerId , skillRequestedId , 1 , 10 , "Test").Value;

            var requester = Sharik.Infrastructure.Auth.AppUser.Create("requester" , "test@test.com").Value;
            exchange.Requester = requester;


            var result = exchange.RejectExchange(providerId);


            result.IsSuccess.Should().BeTrue();
            exchange.ExchangeStatus.Should().Be(ExchangeStatus.Rejected);
        }

        [Fact]
        public void CompleteExchange_StatusIsAccepted_ReturnsSuccess()
        {

            var requesterId = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var skillRequestedId = Guid.NewGuid();
            var exchange = Exchange.CreateTeaching(requesterId , providerId , skillRequestedId , 1 , 10 , "Test").Value;

            var provider = Sharik.Infrastructure.Auth.AppUser.Create("provider" , "test@test.com").Value;

            var userSkill = Sharik.Domain.Skills.UserSkills.UserSkill.Create(providerId , skillRequestedId , Sharik.Domain.Skills.UserSkills.Enums.SkillLevel.Beginner , 20).Value;
            var userSkillsField = typeof(Sharik.Infrastructure.Auth.AppUser).GetField("_userSkills" , System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var userSkillsList = (List<Sharik.Domain.Skills.UserSkills.UserSkill>)userSkillsField.GetValue(provider);
            userSkillsList.Add(userSkill);

            exchange.Provider = provider;
            exchange.AcceptExchange(providerId);


            var result = exchange.CompleteExchange();


            result.IsSuccess.Should().BeTrue();
            exchange.ExchangeStatus.Should().Be(ExchangeStatus.Completed);
        }

      
        [Fact]
        public void CalculateTotalPoints_EnoughPoints_ReturnsRequiredPoints()
        {

            var result = Exchange.CalculateTotalPoints(10 , 50 , 2);


            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(20);
        }

        [Fact]
        public void CalculateTotalPoints_NotEnoughPoints_ReturnsFailure()
        {

            var result = Exchange.CalculateTotalPoints(10 , 15 , 2);


            result.IsFailure.Should().BeTrue();
        }


    }
}
