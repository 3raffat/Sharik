using Sharik.Domain.Common.Results;

namespace Sharik.Domain.Auth
{
    public static class RefreshTokenErrors
    {
        public static Error IdRequired =
            Error.Validation("RefreshToken_Id_Required", "Refresh token ID is required.");

        public static Error TokenRequired =
            Error.Validation("RefreshToken_Token_Required", "Token value is required.");

        public static Error UserIdRequired =
            Error.Validation("RefreshToken_UserId_Required", "User ID is required.");

        public static Error ExpiryInvalid =
            Error.Validation("RefreshToken_Expiry_Invalid", "Expiry must be in the future.");
    }
}
