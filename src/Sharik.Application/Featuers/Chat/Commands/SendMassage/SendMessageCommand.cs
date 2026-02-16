using MediatR;
using Sharik.Application.Featuers.Chat.Dtos;
using Sharik.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.Chat.Commands.SendMassage
{
    public record SendMessageCommand(Guid ExchangeId , Guid SenderId , string Content)
        : IRequest<Result<MessageDto>>;
}
