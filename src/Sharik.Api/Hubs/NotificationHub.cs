using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Sharik.Application.Common.Interfaces;
using System.Security.Claims;

namespace Sharik.Api.Hubs
{
    [Authorize]
    public class NotificationHub(ILogger<NotificationHub> _logger) : Hub
    {

        public async override Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                var groupName = $"user_{userId}";
                await Groups.AddToGroupAsync(Context.ConnectionId , groupName);
                _logger.LogInformation("User {UserId} added to group {GroupName}" , userId , groupName);
            }

            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                var groupName = $"user_{userId}";
                await Groups.RemoveFromGroupAsync(Context.ConnectionId , groupName);
                _logger.LogInformation("User {UserId} removed from group {GroupName} (ConnectionId: {ConnectionId})" ,
                              userId , groupName , Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
