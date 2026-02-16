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
                  .WithMessage(SkillCategoryErrors.SkillCategoryNameRequired.Description)
               .MaximumLength(20)
                   .WithMessage(SkillCategoryErrors.SkillCategoryNameTooLong.Description)
                .MinimumLength(3)
                   .WithMessage(SkillCategoryErrors.SkillCategoryNameTooShort.Description);

            RuleFor(x => x.Id)
                  .NotEmpty()
                      .WithMessage(SkillCategoryErrors.SkillCategoryIdRequired.Description)
                  .NotEqual(Guid.Empty)
                      .WithMessage(SkillCategoryErrors.SkillCategoryIdRequired.Description);
        }
    }
}
