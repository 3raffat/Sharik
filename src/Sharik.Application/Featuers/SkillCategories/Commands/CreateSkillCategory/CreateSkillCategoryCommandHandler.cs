using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.SkillCategories.Dtos;
using Sharik.Application.Featuers.SkillCategories.Mappers;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Skills.SkillCategories;

namespace Sharik.Application.Featuers.SkillCategories.Commands.CreateSkillCategory
{
    public sealed class CreateSkillCategoryCommandHandler(
        ILogger<CreateSkillCategoryCommandHandler> _logger,
        IAppDbContext _context) : IRequestHandler<CreateSkillCategoryCommand, Result<SkillCategoryDto>>
    {
        public async Task<Result<SkillCategoryDto>> Handle(CreateSkillCategoryCommand request, CancellationToken cancellationToken)
        {
            var skillNameExists = await _context.SkillCategories.AnyAsync(sc => sc.Name == request.Name, cancellationToken);

            if (skillNameExists)
            {
                _logger.LogWarning("Category with name {CategoryName} already exists", request.Name);
                return ApplicationErrors.SkillCategoryAlreadyExists;
            }

            var skillCategoryResult = SkillCategory.Create(Guid.NewGuid(),
                                                           request.Name);

            if (skillCategoryResult.IsFailure)
                return skillCategoryResult.Errors;

            var skillCategory = skillCategoryResult.Value;

            await _context.SkillCategories.AddAsync(skillCategory, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Category with ID {CategoryId} created successfully.", skillCategory.Id);

            return skillCategory.ToDto();
        }
    }
}
