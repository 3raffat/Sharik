using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Notifications;
using Sharik.Domain.Notifications.Enums;

namespace Sharik.Application.Featuers.Exchanges.AcceptExchanges
{
    public sealed class AcceptExchangesCommandHandler(
        ILogger<AcceptExchangesCommandHandler> _logger ,
        IAppDbContext _context , INotificationApplicationService _notificationService ,
        HybridCache _cache) : IRequestHandler<AcceptExchangesCommand , Result<Updated>>
    {
        public async Task<Result<Updated>> Handle(AcceptExchangesCommand request , CancellationToken ct)
        {
            var exchangeExists = await _context.Exchanges.FirstOrDefaultAsync(
                e => e.Id == request.ExchangeId , ct);

            if (exchangeExists is null)
            {
                _logger.LogWarning("Exchange with id {ExchangeId} not found." , request.ExchangeId);
                return ApplicationErrors.ExchangeNotFound;
            }


            var acceptResult = exchangeExists.AcceptExchange(request.ProviderId);

            if (acceptResult.IsFailure)
                return acceptResult.Errors;

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Exchange with id {ExchangeId} accepted successfully." , request.ExchangeId);

            await _notificationService.CreateAndSendNotificationAsync(exchangeExists.RequesterId ,
                                                                      NotificationType.ExchangeAccepted ,
                                                                      NotificationMessage.ExchangeAccepted(exchangeExists.Id) ,
                                                                      ct);

            await _cache.RemoveAsync(CacheKeys.Exchange.ExchangeByProviderId(exchangeExists.ProviderId) , ct);

            return Result.Updated;
        }
    }
}
