using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Sharik.Api.Hubs;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Notifications;
using static Sharik.Application.Common.Caching.CacheKeys;

namespace Sharik.Infrastructure.Services
{
    public sealed class NotificationService(ILogger<NotificationService> _logger , IHubContext<NotificationHub> _hubContext) : INotificationService
    {
        public async Task SendToUserAsync(Notification notification)
        {

            try
            {
                _logger.LogInformation("Sending notification to group user_{UserId}" , notification.UserId);

                await _hubContext.Clients
                    .Group($"user_{notification.UserId}")
                    .SendAsync("ReceiveNotification" , notification);

                _logger.LogInformation("Notification sent to user {UserId}" , notification.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex , "Error sending notification to user {UserId}" , notification.UserId);
            }

        }
    }
}
