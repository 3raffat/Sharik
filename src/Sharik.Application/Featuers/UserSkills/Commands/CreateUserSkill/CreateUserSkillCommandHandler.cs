using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.UserSkills.Dtos;
using Sharik.Application.Featuers.UserSkills.Mapper;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Skills.UserSkills;

namespace Sharik.Application.Featuers.UserSkills.Commands.CreateUserSkill
{
    public sealed class CreateUserSkillCommandHandler(
        ILogger<CreateUserSkillCommandHandler> _logger,
        IAppDbContext _context)
        : IRequestHandler<CreateUserSkillCommand, Result<UserSkillDto>>
    {
        public async Task<Result<UserSkillDto>> Handle(CreateUserSkillCommand request, CancellationToken cancellationToken)
        {
           
            var existSkill = await _context.Skills.AnyAsync(us => us.Id == request.skillId, cancellationToken);

            if (!existSkill)
            {
                _logger.LogWarning("Skill with Id: {SkillId} not found", request.skillId);
                return ApplicationErrors.SkillNotFound;
            }

            var hasSkill = await _context.UserSkills.AnyAsync(us => us.UserId == request.userId && us.SkillId == request.skillId,
                                                              cancellationToken);

            if (hasSkill)
            {
                _logger.LogWarning("User {UserId} already has Skill {SkillId}", request.userId,request.skillId);
                return ApplicationErrors.SkillAlreadyExistsForUser;
            }


            var userSkillResult = UserSkill.Create(request.userId,
                                                   request.skillId,
                                                   request.skillLevel,
                                                   request.pointPerHour);

            if (userSkillResult.IsFailure)
                return userSkillResult.Errors;

            var userSkill = userSkillResult.Value;

            await _context.UserSkills.AddAsync(userSkill, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User skills created successfully for UserId {UserId}.", userSkill.UserId);

            return userSkill.ToDto();
        }
    }
}
