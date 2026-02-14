using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Sharik.Application.Common.Interfaces;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Sharik.Api.Hubs
{
    [Authorize]
    public class NotificationHub(ILogger<NotificationHub> _logger , IUser _user) : Hub
    {
        public async override Task OnConnectedAsync()
        {
            var userId = _user.Id;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId , $"user_{userId}");
                _logger.LogInformation("User {UserId} connected" , userId);

            }

            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {

            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId , $"user_{userId}");
                _logger.LogInformation("User {UserId} disconnected" , userId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
