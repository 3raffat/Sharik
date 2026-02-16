using FluentValidation;
using Sharik.Domain.Skills;

namespace Sharik.Application.Featuers.SkillCategories.Commands.DeleteSkill
{
    public sealed class DeleteSkillCommandValidator : AbstractValidator<DeleteSkillCommand>
    {
        public DeleteSkillCommandValidator()
        {
            RuleFor(x => x.SkillId)
               .NotEmpty()
                  .WithMessage(SkillErrors.SkillIdRequired.Description)
               .NotEqual(Guid.Empty)
                  .WithMessage(SkillErrors.SkillIdRequired.Description);
        }
    }
}
