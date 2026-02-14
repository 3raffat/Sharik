using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Notifications;
using Sharik.Domain.Notifications.Enums;

namespace Sharik.Application.Common.Services
{
    public class NotificationApplicationService(ILogger<NotificationApplicationService> _logger , IAppDbContext _context , INotificationService _notificationService) : INotificationApplicationService
    {
        public async Task<Result<Notification>> CreateAndSendNotificationAsync(Guid userId , NotificationType type , string message , CancellationToken ct)
        {

            var notificationResult = Notification.Create(userId , type , message);

            if (notificationResult.IsFailure)
                return notificationResult.Errors;


            var notification = notificationResult.Value;

            await _context.Notifications.AddAsync(notification , ct);

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Notification {NotificationId} created for user {UserId}" ,
                                   notification.Id ,
                                   userId);

            await _notificationService.SendToUserAsync(notification);

            return notification;
        }
    }
}
