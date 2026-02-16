using FluentValidation;
using Sharik.Domain.Skills.UserSkills;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.UserSkills.Commands.DeleteUserSkill
{
    public sealed class DeleteUserSkillCommandValidator:AbstractValidator<DeleteUserSkillCommand>
    {
        public DeleteUserSkillCommandValidator()
        {
            RuleFor(us => us.UserId)
              .NotEmpty()
              .WithMessage(UserSkillErrors.UserIdRequired.Description);

            RuleFor(us => us.SkillId)
                .NotEmpty()
                 .WithMessage(UserSkillErrors.SkillIdRequired.Description);
        }
    }
}
