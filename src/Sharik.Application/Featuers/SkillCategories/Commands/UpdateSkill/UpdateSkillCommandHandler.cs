using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.SkillCategories.Commands.UpdateSkill
{
    public sealed class UpdateSkillCommandHandler(
        ILogger<UpdateSkillCommandHandler> _logger ,
        IAppDbContext _context , HybridCache _cache) : IRequestHandler<UpdateSkillCommand , Result<Updated>>
    {
        public async Task<Result<Updated>> Handle(UpdateSkillCommand request , CancellationToken ct)
        {

            var category = await _context.SkillCategories
                .Include(sc => sc.Skills)
                .FirstOrDefaultAsync(sc => sc.Id == request.SkillCategoryId , ct);

            if (category is null)
            {
                _logger.LogWarning("Skill category with ID {CategoryId} not found." , request.SkillCategoryId);
                return ApplicationErrors.SkillCategoryNotFound;
            }

            var skillResult = category.UpdateSkill(request.SkillId , request.Name);

            if (skillResult.IsFailure)
                return skillResult.Errors;

            await _context.SaveChangesAsync(ct);

            var skill = skillResult.Value;

            _logger.LogInformation("Skill with Id: {SkillId} updated successfully" , request.SkillId);

            await _cache.RemoveAsync(CacheKeys.Skill.AllSkills , ct);

            return Result.Updated;
        }
    }
}
