using FluentValidation;
using Sharik.Domain.Skills;
using Sharik.Domain.Skills.SkillCategories;

namespace Sharik.Application.Featuers.SkillCategories.Commands.CreateSkill
{
    public sealed class CreateSkillCommandValidator : AbstractValidator<CreateSkillCommand>
    {
        public CreateSkillCommandValidator()
        {
            RuleFor(x => x.Name).Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(SkillErrors.SkillNameRequired.Description)
                .MaximumLength(100)
                   .WithMessage(SkillErrors.SkillNameTooLong.Description)
                .MinimumLength(3)
                   .WithMessage(SkillErrors.SkillNameTooShort.Description);


            RuleFor(x => x.CategoryId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(SkillCategoryErrors.SkillCategoryIdRequired.Description)
                .NotEqual(Guid.Empty)
                    .WithMessage(SkillCategoryErrors.SkillCategoryIdRequired.Description);

        }
    }
}
