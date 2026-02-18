using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Exchanges.CreateExchanges;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Exchanges;
using Sharik.Domain.Exchanges.Enums;
using Sharik.Domain.Notifications;
using Sharik.Domain.Notifications.Enums;

namespace Sharik.Application.Featuers.Exchanges.CreateTeachingExchanges
{
    public sealed class CreateTeachingExchangesCommandHandler(
        ILogger<CreateSwapExchangesCommandHandler> _logger ,
        IAppDbContext _context ,
        INotificationApplicationService _notificationService ,
        HybridCache _cache) : IRequestHandler<CreateTeachingExchangesCommand , Result<Success>>
    {
        public async Task<Result<Success>> Handle(CreateTeachingExchangesCommand request , CancellationToken ct)
        {
            var validationData = await _context.Users.Include(u => u.UserSkills)
                           .Where(u => u.Id == request.requesterId || u.Id == request.providerId).ToListAsync(ct);


            var provider = validationData.FirstOrDefault(d => d.Id == request.providerId);
            if (provider == null)
            {
                _logger.LogWarning(
                    "Provider {ProviderId} not found" ,
                    request.providerId);
                return ApplicationErrors.ProviderNotFound;
            }

            var providerSkill = provider.UserSkills.FirstOrDefault(s => s.SkillId == request.skillRequestedId);

            if (providerSkill is null)
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
                    e.SkillRequestedId == request.skillRequestedId &&
                    e.ExchangeStatus == ExchangeStatus.Pending &&
                    e.Type == ExchangeType.Teaching ,
                    ct);

            if (exists)
            {
                _logger.LogWarning(
                    "Duplicate exchange from {RequesterId} to {ProviderId}" ,
                    request.requesterId , request.providerId);
                return ApplicationErrors.ExchangeAlreadyExists;
            }

            var requester = validationData.FirstOrDefault(r => r.Id == request.requesterId);

            var requiredPoints = Exchange.CalculateTotalPoints(providerSkill.PointPerHour , requester!.TotalPointsEarned , request.duration);

            if (requiredPoints.IsFailure)
                return requiredPoints.Errors;

            var deductRequesterPoints = requester.DeductPoints(requiredPoints.Value);

            if (deductRequesterPoints.IsFailure)
                return deductRequesterPoints.Errors;

            var exchangeResult = Exchange.CreateTeaching(request.requesterId ,
                                                         request.providerId ,
                                                         request.skillRequestedId ,
                                                         request.duration ,
                                                         requiredPoints.Value ,
                                                         request.requesterMessage);

            if (exchangeResult.IsFailure)
                return exchangeResult.Errors;


            await _context.Exchanges.AddAsync(exchangeResult.Value , ct);

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Exchange created successfully with id {ExchangeId}" , exchangeResult.Value.Id);

            await _notificationService.CreateAndSendNotificationAsync(request.providerId ,
                                                                      NotificationType.NewExchangeRequest ,
                                                                      NotificationMessage.NewExchangeRequest(requester.FirstName!) , ct);

            await _cache.RemoveAsync(CacheKeys.Exchange.ExchangeByProviderId(request.providerId) , ct);

            await _cache.RemoveAsync(CacheKeys.User.UserById(requester.Id));

            return Result.Success;
        }

    }
}
