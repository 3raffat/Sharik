using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.SkillCategories.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.SkillCategories.Queries.GetCategory
{
    public sealed record GetCategoriesQuery() : ICachedQuery<Result<List<SkillCategoryDto>>>
    {
        public string CacheKey => CacheKeys.Category.AllCategories;

        public TimeSpan Expiration => CacheKeys.ShortExpiration;
    }
}
