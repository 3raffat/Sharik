using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.UserSkills.Commands.DeleteUserSkill
{
    public sealed class DeleteUserSkillCommandHandler(
        ILogger<DeleteUserSkillCommandHandler> _logger,
        IAppDbContext _context, HybridCache _cache) : IRequestHandler<DeleteUserSkillCommand, Result<Deleted>>
    {
        public async Task<Result<Deleted>> Handle(DeleteUserSkillCommand request, CancellationToken ct)
        {

            var userSkill = await _context.UserSkills.FirstOrDefaultAsync(us => us.UserId == request.UserId && us.SkillId == request.SkillId,
                                                              ct);
            if (userSkill is null)
            {
                _logger.LogWarning("User {UserId} does not have Skill {SkillId}", request.UserId, request.SkillId);
                return ApplicationErrors.UserSkillNotFound;
            }

            _context.UserSkills.Remove(userSkill);

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("User skill deleted successfully for UserId {UserId}, SkillId {SkillId}",request.UserId, request.SkillId);

            await _cache.RemoveAsync(CacheKeys.UserSkill.UserSkillById(userSkill.UserId) , ct);

            await _cache.RemoveAsync(CacheKeys.User.UserById(userSkill.UserId) , ct);

            return Result.Deleted;

        }
    }
}
