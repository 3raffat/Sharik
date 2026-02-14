using Sharik.Domain.Common.Results;
using Sharik.Domain.Notifications;
using Sharik.Domain.Notifications.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Common.Interfaces
{
    public interface INotificationApplicationService
    {
        Task<Result<Notification>> CreateAndSendNotificationAsync(Guid userId ,
                                                                  NotificationType type ,
                                                                  string message,
                                                                  CancellationToken ct);
    }
}
