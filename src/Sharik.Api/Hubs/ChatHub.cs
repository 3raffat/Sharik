using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Chat.Commands.SendMassage;
using Sharik.Domain.Exchanges.Enums;
using System.Security.Claims;

namespace Sharik.Api.Hubs
{
    [Authorize]
    public sealed class ChatHub(ILogger<ChatHub> _logger , IAppDbContext _context , ISender _sender) : Hub
    {

        public async Task JoinExchangeChat(Guid exchangeId)
        {
            var userId = GetUserId();

            if (userId is null) return;

            var exchange = _context.Exchanges.SingleOrDefault(e => e.Id == exchangeId);

            if (exchange is null)
            {
                await Clients.Caller.SendAsync("ChatError" , "Exchange not found.");
                return;
            }

            var userGuid = Guid.Parse(userId);

            if (userGuid != exchange.RequesterId && userGuid != exchange.ProviderId)
            {
                await Clients.Caller.SendAsync("ChatError" , "You are not a participant of this exchange.");
                return;
            }

            if (exchange.ExchangeStatus != ExchangeStatus.Accepted)
            {
                await Clients.Caller.SendAsync("ChatError" , "Chat is only available for accepted exchanges.");
                return;
            }

            var groupName = $"exchange_{exchangeId}";

            await Groups.AddToGroupAsync(Context.ConnectionId , groupName);

            _logger.LogInformation("User {UserId} joined chat group {GroupName}" , userId , groupName);

            await Clients.Caller.SendAsync("JoinedChat" , exchangeId);

        }

        public async Task LeaveExchangeChat(Guid exchangeId)
        {
            var userId = GetUserId();
            if (userId is null) return;

            var groupName = $"exchange_{exchangeId}";

            await Groups.RemoveFromGroupAsync(Context.ConnectionId , groupName);

            _logger.LogInformation("User {UserId} left chat group {GroupName}" , userId , groupName);

        }
        public async Task SendMessage(Guid exchangeId , string content)
        {
            var userId = GetUserId();
            if (userId is null) return;

            var userGuid = Guid.Parse(userId);

            var result = await _sender.Send(new SendMessageCommand(exchangeId , userGuid , content));

            if (result.IsFailure)
            {
                var errorMsg = string.Join(", " , result.Errors.Select(e => e.Description));
                await Clients.Caller.SendAsync("ChatError" , errorMsg);
                return;
            }

            var dto = result.Value;
            var groupName = $"exchange_{exchangeId}";

            await Clients.Group(groupName).SendAsync("ReceiveChatMessage" , dto);

            _logger.LogInformation("Message {MessageId} broadcast to group {GroupName}" , dto.Id , groupName);
        }


        private string? GetUserId()
        {
            return Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
