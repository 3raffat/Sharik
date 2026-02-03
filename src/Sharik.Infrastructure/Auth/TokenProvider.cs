using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Auth.Dtos;
using Sharik.Domain.Auth;
using Sharik.Domain.Common.Results;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Sharik.Infrastructure.Auth
{
    public sealed class TokenProvider(IConfiguration _configuration, IAppDbContext _context) : ITokenProvider
    {
        public async Task<Result<TokenResponse>> GenerateJwtTokenAsync(AppUserDto user, CancellationToken ct = default)
        {
            var tokenResult = await GenerateJwtToken(user, ct);

            return tokenResult.Value;
        }

        private async Task<Result<TokenResponse>> GenerateJwtToken(AppUserDto user, CancellationToken ct = default)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var Aduience = jwtSettings["Audience"];
            var Issuer = jwtSettings["Issuer"];
            var Secret = jwtSettings["Secret"];
            var Expire = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["TokenExpirationInMinutes"]!));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));

            var claims = new List<Claim> {
              new Claim(JwtRegisteredClaimNames.Sub ,user.UserId),
              new Claim(JwtRegisteredClaimNames.Email,user.UserEmail),
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = Aduience,
                Issuer = Issuer,
                Expires = Expire,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(descriptor);

            var oldRefreshTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == user.UserId)
                .ExecuteDeleteAsync(ct);

            var refreshTokenResult = RefreshToken.Create(GenerateRefreshToken(),
                                                         user.UserId,
                                                         DateTime.UtcNow.AddDays(7));

            if (refreshTokenResult.IsFailure)
                return refreshTokenResult.Errors;

            var refreshToken = refreshTokenResult.Value;

            await _context.RefreshTokens.AddAsync(refreshToken, ct);

            await _context.SaveChangesAsync(ct);

            return new TokenResponse(tokenHandler.WriteToken(token),
                                     refreshToken.Token!,
                                     Expire);
        }

        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]!)),
                ValidateIssuer = true,
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["JwtSettings:Audience"],
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
               !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token.");
            }

            return principal;
        }
    }
}
