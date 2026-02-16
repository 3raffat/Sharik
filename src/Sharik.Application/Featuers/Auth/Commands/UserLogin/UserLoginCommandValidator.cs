using FluentValidation;
using Sharik.Domain.User;

namespace Sharik.Application.Featuers.Auth.Commands.UserLogin
{
    public sealed class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
    {
        public UserLoginCommandValidator()
        {

            RuleFor(x => x.email)
              .NotEmpty()
              .WithMessage(AppUserErrors.EmailRequired.Description)
              .EmailAddress()
              .WithMessage(AppUserErrors.EmailInvalid.Description);

            RuleFor(x => x.password)
                 .NotEmpty()
                 .WithMessage(AppUserErrors.PasswordRequired.Description)
                 .Matches(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]).{8,}$")
                 .WithMessage(AppUserErrors.PasswordInvalidFormat.Description);

        }
    }
}
