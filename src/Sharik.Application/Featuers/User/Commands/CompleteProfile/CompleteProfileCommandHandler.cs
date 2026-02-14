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

namespace Sharik.Application.Featuers.User.Commands.CompleteProfile
{
    public sealed class CompleteProfileCommandHandler(
        ILogger<CompleteProfileCommandHandler> _logger ,
        IAppDbContext _context , INotificationApplicationService _notificationService ,
        HybridCache _cache) : IRequestHandler<CompleteProfileCommand , Result<Updated>>
    {
        public async Task<Result<Updated>> Handle(CompleteProfileCommand request , CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.userId , ct);

            if (user is null)
            {
                _logger.LogWarning("User not found with UserId: {UserId}" , request.userId);
                return ApplicationErrors.UserNotFound;
            }


            var userResult = user.CompleteProfile(request.firstName ,
                                                   request.lastName ,
                                                   request.bio);

            if (userResult.IsFailure)
                return userResult.Errors;

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("User profile completed successfully for UserId: {UserId}" , request.userId);


            await _notificationService.CreateAndSendNotificationAsync(user.Id ,
                                                                      NotificationType.ProfileCompleted ,
                                                                      NotificationMessage.ProfileCompleted() ,
                                                                      ct);

            await _cache.RemoveAsync(CacheKeys.User.UserById(user.Id) , ct);

            return Result.Updated;
        }
    }
}
