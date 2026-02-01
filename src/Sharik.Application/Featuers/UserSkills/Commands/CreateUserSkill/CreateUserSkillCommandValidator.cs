using FluentValidation;
using Sharik.Domain.Skills.UserSkills;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.UserSkills.Commands.CreateUserSkill
{
    public sealed class CreateUserSkillCommandValidator :AbstractValidator<CreateUserSkillCommand>
    {
        public CreateUserSkillCommandValidator()
        {
            RuleFor(us => us.userId)
                .NotEmpty()
                .WithErrorCode(UserSkillErrors.UserIdRequired.Code)
                .WithMessage(UserSkillErrors.UserIdRequired.Description);

            RuleFor(us => us.skillId)
                .NotEmpty()
                 .WithErrorCode(UserSkillErrors.SkillIdRequired.Code)
                 .WithMessage(UserSkillErrors.SkillIdRequired.Description);

            RuleFor(us => us.skillLevel)
                .NotEmpty()
                 .WithErrorCode(UserSkillErrors.SkillLevelRequired.Code)
                 .WithMessage(UserSkillErrors.SkillLevelRequired.Description);

            RuleFor(us=>us.pointPerHour)
                .NotEmpty()
                 .WithErrorCode(UserSkillErrors.PointPerHourRequired.Code)
                 .WithMessage(UserSkillErrors.PointPerHourRequired.Description);

        }
    }
}
