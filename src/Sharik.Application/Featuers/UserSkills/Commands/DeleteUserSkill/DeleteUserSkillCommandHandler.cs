using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.UserSkills.Commands.DeleteUserSkill
{
    public sealed class DeleteUserSkillCommandHandler(
        ILogger<DeleteUserSkillCommandHandler> _logger,
        IAppDbContext _context) : IRequestHandler<DeleteUserSkillCommand, Result<Deleted>>
    {
        public async Task<Result<Deleted>> Handle(DeleteUserSkillCommand request, CancellationToken cancellationToken)
        {

            var userSkill = await _context.UserSkills.FirstOrDefaultAsync(us => us.UserId == request.UserId && us.SkillId == request.SkillId,
                                                              cancellationToken);
            if (userSkill is null)
            {
                _logger.LogWarning("User {UserId} does not have Skill {SkillId}", request.UserId, request.SkillId);
                return ApplicationErrors.UserSkillNotFound;
            }

            _context.UserSkills.Remove(userSkill);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User skill deleted successfully for UserId {UserId}, SkillId {SkillId}",request.UserId, request.SkillId);

            return Result.Deleted;

        }
    }
}
