using FluentValidation;
using Sharik.Domain.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.Auth.Commands.ConfirmEmail
{
    public sealed class ConfirmEmailCommandValidator :AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator() 
        {

            RuleFor(x => x.userId)
           .NotEmpty()
           .WithMessage(AppUserErrors.UserIdRequired.Description);

            RuleFor(x => x.token)
                .NotEmpty()
                .WithMessage(AppUserErrors.TokenRequired.Description);

        }
    }
}
