using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Chat.Dtos;
using Sharik.Application.Featuers.Chat.Mapper;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Chat.Commands.SendMassage
{
    public sealed class SendMessageCommandHandler(
        ILogger<SendMessageCommandHandler> _logger ,
        IAppDbContext _context , HybridCache _cache) : IRequestHandler<SendMessageCommand , Result<MessageDto>>
    {
        public async Task<Result<MessageDto>> Handle(SendMessageCommand request , CancellationToken ct)
        {
            var exchange = await _context.Exchanges
                .FirstOrDefaultAsync(e => e.Id == request.ExchangeId , ct);

            if (exchange is null)
            {
                _logger.LogWarning("Exchange {ExchangeId} not found." , request.ExchangeId);
                return ApplicationErrors.ExchangeNotFound;
            }

            var msgResult = exchange.AddMessage(request.SenderId , request.Content);

            if (msgResult.IsFailure)
                return msgResult.Errors;

            var message = msgResult.Value;

            await _context.Messages.AddAsync(message , ct);

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Message {MessageId} sent in exchange {ExchangeId} by user {SenderId}" ,
                message.Id , request.ExchangeId , request.SenderId);

            await _cache.RemoveAsync(CacheKeys.Message.MessagesByExchangeId(request.ExchangeId) , ct);

            return message.ToDto();
        }
    }
}
