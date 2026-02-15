namespace Sharik.Application.Featuers.User.Dtos
{
    public record NotificationDto(Guid Id ,
                                     string Type ,
                                     string Message ,
                                     bool IsRead ,
                                     DateTime CreatedAt);
}
