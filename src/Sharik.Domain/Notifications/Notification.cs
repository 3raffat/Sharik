using Sharik.Domain.Common;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Notifications.Enums;
using Sharik.Infrastructure.Auth;

namespace Sharik.Domain.Notifications
{
    public sealed class Notification : Entity
    {

        public Guid UserId { get; private set; }
        public AppUser User { get; private set; } = null!;

        public NotificationType Type { get; private set; }

        public string Message { get; private set; } = string.Empty; 

        public bool IsRead { get; private set; }

        public DateTime CreatedAt { get; private set; }

        private Notification() { }

        private Notification(Guid Id , Guid userId , NotificationType type , string message) : base(Id)
        {
            UserId = userId;
            Type = type;
            Message = message;
            CreatedAt = DateTime.UtcNow;
            IsRead = false;
        }

        public static Result<Notification> Create(Guid userId , NotificationType type , string message)
        {

            if (userId == Guid.Empty)
                return NotificationErrors.UserIdRequired;

            if (!Enum.IsDefined(type))
                return NotificationErrors.InvalidNotificationType;

            if (string.IsNullOrWhiteSpace(message))
                return NotificationErrors.MassageRequired;

            return new Notification(Guid.NewGuid() , userId , type , message);
        }

        public void MarkAsRead() => IsRead = true;
    }

}
