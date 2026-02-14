using MediatR;
using Microsoft.EntityFrameworkCore;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.SkillCategories.Dtos;
using Sharik.Domain.Common.Results;
using Sharik.Application.Featuers.SkillCategories.Mappers;

namespace Sharik.Application.Featuers.SkillCategories.Queries.GetCategory
{
    public sealed class GetCategoriesQueryHandler(IAppDbContext _context) : IRequestHandler<GetCategoriesQuery , Result<List<SkillCategoryDto>>>
    {
        public async Task<Result<List<SkillCategoryDto>>> Handle(GetCategoriesQuery request , CancellationToken ct)
        {

            var data = await _context.SkillCategories.ToListAsync(ct);

            return data.ToDtos();
        }
    }
}
