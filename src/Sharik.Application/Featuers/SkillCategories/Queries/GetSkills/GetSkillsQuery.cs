using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.SkillCategories.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.SkillCategories.Queries.GetSkillsQuery
{
    public sealed record GetSkillsQuery() : ICachedQuery<Result<List<SkillWithUsersDto>>>
    {
        public string CacheKey => CacheKeys.Skill.AllSkills;

        public TimeSpan Expiration => CacheKeys.LongExpiration;
    }

}
