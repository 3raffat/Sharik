using Sharik.Domain.Common;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Exchanges;

namespace Sharik.Domain.Messages
{
    public class Message : Entity
    {
        public Guid ExchangeId { get; private set; }
        public Exchange Exchange { get; private set; } = null!;

        public Guid SenderId { get; private set; }
        public string Content { get; private set; } = string.Empty;

        public DateTime SentAt { get; private set; }

        private Message() { }

        private Message(Guid id , Guid exchangeId , Guid senderId , string content) : base(id)
        {
            ExchangeId = exchangeId;
            SenderId = senderId;
            Content = content;
            SentAt = DateTime.UtcNow;
        }

        public static Result<Message> Create(Guid exchangeId , Guid senderId , string content)
        {

            if (exchangeId == Guid.Empty)
                return MessageErrors.ExchangeIdRequired;

            if (senderId == Guid.Empty)
                return MessageErrors.SenderIdRequired;

            if (string.IsNullOrWhiteSpace(content))
                return MessageErrors.MassageRequired;

            return new Message(Guid.NewGuid() , exchangeId , senderId , content);
        }
    }
}
