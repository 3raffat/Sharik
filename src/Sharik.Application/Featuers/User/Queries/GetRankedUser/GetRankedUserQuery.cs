using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.User.Dtos;
using Sharik.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.User.Queries.GetRankedUser
{
    public sealed class GetRankedUserQuery : ICachedQuery<Result<List<RankedUserDto>>>
    {
        public string CacheKey => CacheKeys.User.AllUsers;

        public TimeSpan Expiration => CacheKeys.LongExpiration;
    }
}
