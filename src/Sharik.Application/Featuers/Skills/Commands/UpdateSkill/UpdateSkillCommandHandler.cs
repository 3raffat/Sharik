using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Skills.Dtos;
using Sharik.Application.Featuers.Skills.Mapper;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Skills.Commands.UpdateSkill
{
    public sealed class UpdateSkillCommandHandler(
        ILogger<UpdateSkillCommandHandler> _logger,
        IAppDbContext _context) : IRequestHandler<UpdateSkillCommand, Result<SkillDto>>
    {
        public async Task<Result<SkillDto>> Handle(UpdateSkillCommand request, CancellationToken ct)
        {
            var skill = await _context.Skills.FirstOrDefaultAsync(
                s => s.Id == request.SkillId, ct);

            if (skill is null)
            {
                _logger.LogWarning("Skill with Id: {SkillId} not found", request.SkillId);
                return ApplicationErrors.SkillNotFound;
            }

            var skillName = request.Name.Trim();

            if (!skill.Name.Equals(skillName, StringComparison.OrdinalIgnoreCase))
            {
                var skillNameExists = await _context.Skills
                    .AsNoTracking()
                    .AnyAsync(s => s.Name.ToLower() == skillName.ToLower()
                                && s.Id != request.SkillId,
                              ct);

                if (skillNameExists)
                {
                    _logger.LogWarning("Skill with name {SkillName} already exists", skillName);
                    return ApplicationErrors.SkillAlreadyExists;
                }
            }

            var skillCategoryExists = await _context.SkillCategories
                .AsNoTracking()
                .AnyAsync(sc => sc.Id == request.SkillCategoryId, ct);

            if (!skillCategoryExists)
            {
                _logger.LogWarning("Skill category with Id: {SkillCategoryId} not found", request.SkillCategoryId);
                return ApplicationErrors.SkillCategoryNotFound;
            }

            var skillResult = skill.Update(request.Name,
                                           request.SkillCategoryId);

            if (skillResult.IsFailure)
                return skillResult.Errors;

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Skill with Id: {SkillId} updated successfully", request.SkillId);

            return skill.ToDto();
        }
    }
}
