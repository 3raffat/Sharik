using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Exchanges.CompleteExchanges;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Notifications;
using Sharik.Domain.Notifications.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.Exchanges.RejectExchanges
{

    public sealed class RejectExchangeCommandHandler(
       ILogger<RejectExchangeCommandHandler> _logger ,
      IAppDbContext _context , INotificationApplicationService _notificationService ,
      HybridCache _cache) : IRequestHandler<RejectExchangeCommand , Result<Updated>>
    {
        public async Task<Result<Updated>> Handle(RejectExchangeCommand request , CancellationToken ct)
        {
            var existingExchange = await _context.Exchanges.Include(e=>e.Requester)
                .FirstOrDefaultAsync(e => e.Id == request.ExchangeId && e.ProviderId == request.ProviderId , ct);

            if (existingExchange is null)
            {
                _logger.LogWarning("Exchange with ID {ExchangeId} not found for provider {ProviderId}." , request.ExchangeId , request.ProviderId);
                return ApplicationErrors.ExchangeNotFound;
            }


            var cancelResult = existingExchange.RejectExchange(request.ProviderId);

            if (cancelResult.IsFailure)
                return cancelResult.Errors;

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Exchange with ID {ExchangeId} rejected for provider {ProviderId}." , request.ExchangeId , request.ProviderId);


            await _notificationService.CreateAndSendNotificationAsync(existingExchange.RequesterId ,
                                                                      NotificationType.ExchangeRejected ,
                                                                      NotificationMessage.ExchangeRejected(existingExchange.Id) ,
                                                                      ct);

            await _cache.RemoveAsync(CacheKeys.Exchange.ExchangeByProviderId(existingExchange.ProviderId) , ct);

            return Result.Updated;
        }
    }
}
