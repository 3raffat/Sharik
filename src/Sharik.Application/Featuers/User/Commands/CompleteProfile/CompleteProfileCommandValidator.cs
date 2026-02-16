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
            RuleFor(x => x.userId)
               .NotEmpty()
               .WithMessage(AppUserErrors.UserIdRequired.Description);

            RuleFor(x => x.firstName)
                .NotEmpty()
                .WithMessage(AppUserErrors.FirstNameRequired.Description);

            RuleFor(x => x.lastName)
                .NotEmpty()
                .WithMessage(AppUserErrors.LastNameRequired.Description);

            RuleFor(x => x.bio)
                .MaximumLength(1000)
                .WithMessage(AppUserErrors.BioTooLong.Description);
        }
    }
}
