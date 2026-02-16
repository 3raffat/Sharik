using FluentValidation;
using Sharik.Domain.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.User.Commands.UpdateProfile
{
    public sealed class UpdateProfileCommandValidator:AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {

            RuleFor(x => x.userId)
             .NotEmpty()
             .WithMessage(AppUserErrors.UserIdRequired.Description);

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage(AppUserErrors.FirstNameRequired.Description);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage(AppUserErrors.LastNameRequired.Description);

            RuleFor(x => x.Bio)
                .MaximumLength(1000)
                .WithMessage(AppUserErrors.BioTooLong.Description);

        }
    }
}
