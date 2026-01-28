using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.SkillCategories.Dtos;
using Sharik.Application.Featuers.SkillCategories.Mappers;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Skills;

namespace Sharik.Application.Featuers.SkillCategories.Commands.UpdateSkillCategory
{
    public sealed class UpdateSkillCategoryCommandHandler(
        ILogger<UpdateSkillCategoryCommandHandler> _logger,
        IAppDbContext _context)
        : IRequestHandler<UpdateSkillCategoryCommand, Result<SkillCategoryDto>>
    {
        public async Task<Result<SkillCategoryDto>> Handle(UpdateSkillCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.SkillCategories.FirstOrDefaultAsync(sc => sc.Id == request.Id, cancellationToken);

            if (category is null)
            {
                _logger.LogWarning("Category with Id {CategoryId} not found", request.Id);
                return ApplicationErrors.SkillCategoryNotFound;
            }

            if (!category.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase))
            {
                var categoryNameExists = await _context.SkillCategories
                    .AsNoTracking()
                    .AnyAsync(s => s.Name == request.Name
                                && s.Id != request.Id,
                              cancellationToken);

                if (categoryNameExists)
                {
                    _logger.LogWarning("Category with name {CategoryName} already exists", request.Name);
                    return ApplicationErrors.SkillCategoryAlreadyExists;
                }
            }

            var categoryResult = category.Update(request.Name);

            if (categoryResult.IsFailure)
                return categoryResult.Errors;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Category with Id: {CategoryId} updated successfully", request.Id);


            return category.ToDto();
        }
    }
}
