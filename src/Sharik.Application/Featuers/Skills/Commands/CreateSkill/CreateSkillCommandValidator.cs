using FluentValidation;
using Sharik.Domain.Skills;
using Sharik.Domain.Skills.SkillCategories;

namespace Sharik.Application.Featuers.Skills.Commands.CreateSkill
{
    public sealed class CreateSkillCommandValidator : AbstractValidator<CreateSkillCommand>
    {
        public CreateSkillCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithErrorCode(SkillErrors.SkillNameRequired.Code)
                    .WithMessage(SkillErrors.SkillNameRequired.Description)
                .MaximumLength(100)
                   .WithErrorCode(SkillErrors.SkillNameTooLong.Code)
                   .WithMessage(SkillErrors.SkillNameTooLong.Description)
                .MinimumLength(3)
                   .WithErrorCode(SkillErrors.SkillNameTooShort.Code)
                   .WithMessage(SkillErrors.SkillNameTooShort.Description);


            RuleFor(x => x.CategoryId)
                .NotEmpty()
                    .WithErrorCode(SkillCategoryErrors.SkillCategoryIdRequired.Code)
                    .WithMessage(SkillCategoryErrors.SkillCategoryIdRequired.Description)
                .NotEqual(Guid.Empty)
                    .WithErrorCode(SkillCategoryErrors.SkillCategoryIdRequired.Code)
                    .WithMessage(SkillCategoryErrors.SkillCategoryIdRequired.Description);

        }
    }
}
