using FluentValidation;
using Sharik.Domain.Skills;

namespace Sharik.Application.Featuers.Skills.Commands.DeleteSkill
{
    public sealed class DeleteSkillCommandValidator : AbstractValidator<DeleteSkillCommand>
    {
        public DeleteSkillCommandValidator()
        {
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
