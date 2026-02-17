using FluentValidation;
using Sharik.Domain.Skills;
using Sharik.Domain.Skills.SkillCategories;

namespace Sharik.Application.Featuers.SkillCategories.Commands.UpdateSkill
{
    public sealed class UpdateSkillCommandValidator : AbstractValidator<UpdateSkillCommand>
    {
        public UpdateSkillCommandValidator()
        {
            RuleFor(x => x.Name)
               .Cascade(CascadeMode.Stop)
               .NotEmpty()
                   .WithMessage(SkillErrors.SkillNameRequired.Description)
               .MaximumLength(100)
                  .WithMessage(SkillErrors.SkillNameTooLong.Description);

            RuleFor(x => x.SkillCategoryId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(SkillCategoryErrors.SkillCategoryIdRequired.Description)
                .NotEqual(Guid.Empty)
                    .WithMessage(SkillCategoryErrors.SkillCategoryIdRequired.Description);

            RuleFor(x => x.SkillId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                   .WithMessage(SkillErrors.SkillIdRequired.Description)
                .NotEqual(Guid.Empty)
                   .WithMessage(SkillErrors.SkillIdRequired.Description);
        }
    }
}
