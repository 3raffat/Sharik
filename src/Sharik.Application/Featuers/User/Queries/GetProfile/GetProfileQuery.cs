using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.User.Dtos;
using Sharik.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.User.Queries.GetProfile
{
    public sealed record GetProfileQuery(Guid UserId) : ICachedQuery<Result<CompleteUserProfileDto>>
    {
        public string CacheKey => CacheKeys.User.UserById(UserId);

        public TimeSpan Expiration => CacheKeys.ShortExpiration;
    }
}
