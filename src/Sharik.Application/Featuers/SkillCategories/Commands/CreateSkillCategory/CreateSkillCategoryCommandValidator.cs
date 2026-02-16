using FluentValidation;
using Sharik.Domain.Skills.SkillCategories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.SkillCategories.Commands.CreateSkillCategory
{
    public sealed class CreateSkillCategoryCommandValidator :AbstractValidator<CreateSkillCategoryCommand>
    {
        public CreateSkillCategoryCommandValidator()
        {
            RuleFor(sc=>sc.Name)
                .NotEmpty()
                   .WithMessage(SkillCategoryErrors.SkillCategoryNameRequired.Description)
                .MaximumLength(20)
                    .WithMessage(SkillCategoryErrors.SkillCategoryNameTooLong.Description)
                 .MinimumLength(3)
                    .WithMessage(SkillCategoryErrors.SkillCategoryNameTooShort.Description);

        }
    }
}
