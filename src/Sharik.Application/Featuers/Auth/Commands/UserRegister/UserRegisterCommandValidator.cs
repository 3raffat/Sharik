using FluentValidation;
using Sharik.Domain.User;

namespace Sharik.Application.Featuers.Auth.Commands.UserRegister
{
    public sealed class UserRegisterCommandValidator : AbstractValidator<UserRegisterCommand>
    {

        public UserRegisterCommandValidator()
        {

            RuleFor(x => x.username)
                .NotEmpty()
                .WithMessage(AppUserErrors.UserNameRequired.Description)
                .MinimumLength(3)
                .WithMessage(AppUserErrors.UserNameTooShort.Description)
                .MaximumLength(15)
                .WithMessage(AppUserErrors.UserNameTooLong.Description);

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
