using MediatR;
using Sharik.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.Exchanges.RejectExchanges
{
    public sealed record RejectExchangeCommand(Guid ProviderId , Guid ExchangeId) : IRequest<Result<Updated>>;

}
