using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.UserSkills.Dtos;
using Sharik.Application.Featuers.UserSkills.Mapper;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Skills.UserSkills;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.UserSkills.Commands.UpdateUserSkill
{
    public sealed class UpdateUserSkillCommandHandler(
        ILogger<UpdateUserSkillCommandHandler> _logger,
        IAppDbContext _context) : IRequestHandler<UpdateUserSkillCommand, Result<Updated>>
    {
        public async Task<Result<Updated>> Handle(UpdateUserSkillCommand request, CancellationToken ct)
        {
            var existSkill = await _context.Skills.AnyAsync(us => us.Id == request.skillId, ct);

            if (!existSkill)
            {
                _logger.LogWarning("Skill with Id: {SkillId} not found", request.skillId);
                return ApplicationErrors.SkillNotFound;
            }

            var hasSkill = await _context.UserSkills.FirstOrDefaultAsync(us => us.UserId == request.userId && us.SkillId == request.skillId,
                                                              ct);

            if (hasSkill is null)
            {
                _logger.LogWarning("User {UserId} does not have Skill {SkillId}",request.userId, request.skillId);
                return ApplicationErrors.UserSkillNotFound;
            }

            var userSkillResult = hasSkill.Update(request.skillLevel,
                                                  request.pointPerHour);

            if (userSkillResult.IsFailure)
                return userSkillResult.Errors;

            var userSkill = userSkillResult.Value;

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("User skills updated  successfully for UserId {UserId}.", request.userId);

            return Result.Updated;
        }
    }
}
