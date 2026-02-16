using MediatR;
using Sharik.Application.Featuers.Auth.Dtos;
using Sharik.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.Auth.Queries.RefreshTokens
{
    public record RefreshTokenQuery(string RefreshToken , string ExpiredAccessToken) : IRequest<Result<TokenResponse>>;

}
