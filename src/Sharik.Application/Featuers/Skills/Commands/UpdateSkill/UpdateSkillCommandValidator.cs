using FluentValidation;
using Sharik.Domain.Skills;
using Sharik.Domain.Skills.SkillCategories;

namespace Sharik.Application.Featuers.Skills.Commands.UpdateSkill
{
    public sealed class UpdateSkillCommandValidator : AbstractValidator<UpdateSkillCommand>
    {
        public UpdateSkillCommandValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty()
                   .WithErrorCode(SkillErrors.SkillNameRequired.Code)
                   .WithMessage(SkillErrors.SkillNameRequired.Description)
               .MaximumLength(100)
                  .WithErrorCode(SkillErrors.SkillNameTooLong.Code)
                  .WithMessage(SkillErrors.SkillNameTooLong.Description);

            RuleFor(x => x.SkillCategoryId)
                .NotEmpty()
                    .WithErrorCode(SkillCategoryErrors.SkillCategoryIdRequired.Code)
                    .WithMessage(SkillCategoryErrors.SkillCategoryIdRequired.Description)
                .NotEqual(Guid.Empty)
                    .WithErrorCode(SkillCategoryErrors.SkillCategoryIdRequired.Code)
                    .WithMessage(SkillCategoryErrors.SkillCategoryIdRequired.Description);

            RuleFor(x => x.SkillId)
                .NotEmpty()
                   .WithErrorCode(SkillErrors.SkillIdRequired.Code)
                   .WithMessage(SkillErrors.SkillIdRequired.Description)
                .NotEqual(Guid.Empty)
                   .WithErrorCode(SkillErrors.SkillIdRequired.Code)
                   .WithMessage(SkillErrors.SkillIdRequired.Description);
        }
    }
}
