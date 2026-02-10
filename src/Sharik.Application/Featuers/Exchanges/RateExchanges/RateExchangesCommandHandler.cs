using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Exchanges.RateExchanges
{
    public sealed class RateExchangesCommandHandler(
        ILogger<RateExchangesCommandHandler> _logger ,
        IAppDbContext _context) : IRequestHandler<RateExchangesCommand , Result<Created>>
    {
        public async Task<Result<Created>> Handle(RateExchangesCommand request , CancellationToken ct)
        {
            var exchange = await _context.Exchanges.Include(e=>e.Ratings).FirstOrDefaultAsync(e => e.Id == request.exchangeId , ct);
            if (exchange is null)
            {
                _logger.LogWarning("Exchange with id {ExchangeId} not found for rating" , request.exchangeId);

                return ApplicationErrors.ExchangeNotFound;
            }

            var ratingResult = exchange.RateExchange(request.raterId ,
                                                     request.ratedUserId ,
                                                     request.score ,
                                                     request.comment);

            if (ratingResult.IsFailure)
                return ratingResult.Errors;

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Exchange with id {ExchangeId} rated successfully by user {RaterId}" , request.exchangeId , request.raterId);

            return Result.Created;
        }
    }
}
