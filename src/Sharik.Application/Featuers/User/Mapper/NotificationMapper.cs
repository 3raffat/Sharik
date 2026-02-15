using Sharik.Application.Featuers.User.Dtos;
using Sharik.Domain.Notifications;

namespace Sharik.Application.Featuers.User.Mapper
{
    public static class NotificationMapper
    {

        extension(Notification notification)
        {
            public NotificationDto ToDto()
                => new(notification.Id , notification.Type.ToString() , notification.Message , notification.IsRead , notification.CreatedAt);
        }
    }
}
