using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Skills.Commands.DeleteSkill
{
    public sealed class DeleteSkillCommandHandler(
        ILogger<DeleteSkillCommandHandler> _logger,
        IAppDbContext _context) : IRequestHandler<DeleteSkillCommand, Result<Deleted>>
    {
        public async Task<Result<Deleted>> Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
        {
            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == request.SkillId,
                                                                  cancellationToken);

            if (skill is null)
            {
                _logger.LogWarning("Skill with Id: {SkillId} not found", request.SkillId);
                return ApplicationErrors.SkillNotFound;
            }


            _context.Skills.Remove(skill);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Skill with Id: {SkillId} deleted successfully.", request.SkillId);

            return Result.Deleted;
        }
    }
}
