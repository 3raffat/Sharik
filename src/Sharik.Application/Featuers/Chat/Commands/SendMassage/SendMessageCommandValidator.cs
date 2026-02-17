using FluentValidation;
using Sharik.Domain.Messages;

namespace Sharik.Application.Featuers.Chat.Commands.SendMassage
{
    public sealed class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
    {
        public SendMessageCommandValidator()
        {
            RuleFor(x => x.ExchangeId)
               .Cascade(CascadeMode.Stop)
               .NotEmpty()
               .WithMessage(MessageErrors.ExchangeIdRequired.Description);

            RuleFor(x => x.SenderId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(MessageErrors.SenderIdRequired.Description);

            RuleFor(x => x.Content)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(MessageErrors.MassageRequired.Description)
                .MaximumLength(1000)
                .WithMessage(MessageErrors.ContentTooLong.Description);

        }
    }
}
