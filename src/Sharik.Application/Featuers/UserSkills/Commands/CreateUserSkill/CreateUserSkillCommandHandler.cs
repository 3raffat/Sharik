using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.UserSkills.Dtos;
using Sharik.Application.Featuers.UserSkills.Mapper;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Skills.UserSkills;
using Sharik.Domain.User.Enums;

namespace Sharik.Application.Featuers.UserSkills.Commands.CreateUserSkill
{
    public sealed class CreateUserSkillCommandHandler(
        ILogger<CreateUserSkillCommandHandler> _logger,
        IAppDbContext _context)
        : IRequestHandler<CreateUserSkillCommand, Result<UserSkillDto>>
    {
        public async Task<Result<UserSkillDto>> Handle(CreateUserSkillCommand request, CancellationToken ct)
        {

            var isIncompleteProfile = await _context.Users
                .AnyAsync(u => u.Id == request.userId && u.ProfileStatus == ProfileStatus.Incomplete, ct);

            if (isIncompleteProfile)
            {
                _logger.LogInformation("User {UserId} profile is incomplete.", request.userId);
                return ApplicationErrors.ProfileIncomplete;
            }

            var existSkill = await _context.Skills.AnyAsync(us => us.Id == request.skillId, ct);

            if (!existSkill)
            {
                _logger.LogWarning("Skill with Id: {SkillId} not found", request.skillId);
                return ApplicationErrors.SkillNotFound;
            }

            var hasSkill = await _context.UserSkills.AnyAsync(us => us.UserId == request.userId && us.SkillId == request.skillId,
                                                              ct);

            if (hasSkill)
            {
                _logger.LogWarning("User {UserId} already has Skill {SkillId}", request.userId, request.skillId);
                return ApplicationErrors.SkillAlreadyExistsForUser;
            }


            var userSkillResult = UserSkill.Create(request.userId,
                                                   request.skillId,
                                                   request.skillLevel,
                                                   request.pointPerHour);

            if (userSkillResult.IsFailure)
                return userSkillResult.Errors;

            var userSkill = userSkillResult.Value;

            await _context.UserSkills.AddAsync(userSkill, ct);

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("User skills created successfully for UserId {UserId}.", userSkill.UserId);

            return userSkill.ToDto();
        }
    }
}
