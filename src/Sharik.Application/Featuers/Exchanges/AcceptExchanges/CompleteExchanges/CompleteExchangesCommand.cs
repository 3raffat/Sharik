using MediatR;
using Sharik.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.Exchanges.AcceptExchanges.CompleteExchanges
{
    public sealed record  CompleteExchangesCommand(Guid ProviderId , Guid ExchangeId):IRequest<Result<Updated>>;
    
    
}
