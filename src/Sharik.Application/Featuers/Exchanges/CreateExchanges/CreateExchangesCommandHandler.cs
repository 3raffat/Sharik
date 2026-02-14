using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Exchanges;
using Sharik.Domain.Exchanges.Enums;
using Sharik.Domain.Notifications;
using Sharik.Domain.Notifications.Enums;

namespace Sharik.Application.Featuers.Exchanges.CreateExchanges
{
    public sealed class CreateExchangesCommandHandler(
        ILogger<CreateExchangesCommandHandler> _logger ,
        IAppDbContext _context ,
        INotificationApplicationService _notificationService ,
        HybridCache _cache) : IRequestHandler<CreateExchangesCommand , Result<Success>>
    {
        public async Task<Result<Success>> Handle(CreateExchangesCommand request , CancellationToken ct)
        {
            var validationData = await _context.Users
                .Where(u => u.Id == request.requesterId || u.Id == request.providerId)
                .Select(u => new
                {
                    u.Id ,
                    u.FirstName ,
                    Skills = u.UserSkills
                        .Select(us => us.SkillId)
                        .ToList()
                })
                .ToListAsync(ct);

            var requester = validationData.FirstOrDefault(d => d.Id == request.requesterId);
            if (requester == null)
            {
                _logger.LogWarning(
                    "Requester {RequesterId} not found" ,
                    request.requesterId);
                return ApplicationErrors.UserNotFound;
            }


            var provider = validationData.FirstOrDefault(d => d.Id == request.providerId);
            if (provider == null)
            {
                _logger.LogWarning(
                    "Provider {ProviderId} not found" ,
                    request.providerId);
                return ApplicationErrors.ProviderNotFound;
            }


            if (!requester.Skills.Contains(request.skillOfferedId))
            {
                _logger.LogWarning(
                    "Skill offered {SkillId} not found for requester {RequesterId}" ,
                    request.skillOfferedId , request.requesterId);
                return ApplicationErrors.UserSkillNotFound;
            }


            if (!provider.Skills.Contains(request.skillRequestedId))
            {
                _logger.LogWarning(
                    "Skill requested {SkillId} not found for provider {ProviderId}" ,
                    request.skillRequestedId , request.providerId);
                return ApplicationErrors.UserSkillNotFound;
            }

            var exists = await _context.Exchanges
                .AnyAsync(e =>
                    e.RequesterId == request.requesterId &&
                    e.ProviderId == request.providerId &&
                    e.SkillOfferedId == request.skillOfferedId &&
                    e.SkillRequestedId == request.skillRequestedId &&
                    e.ExchangeStatus == ExchangeStatus.Pending ,
                    ct);

            if (exists)
            {
                _logger.LogWarning(
                    "Duplicate exchange from {RequesterId} to {ProviderId}" ,
                    request.requesterId , request.providerId);
                return ApplicationErrors.ExchangeAlreadyExists;
            }

            var exchangeResult = Exchange.Create(
                request.requesterId ,
                request.providerId ,
                request.skillOfferedId ,
                request.skillRequestedId ,
                request.type ,
                request.duration ,
                request.pointsValue ,
                request.requesterMessage);

            if (exchangeResult.IsFailure)
                return exchangeResult.Errors;

            var exchange = exchangeResult.Value;


            await _context.Exchanges.AddAsync(exchangeResult.Value , ct);

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Exchange created successfully with id {ExchangeId}" , exchangeResult.Value.Id);

            await _notificationService.CreateAndSendNotificationAsync(request.providerId ,
                                                                      NotificationType.NewExchangeRequest ,
                                                                      NotificationMessage.NewExchangeRequest(requester.FirstName!) , ct);

            await _cache.RemoveAsync(CacheKeys.Exchange.ExchangeByProviderId(request.providerId) , ct);

            return Result.Success;
        }
    }
}
