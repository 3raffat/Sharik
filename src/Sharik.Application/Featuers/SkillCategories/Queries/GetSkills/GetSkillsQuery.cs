using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.SkillCategories.Dtos;
using Sharik.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.SkillCategories.Queries.GetSkills
{
    public sealed record GetSkillsQuery() : ICachedQuery<Result<List<SkillDto>>>
    {
        public string CacheKey => CacheKeys.Skill.AllSkills;

        public TimeSpan Expiration => CacheKeys.ShortExpiration;
    }
}
