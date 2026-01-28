using FluentValidation;
using Sharik.Domain.Skills.SkillCategories;

namespace Sharik.Application.Featuers.SkillCategories.Commands.UpdateSkillCategory
{
    public sealed class UpdateSkillCategoryCommandValidator : AbstractValidator<UpdateSkillCategoryCommand>
    {
        public UpdateSkillCategoryCommandValidator()
        {
            RuleFor(sc => sc.Name)
               .NotEmpty()
                  .WithErrorCode(SkillCategoryErrors.SkillCategoryNameRequired.Code)
                  .WithMessage(SkillCategoryErrors.SkillCategoryNameRequired.Description)
               .MaximumLength(20)
                   .WithErrorCode(SkillCategoryErrors.SkillCategoryNameTooLong.Code)
                   .WithMessage(SkillCategoryErrors.SkillCategoryNameTooLong.Description)
                .MinimumLength(3)
                   .WithErrorCode(SkillCategoryErrors.SkillCategoryNameTooShort.Code)
                   .WithMessage(SkillCategoryErrors.SkillCategoryNameTooShort.Description);

            RuleFor(x => x.Id)
                  .NotEmpty()
                      .WithErrorCode(SkillCategoryErrors.SkillCategoryIdRequired.Code)
                      .WithMessage(SkillCategoryErrors.SkillCategoryIdRequired.Description)
                  .NotEqual(Guid.Empty)
                      .WithErrorCode(SkillCategoryErrors.SkillCategoryIdRequired.Code)
                      .WithMessage(SkillCategoryErrors.SkillCategoryIdRequired.Description);
        }
    }
}
