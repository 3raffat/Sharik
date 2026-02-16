using Sharik.Application.Featuers.Chat.Dtos;
using Sharik.Domain.Messages;

namespace Sharik.Application.Featuers.Chat.Mapper
{
    public static class MessageMapper
    {

        extension(Message message)
        {
            public MessageDto ToDto()
                => new(message.Id , message.ExchangeId , message.SenderId , message.Content , message.SentAt);

        }
    }
}
