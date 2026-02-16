using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Chat.Dtos;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Exchanges;

namespace Sharik.Application.Featuers.Chat.Queries.GetMessages
{
    public sealed class GetMessagesQueryHandler(
          ILogger<GetMessagesQueryHandler> _logger ,
          IAppDbContext _context) : IRequestHandler<GetMessagesQuery , Result<List<MessageDto>>>
    {
        public async Task<Result<List<MessageDto>>> Handle(GetMessagesQuery request , CancellationToken ct)
        {
            var exchange = await _context.Exchanges
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.ExchangeId , ct);

            if (exchange is null)
            {
                _logger.LogWarning("Exchange {ExchangeId} not found." , request.ExchangeId);
                return ApplicationErrors.ExchangeNotFound;
            }

            if (request.UserId != exchange.RequesterId && request.UserId != exchange.ProviderId)
                return ExchangeErrors.Unauthorized;

            var messages = await _context.Messages
                .AsNoTracking()
                .Where(m => m.ExchangeId == request.ExchangeId)
                .OrderBy(m => m.SentAt)
                .Select(m => new MessageDto(m.Id , m.ExchangeId , m.SenderId , m.Content , m.SentAt))
                .ToListAsync(ct);

            _logger.LogInformation("Retrieved {Count} messages for exchange {ExchangeId}" ,
                messages.Count , request.ExchangeId);

            return messages;
        }
    }
}
