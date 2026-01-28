using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.SkillCategories.Commands.DeleteSkillCategory
{
    public sealed class DeleteSkillCategoryCommandHandler(
        ILogger<DeleteSkillCategoryCommandHandler> _logger,
        IAppDbContext _context)
        : IRequestHandler<DeleteSkillCategoryCommand, Result<Deleted>>
    {
        public async Task<Result<Deleted>> Handle(DeleteSkillCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.SkillCategories.FirstOrDefaultAsync(sc => sc.Id == request.Id,
                                                                              cancellationToken);

            if (category is null)
            {
                _logger.LogWarning("Skill category with ID {CategoryId} not found.", request.Id);
                return ApplicationErrors.SkillCategoryNotFound;
            }

            _context.SkillCategories.Remove(category);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Category with Id: {CategoryId} deleted successfully.", request.Id);

            return Result.Deleted;
        }
    }
}
