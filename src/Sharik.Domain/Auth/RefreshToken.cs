using Sharik.Domain.Common;
using Sharik.Domain.Common.Results;

namespace Sharik.Domain.Auth
{
    public sealed class RefreshToken : AuditableEntity
    {
        public string? Token { get; }
        public string? UserId { get; }
        public DateTimeOffset ExpiresOnUtc { get; }

        private RefreshToken()
        { }

        private RefreshToken(Guid id, string? token, string? userId, DateTimeOffset expiresOnUtc)
            : base(id)
        {
            Token = token;
            UserId = userId;
            ExpiresOnUtc = expiresOnUtc;
        }

        public static Result<RefreshToken> Create( string? token, string? userId, DateTimeOffset expiresOnUtc)
        {
            var validation = Validate(token, userId, expiresOnUtc);

            if (validation.IsFailure)
                return validation.Errors;

            return new RefreshToken(Guid.NewGuid(), token, userId, expiresOnUtc);
        }

        private static Result<Success> Validate(string? token, string? userId, DateTimeOffset expiresOnUtc)
        {
            if (string.IsNullOrWhiteSpace(token))
                return RefreshTokenErrors.TokenRequired;
            
            if (string.IsNullOrWhiteSpace(userId))
                return RefreshTokenErrors.UserIdRequired;
            
            if (expiresOnUtc <= DateTimeOffset.UtcNow)
                return RefreshTokenErrors.ExpiryInvalid;
            
            return Result.Success;
        }
    }
}
