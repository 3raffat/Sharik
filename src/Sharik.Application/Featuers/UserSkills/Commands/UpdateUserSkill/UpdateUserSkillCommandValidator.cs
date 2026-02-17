using FluentValidation;
using Sharik.Domain.Skills.UserSkills;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.UserSkills.Commands.UpdateUserSkill
{
    public sealed class UpdateUserSkillCommandValidator:AbstractValidator<UpdateUserSkillCommand>
    {
        public UpdateUserSkillCommandValidator()
        {
            RuleFor(us => us.userId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(UserSkillErrors.UserIdRequired.Description);

            RuleFor(us => us.skillId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                 .WithMessage(UserSkillErrors.SkillIdRequired.Description);

            RuleFor(us => us.skillLevel).Cascade(CascadeMode.Stop)
                .NotEmpty()
                 .WithMessage(UserSkillErrors.SkillLevelRequired.Description);

            RuleFor(us => us.pointPerHour).Cascade(CascadeMode.Stop)
                .NotEmpty()
                 .WithMessage(UserSkillErrors.PointPerHourRequired.Description);

        }
    }
}
