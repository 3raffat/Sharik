using Sharik.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Domain.Notifications
{
    public static class NotificationErrors
    {

        public static Error UserIdRequired
                => Error.Validation(
                    code: "Notification.UserId.Required" ,
                    description: "User ID ."
                );

        public static Error MassageRequired
                => Error.Validation(
                 code: "Notification.Massage.Required",
                 description: "Massage cannot be empty."
                );

        public static Error InvalidNotificationType => Error.Validation(
                  code: "Notification.Type.Invalid" ,
                  description: "Notification type is invalid."
              );


    }
}
