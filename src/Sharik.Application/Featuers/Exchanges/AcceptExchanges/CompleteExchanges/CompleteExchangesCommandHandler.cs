using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Exchanges.CancelleExchanges;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Exchanges.AcceptExchanges.CompleteExchanges
{
    public sealed class CompleteExchangesCommandHandler(
       ILogger<CompleteExchangesCommandHandler> _logger ,
      IAppDbContext _context) : IRequestHandler<CompleteExchangesCommand , Result<Updated>>
    {
        public async Task<Result<Updated>> Handle(CompleteExchangesCommand request , CancellationToken ct)
        {
            var existingExchange = await _context.Exchanges
                .FirstOrDefaultAsync(e => e.Id == request.ExchangeId && e.ProviderId == request.ProviderId , ct);

            if (existingExchange is null)
            {
                _logger.LogWarning("Exchange with ID {ExchangeId} not found for provider {ProviderId}." , request.ExchangeId , request.ProviderId);
                return ApplicationErrors.ExchangeNotFound;
            }

        
            var cancelResult = existingExchange.CompleteExchange();

            if (cancelResult.IsFailure)
                return cancelResult.Errors;

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Exchange with ID {ExchangeId} completed successfully for provider {ProviderId}." , request.ExchangeId , request.ProviderId);

            return Result.Updated;
        }
    }
}
