using Sharik.Application.Featuers.Exchanges.AcceptExchanges;
using Sharik.Application.Featuers.Exchanges.CreateExchanges;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Exchanges;

namespace Sharik.Application.Featuers.Exchanges.Validation
{
    public interface IExchangeBusinessValidator
    {
        Task<Result<Success>> ValidateCreateExchangeAsync(CreateExchangesCommand command , CancellationToken ct);

    }
}
