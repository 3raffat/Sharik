using FluentAssertions;
using Sharik.Domain.Ratings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Domain.UnitTests.Ratings
{
    public sealed class RatingTests
    {

        [Fact]
        public void Create_WithValidData_ShouldSuccess()
        {
            var exchangeId = Guid.NewGuid();
            var raterId = Guid.NewGuid();
            var ratedUserId = Guid.NewGuid();
            var score = 5;
            var comment = "Excellent!";

            var result = Rating.Create(exchangeId , raterId , ratedUserId , score , comment);

            result.IsSuccess.Should().BeTrue();
            result.Value.ExchangeId.Should().Be(exchangeId);
            result.Value.Score.Should().Be(score);
        }

        [Fact]
        public void Create_WhneScoreLessThanOne_ShouldFail()
        {
            var result = Rating.Create(Guid.NewGuid() , Guid.NewGuid() , Guid.NewGuid() , 0 , "test");

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == RatingErrors.ScoreOutOfRange.Code);
        }

        [Fact]
        public void Create_WhenExchangeIdIsEmpty_ShouldFail()
        {
            var result = Rating.Create(Guid.Empty , Guid.NewGuid() , Guid.NewGuid() , 5 , "test");

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == RatingErrors.ExchangeIdRequired.Code);
        }

        [Fact]
        public void Create_WhenRaterIdIsEmpty_ShouldFail()
        {
            var result = Rating.Create(Guid.NewGuid() , Guid.Empty , Guid.NewGuid() , 5 , "test");

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == RatingErrors.RaterIdRequired.Code);
        }

        [Fact]
        public void Create_WhenRatedUserIdIsEmpty_Shouldailure()
        {
            var result = Rating.Create(Guid.NewGuid() , Guid.NewGuid() , Guid.Empty , 5 , "test");

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == RatingErrors.RatedUserIdRequired.Code);
        }

        [Fact]
        public void Create_WhenRatingSelf_ShouldFail()
        {
            var userId = Guid.NewGuid();

            var result = Rating.Create(Guid.NewGuid() , userId , userId , 5 , "test");

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == RatingErrors.CannotRateSelf.Code);
        }

        [Fact]
        public void Create_WhenCommentTooLong_ShouldFail()
        {
            var longComment = new string('a' , 501);

            var result = Rating.Create(Guid.NewGuid() , Guid.NewGuid() , Guid.NewGuid() , 5 , longComment);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == RatingErrors.CommentTooLong.Code);
        }

        [Fact]
        public void Update_WithValidData_ShouldSuccess()
        {
            var rating = Rating.Create(Guid.NewGuid() , Guid.NewGuid() , Guid.NewGuid() , 3 , "OK").Value;
            var newScore = 4;
            var newComment = "Better than OK";

            var result = rating.Update(newScore , newComment);

            result.IsSuccess.Should().BeTrue();
            rating.Score.Should().Be(newScore);
            rating.Comment.Should().Be(newComment);
        }
    }
}
