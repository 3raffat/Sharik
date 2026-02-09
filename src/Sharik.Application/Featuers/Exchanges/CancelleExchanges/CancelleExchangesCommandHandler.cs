using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Exchanges.CancelleExchanges
{
    public sealed class CancelleExchangesCommandHandler(
        ILogger<CancelleExchangesCommandHandler> _logger ,
       IAppDbContext _context) : IRequestHandler<CancelleExchangesCommand , Result<Success>>
    {
        public async Task<Result<Success>> Handle(CancelleExchangesCommand request , CancellationToken ct)
        {
            var existingExchange = await _context.Exchanges
                .FirstOrDefaultAsync(e => e.Id == request.ExchangeId && e.ProviderId == request.ProviderId , ct);

            if (existingExchange is null)
            {
                _logger.LogWarning("Exchange with ID {ExchangeId} not found for provider {ProviderId}." , request.ExchangeId , request.ProviderId);
                return ApplicationErrors.ExchangeNotFound;
            }

            var cancelResult = existingExchange.CancelExchange(request.cancellationReason);

            if(cancelResult.IsFailure)
                return cancelResult.Errors;

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Exchange with ID {ExchangeId} cancelled successfully for provider {ProviderId}." , request.ExchangeId , request.ProviderId);

            return Result.Success;
        }
    }
}
