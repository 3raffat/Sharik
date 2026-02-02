using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Skills.Dtos;
using Sharik.Application.Featuers.Skills.Mapper;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Skills;

namespace Sharik.Application.Featuers.Skills.Commands.CreateSkill
{
    public sealed class CreateSkillCommandHandler(
        ILogger<CreateSkillCommandHandler> _logger,
        IAppDbContext _context) : IRequestHandler<CreateSkillCommand, Result<SkillDto>>
    {
        public async Task<Result<SkillDto>> Handle(CreateSkillCommand request, CancellationToken ct)
        {
            var categoryExists = await _context.SkillCategories
                .AnyAsync(c => c.Id == request.CategoryId, ct);

            if (!categoryExists)
            {
                _logger.LogWarning("Skill category with ID {CategoryId} not found.", request.CategoryId);
                return ApplicationErrors.SkillCategoryNotFound;
            }

            var skillName = request.Name.Trim().ToLower();

            var skillExists = await _context.Skills
                .AnyAsync(s => s.Name.ToLower() == skillName, ct);

            if (skillExists)
            {
                _logger.LogWarning("Skill with name {SkillName} already exists.", request.Name);
                return ApplicationErrors.SkillAlreadyExists;
            }

            var skillResult = Skill.Create(request.CategoryId,
                                           request.Name);

            if (skillResult.IsFailure)
                return skillResult.Errors;


            var skill = skillResult.Value;

            await _context.Skills.AddAsync(skill, ct);

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Skill with ID {SkillId} created successfully.", skill.Id);


            return skill.ToDto();
        }
    }
}
