using Sharik.Domain.Common.Results;
using Sharik.Domain.Notifications;
using Sharik.Domain.Notifications.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Common.Interfaces
{
    public interface INotificationService
    {

        Task SendToUserAsync(Notification notification);

        //Task<Result<Notification>> CreateSaveAndSendAsync(Guid userId ,
        //                                              string message ,
        //                                              NotificationType type ,
        //                                              CancellationToken ct = default);

    }
}
