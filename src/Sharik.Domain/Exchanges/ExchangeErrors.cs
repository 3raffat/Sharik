using Sharik.Domain.Common.Results;

namespace Sharik.Domain.Exchanges
{
    public static class ExchangeErrors
    {
        public static Error RequesterIdRequired => Error.Validation(
            code: "Exchange.RequesterId.Required" ,
            description: "Requester ID cannot be empty."
        );

        public static Error ProviderIdRequired => Error.Validation(
            code: "Exchange.ProviderId.Required" ,
            description: "Provider ID cannot be empty."
        );

        public static Error SkillOfferedIdRequired => Error.Validation(
            code: "Exchange.SkillOfferedId.Required" ,
            description: "Skill offered ID cannot be empty."
        );

        public static Error SkillRequestedIdRequired => Error.Validation(
            code: "Exchange.SkillRequestedId.Required" ,
            description: "Skill requested ID cannot be empty."
        );

        public static Error InvalidDuration => Error.Validation(
            code: "Exchange.Duration.Invalid" ,
            description: "Duration must be a positive integer."
        );

        public static Error InvalidPointsValue => Error.Validation(
            code: "Exchange.PointsValue.Invalid" ,
            description: "Points value must be a non-negative integer."
        );

        public static Error InvalidExchangeType => Error.Validation(
            code: "Exchange.Type.Invalid" ,
            description: "Exchange type is invalid."
        );

        public static Error DurationRequiredForPoints => Error.Validation(
            code: "Exchange.Duration.RequiredForPoints" ,
            description: "Duration is required for Points exchanges."
        );

        public static Error PointsValueRequiredForPoints => Error.Validation(
            code: "Exchange.PointsValue.RequiredForPoints" ,
            description: "Points value is required for points exchanges."
        );

        public static Error CannotRateOwnExchange => Error.Validation(
            code: "Exchange.Rating.CannotRateOwnExchange" ,
            description: "Users cannot rate their own exchanges."
        );

        public static Error ExchangeNotFound => Error.NotFound(
            code: "Exchange.NotFound" ,
            description: "The specified exchange was not found."
        );

        public static Error PointsValueRequiredForPointsExchange => Error.Validation(
            code: "Exchange.PointsValue.RequiredForPointsExchange" ,
            description: "Points value is required for points exchanges."
        );

        public static Error RequesterMessageTooLong => Error.Validation(
            code: "Exchange.RequesterMessage.TooLong" ,
            description: "The requester message exceeds the maximum allowed length."
        );

        public static Error ExchangeAlreadyCompleted => Error.Conflict(
            code: "Exchange.AlreadyCompleted" ,
            description: "The exchange has already been completed."
        );

        public static Error CanOnlyApprovePendingExchanges => Error.Conflict(
            code: "Exchange.CanOnlyApprovePending" ,
            description: "Only exchanges with a pending status can be approved.");


        public static Error ExchangeAlreadyCancelled => Error.Conflict(
            code: "Exchange.AlreadyCancelled" ,
            description: "The exchange has already been cancelled."
        );

        public static Error InvalidExchangeStatusTransition => Error.Validation(
            code: "Exchange.InvalidStatusTransition" ,
            description: "The requested status transition is not allowed."
        );

        public static Error Unauthorized => Error.Unauthorized(
          code: "Unauthorized" ,
          description: "You are not authorized to perform this action");

        public static Error CannotExchangeWithSelf => Error.Validation(
            code: "Exchange.CannotExchangeWithSelf" ,
            description: "Users cannot create exchanges with themselves.");

        public static Error CannotExchangeSameSkill => Error.Validation(
            code: "Exchange.CannotExchangeSameSkill" ,
            description: "Users cannot exchange the same skill.");
    }
}
