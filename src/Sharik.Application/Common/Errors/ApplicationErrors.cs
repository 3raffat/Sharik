using Sharik.Domain.Common.Results;

namespace Sharik.Application.Common.Errors
{
    public static class ApplicationErrors
    {

        public static Error SkillCategoryNotFound => Error.NotFound(
            code: "SkillCategory.NotFound",
            description: "The specified skill category was not found.");

        public static Error SkillAlreadyExists => Error.Conflict(
            code: "Skill.AlreadyExists",
            description: "A skill with the specified name already exists.");

        public static Error SkillNotFound => Error.NotFound(
            code: "Skill.NotFound",
            description: "The specified skill was not found.");

        public static Error SkillCategoryAlreadyExists => Error.Conflict(
            code: "SkillCategory.AlreadyExists",
            description: "A Category with the specified name already exists.");

        public static Error UserNotFound => Error.NotFound(
             code: "User.NotFound",
             description: "The user with the specified ID was not found.");

        public static Error SkillAlreadyExistsForUser => Error.Conflict(
             code: "UserSkill.AlreadyExists",
             description: "This skill is already assigned to the user.");

        public static Error UserSkillNotFound => Error.NotFound(
            code: "UserSkill.NotFound",
            description: "The Skill with the specified ID was not found.");

        public static Error ProfileIncomplete => Error.Conflict(
            code: "AppUser.Profile.Incomplete",
            description: "Profile is still incomplete."
       );

        public static Error ExchangeNotFound => Error.NotFound(
            code: "Exchange.NotFound",
            description: "The exchange with the specified ID was not found.");

        public static Error ProviderNotFound => Error.NotFound(
             code: "User.NotFound" ,
             description: "The provider with the specified ID does not exist");

        public static Error ExchangeAlreadyExists => Error.Conflict(
                code: "Exchange.AlreadyExists" ,
                description: "An active exchange already exists between the requester and provider for the same skills");

        public static Error Unauthorized => Error.Unauthorized(
            code: "Unauthorized" ,
            description: "You are not authorized to perform this action");


        public static Error ExpiredAccessTokenInvalid = Error.Conflict(
            "Auth.ExpiredAccessToken.Invalid" , "Expired access token is not valid.");

        public static Error UserIdClaimInvalid = Error.Conflict(
             "Auth.UserIdClaim.Invalid" , "Invalid userId claim.");

        public static Error RefreshTokenExpired = Error.Conflict(
             "Auth.RefreshToken.Expired" , "Refresh token is invalid or has expired.");

        public static readonly Error TokenGenerationFailed = Error.Failure(
             "Auth.TokenGeneration.Failed" , "Failed to generate new JWT token.");
    }
}
