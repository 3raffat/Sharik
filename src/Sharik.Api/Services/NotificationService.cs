using Microsoft.AspNetCore.SignalR;
using Sharik.Api.Hubs;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.User.Mapper;
using Sharik.Domain.Notifications;

namespace Sharik.Api.Services
{
    public sealed class NotificationService(ILogger<NotificationService> _logger , IHubContext<NotificationHub> _hubContext) : INotificationService
    {
        public async Task SendToUserAsync(Notification notification)
        {

            try
            {
                var groupName = $"user_{notification.UserId}";
                _logger.LogInformation("Sending notification to group {GroupName}" , groupName);

                var dto = notification.ToDto();

                await _hubContext.Clients
                    .Group(groupName)
                    .SendAsync("ReceiveNotification" , dto);

                _logger.LogInformation("Notification sent to user {UserId}" , notification.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex , "Error sending notification to user {UserId}" , notification.UserId);
            }

        }
    }
}
