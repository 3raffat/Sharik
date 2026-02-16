namespace Sharik.Application.Featuers.Chat.Dtos
{
    public record MessageDto(Guid Id , Guid ExchangeId , Guid SenderId , string Content , DateTime SentAt);

}
