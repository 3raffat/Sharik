using Sharik.Application.Featuers.Auth.Dtos;
using Sharik.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Sharik.Application.Common.Interfaces
{
    public interface ITokenProvider
    {
       Task<Result<TokenResponse>> GenerateJwtTokenAsync(AppUserDto user, CancellationToken ct = default);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

    }
}
