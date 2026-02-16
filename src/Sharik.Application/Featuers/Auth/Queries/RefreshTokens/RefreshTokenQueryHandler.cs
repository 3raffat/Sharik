using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Auth.Dtos;
using Sharik.Domain.Common.Results;
using System.Security.Claims;

namespace Sharik.Application.Featuers.Auth.Queries.RefreshTokens
{
    public sealed class RefreshTokenQueryHandler(IAppDbContext _context ,
        ITokenProvider _provider , IUserService _userService ,
        ILogger<RefreshTokenQueryHandler> _logger)
        : IRequestHandler<RefreshTokenQuery , Result<TokenResponse>>
    {
        public async Task<Result<TokenResponse>> Handle(RefreshTokenQuery request , CancellationToken ct)
        {
            var principal = _provider.GetPrincipalFromExpiredToken(request.ExpiredAccessToken);

            if (principal is null)
            {
                _logger.LogError("Expired access token is not valid");

                return ApplicationErrors.ExpiredAccessTokenInvalid;
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
            {
                _logger.LogError("Invalid userId claim");

                return ApplicationErrors.UserIdClaimInvalid;
            }

            var getUserResult = await _userService.GetUserByIdAsync(userId);

            if (getUserResult.IsFailure)
            {
                _logger.LogError("Get user by id error occurred: {ErrorDescription}" , getUserResult.TopError.Description);
                return getUserResult.Errors;
            }

            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == request.RefreshToken && r.UserId == userId , ct);

            if (refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
            {
                _logger.LogError("Refresh token has expired");

                return ApplicationErrors.RefreshTokenExpired;
            }

            var generateTokenResult = await _provider.GenerateJwtTokenAsync(getUserResult.Value , ct);

            if (generateTokenResult.IsFailure)
            {
                _logger.LogError("Generate token error occurred: {ErrorDescription}" , generateTokenResult.TopError.Description);

                return generateTokenResult.Errors;
            }
            return generateTokenResult.Value;
        }
    }
}
