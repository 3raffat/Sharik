using FluentValidation;
using Sharik.Domain.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.User.Commands.CompleteProfile
{
    public sealed class CompleteProfileCommandValidator :AbstractValidator<CompleteProfileCommand>
    {
        public CompleteProfileCommandValidator()
        {
            RuleFor(x => x.userId).Cascade(CascadeMode.Stop)
               .NotEmpty()
               .WithMessage(AppUserErrors.UserIdRequired.Description);

            RuleFor(x => x.firstName).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(AppUserErrors.FirstNameRequired.Description);

            RuleFor(x => x.lastName).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(AppUserErrors.LastNameRequired.Description);

            RuleFor(x => x.bio).Cascade(CascadeMode.Stop)
                .MaximumLength(1000)
                .WithMessage(AppUserErrors.BioTooLong.Description);
        }
    }
}
