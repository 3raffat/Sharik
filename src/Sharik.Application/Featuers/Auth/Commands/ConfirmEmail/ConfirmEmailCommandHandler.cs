using MediatR;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Notifications;
using Sharik.Domain.Notifications.Enums;

namespace Sharik.Application.Featuers.Auth.Commands.ConfirmEmail
{
    public sealed class ConfirmEmailCommandHandler(
        ILogger<ConfirmEmailCommandHandler> _logger ,
        IUserService _service ,
        INotificationApplicationService _notificationService) : IRequestHandler<ConfirmEmailCommand , Result<Success>>
    {
        public async Task<Result<Success>> Handle(ConfirmEmailCommand request , CancellationToken ct)
        {
            var result = await _service.ConfirmEmailAsync(request.userId , request.token , ct);

            if (result.IsFailure)
                return result.Errors;

            _logger.LogInformation("Email confirmation completed for UserId {UserId} at {ConfirmedAt}." ,
                                   request.userId ,
                                   DateTime.UtcNow);

            var userId = Guid.Parse(request.userId);

            await _notificationService.CreateAndSendNotificationAsync(userId ,
                                                                      NotificationType.WelcomePoints ,
                                                                      NotificationMessage.WelcomePoints() ,
                                                                      ct);

            return Result.Success;
        }
    }
}
