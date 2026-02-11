using MediatR;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Common.Results.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.Auth.Commands.ConfirmEmail
{
    public sealed record ConfirmEmailCommand(string userId , string token): IRequest<Result<Success>>;

}
