using Sharik.Domain.Common.Results;
using Sharik.Domain.Ratings.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Domain.Ratings
{
    public static class RatingErrors
    {

        public static Error ExchangeIdRequired => Error.Validation(
            code: "Exchange.Id.Required"
            , description: "Exchange ID cannot be empty.");

        public static Error RaterIdRequired => Error.Validation( 
            code: "Rater.Id.Required"
            , description: "Rater ID cannot be empty.");

        public static Error RatedUserIdRequired => Error.Validation(
            code: "RatedUser.Id.Required"
            , description: "Rated user ID cannot be empty.");

        public static Error ScoreOutOfRange => Error.Validation(
            code: "Rating.Score.OutOfRange"
            , description: "Rating score must be between 1 and 5.");

        public static Error InvalidRatingType => Error.Validation(
            code: "Rating.Type.Invalid"
            , description: "Invalid rating type.");

        public static Error CommentTooLong => Error.Validation(
            code: "Rating.Comment.TooLong"
            , description: "Rating comment exceeds maximum length of 500 characters.");


        public static Error CannotRateSelf => Error.Validation(
            code: "Rating.CannotRateSelf"
            , description: "A user cannot rate themselves.");
    }
}
