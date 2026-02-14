using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.SkillCategories.Commands.DeleteSkill
{
    public sealed class DeleteSkillCommandHandler(
        ILogger<DeleteSkillCommandHandler> _logger,
        IAppDbContext _context,HybridCache _cache) : IRequestHandler<DeleteSkillCommand, Result<Deleted>>
    {
        public async Task<Result<Deleted>> Handle(DeleteSkillCommand request, CancellationToken ct)
        {
           var category = await _context.SkillCategories
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == request.CategoryId, ct);

            if (category is null)
            {
                _logger.LogWarning("Skill category with ID {CategoryId} not found.", request.CategoryId);
                return ApplicationErrors.SkillCategoryNotFound;
            }

            var skillResult = category.RemoveSkill(request.SkillId);

            if (skillResult.IsFailure)
            return skillResult.Errors;

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Skill with Id: {SkillId} deleted successfully.", request.SkillId);

            await _cache.RemoveAsync(CacheKeys.Skill.AllSkills , ct);

            return Result.Deleted;
        }
    }
}
