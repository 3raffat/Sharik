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
            RuleFor(us => us.userId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(UserSkillErrors.UserIdRequired.Description);

            RuleFor(us => us.skillId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                 .WithMessage(UserSkillErrors.SkillIdRequired.Description);

            RuleFor(us => us.skillLevel).Cascade(CascadeMode.Stop)
                .IsInEnum()
                 .WithMessage(UserSkillErrors.SkillLevelRequired.Description);

            RuleFor(us => us.pointPerHour).Cascade(CascadeMode.Stop)
                .NotEmpty()
                 .WithMessage(UserSkillErrors.PointPerHourRequired.Description);

        }
    }
}
