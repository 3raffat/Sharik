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
                   .WithErrorCode(SkillCategoryErrors.SkillCategoryNameRequired.Code)
                   .WithMessage(SkillCategoryErrors.SkillCategoryNameRequired.Description)
                .MaximumLength(20)
                    .WithErrorCode(SkillCategoryErrors.SkillCategoryNameTooLong.Code)
                    .WithMessage(SkillCategoryErrors.SkillCategoryNameTooLong.Description)
                 .MinimumLength(3)
                    .WithErrorCode(SkillCategoryErrors.SkillCategoryNameTooShort.Code)
                    .WithMessage(SkillCategoryErrors.SkillCategoryNameTooShort.Description);

        }
    }
}
