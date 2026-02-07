using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.SkillCategories.Dtos;
using Sharik.Application.Featuers.SkillCategories.Mappers;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Skills;

namespace Sharik.Application.Featuers.SkillCategories.Commands.CreateSkill
{
    public sealed class CreateSkillCommandHandler(
        ILogger<CreateSkillCommandHandler> _logger,
        IAppDbContext _context) : IRequestHandler<CreateSkillCommand, Result<SkillDto>>
    {
        public async Task<Result<SkillDto>> Handle(CreateSkillCommand request, CancellationToken ct)
        {
            var category = await _context.SkillCategories
                .Include(c => c.Skills)
                .SingleOrDefaultAsync(c => c.Id == request.CategoryId, ct);

            if (category is null)
            {
                _logger.LogWarning("Skill category with ID {CategoryId} not found.", request.CategoryId);
                return ApplicationErrors.SkillCategoryNotFound;
            }

            var skillResult = category.AddSkill(request.Name);

            if (skillResult.IsFailure)
                return skillResult.Errors;

            var skill = skillResult.Value;

           var state = _context.Entry(category).State;
            
            await _context.Skills.AddAsync(skill, ct);

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Skill with ID {SkillId} created successfully.", skill.Id);


            return skill.ToDto();
        }
    }
}
