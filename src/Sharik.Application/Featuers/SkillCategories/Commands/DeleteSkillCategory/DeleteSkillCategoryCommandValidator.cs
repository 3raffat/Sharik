using FluentValidation;
using Sharik.Domain.Skills.SkillCategories;

namespace Sharik.Application.Featuers.SkillCategories.Commands.DeleteSkillCategory
{
    public sealed class DeleteSkillCategoryCommandValidator : AbstractValidator<DeleteSkillCategoryCommand>
    {
        public DeleteSkillCategoryCommandValidator()
        {
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
