using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.UserSkills.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.UserSkills.Queries
{
    public sealed record GetUserSkillsQuery(Guid userId) : ICachedQuery<Result<List<UserSkillsDto>>>
    {
        public string CacheKey => CacheKeys.UserSkill.UserSkillById(userId);

        public TimeSpan Expiration => CacheKeys.LongExpiration;
    }
}
