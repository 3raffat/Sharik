using FluentAssertions;
using Sharik.Domain.User;
using Sharik.Domain.User.Enums;
using Sharik.Infrastructure.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Domain.UnitTests.User
{
    public sealed class AppUserTests
    {
        [Fact]
        public void Create_ValidArguments_ReturnsSuccess()
        {

            var username = "testuser";
            var email = "test@example.com";


            var result = AppUser.Create(username , email);


            result.IsSuccess.Should().BeTrue();
            result.Value.UserName.Should().Be(username);
            result.Value.Email.Should().Be(email);
        }

        [Fact]
        public void Create_UserNameIsEmpty_ReturnsFailure()
        {

            var result = AppUser.Create("" , "test@test.com");


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == AppUserErrors.UserNameRequired.Code);
        }

        [Fact]
        public void Create_EmailIsInvalid_ReturnsFailure()
        {

            var result = AppUser.Create("user" , "invalid-email");


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == AppUserErrors.EmailInvalid.Code);
        }

        [Fact]
        public void AddPoints_ValidAmount_IncreasesTotalPoints()
        {

            var user = AppUser.Create("user" , "user@test.com").Value;
            var initialPoints = user.TotalPointsEarned;
            var pointsToAdd = 10;


            user.AddPoints(pointsToAdd);


            user.TotalPointsEarned.Should().Be(initialPoints + pointsToAdd);
        }

        [Fact]
        public void DeductPoints_UserHasEnoughPoints_DecreasesTotalPoints()
        {

            var user = AppUser.Create("user" , "user@test.com").Value;
            user.AddPoints(100);
            var pointsToDeduct = 40;


            var result = user.DeductPoints(pointsToDeduct);


            result.IsSuccess.Should().BeTrue();
            user.TotalPointsEarned.Should().Be(60);
        }

        [Fact]
        public void DeductPoints_InsufficientPoints_ReturnsFailure()
        {

            var user = AppUser.Create("user" , "user@test.com").Value;
            user.AddPoints(10);
            var pointsToDeduct = 40;


            var result = user.DeductPoints(pointsToDeduct);


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == AppUserErrors.InsufficientPoints.Code);
        }

        [Fact]
        public void CompleteProfile_ValidData_ProfileUpdatedAndBonusPointsAdded()
        {

            var user = AppUser.Create("user" , "user@test.com").Value;
            var firstName = "John";
            var lastName = "Doe";
            var bio = new string('a' , 60);


            var result = user.CompleteProfile(firstName , lastName , bio);


            result.IsSuccess.Should().BeTrue();
            user.FirstName.Should().Be(firstName);
            user.LastName.Should().Be(lastName);
            user.Bio.Should().Be(bio);
            user.ProfileStatus.Should().Be(ProfileStatus.Complete);
            user.TotalPointsEarned.Should().Be(50);
        }

        [Fact]
        public void CompleteProfile_FirstNameTooShort_ReturnsFailure()
        {

            var user = AppUser.Create("user" , "user@test.com").Value;


            var result = user.CompleteProfile("Jo" , "Doe" , new string('a' , 60));


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == AppUserErrors.FirstNameTooShort.Code);
        }

        [Fact]
        public void UpdateProfile_ProfileIsComplete_ReturnsSuccess()
        {

            var user = AppUser.Create("user" , "user@test.com").Value;
            user.CompleteProfile("John" , "John" , new string('a' , 60));


            var result = user.UpdateProfile("Johnny" , "Doe" , new string('b' , 60));


            result.IsSuccess.Should().BeTrue();
            user.FirstName.Should().Be("Johnny");
        }

        [Fact]
        public void UpdateProfile_ProfileIsIncomplete_ReturnsFailure()
        {

            var user = AppUser.Create("user" , "user@test.com").Value;


            var result = user.UpdateProfile("John" , "Doe" , new string('a' , 60));


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == AppUserErrors.ProfileIncomplete.Code);
        }

        [Fact]
        public void AddPoints_InvalidPoints_ReturnsFailure()
        {

            var user = AppUser.Create("user" , "user@test.com").Value;


            var result = user.AddPoints(0);


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == AppUserErrors.InvalidPoints.Code);
        }
    }
}
