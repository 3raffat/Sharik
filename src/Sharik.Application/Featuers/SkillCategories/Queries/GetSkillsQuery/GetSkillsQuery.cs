using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.SkillCategories.Dtos;

namespace Sharik.Application.Featuers.SkillCategories.Queries.GetSkillsQuery
{
    public sealed record GetSkillsQuery() : ICachedQuery<List<SkillDto>>
    {
        public string CacheKey => CacheKeys.Skill.AllSkills;

        public TimeSpan Expiration => CacheKeys.LongExpiration;
    }

}
