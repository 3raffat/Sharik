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
           .Cascade(CascadeMode.Stop)
           .NotEmpty()
           .WithMessage(AppUserErrors.UserIdRequired.Description);

            RuleFor(x => x.token)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(AppUserErrors.TokenRequired.Description);

        }
    }
}
