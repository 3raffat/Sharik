using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Notifications;
using Sharik.Domain.Notifications.Enums;

namespace Sharik.Application.Featuers.Exchanges.RateExchanges
{
    public sealed class RateExchangesCommandHandler(
        ILogger<RateExchangesCommandHandler> _logger ,
        IAppDbContext _context , INotificationApplicationService _notificationService) : IRequestHandler<RateExchangesCommand , Result<Created>>
    {
        public async Task<Result<Created>> Handle(RateExchangesCommand request , CancellationToken ct)
        {
            var exchange = await _context.Exchanges
                .Include(e => e.Ratings).FirstOrDefaultAsync(e => e.Id == request.exchangeId , ct);
            if (exchange is null)
            {
                _logger.LogWarning("Exchange with id {ExchangeId} not found for rating" , request.exchangeId);

                return ApplicationErrors.ExchangeNotFound;
            }

            var ratingResult = exchange.RateExchange(request.raterId ,
                                                     request.ratedUserId ,
                                                     request.score ,
                                                     request.comment);

            if (ratingResult.IsFailure)
                return ratingResult.Errors;

            await _context.Ratings.AddAsync(ratingResult.Value , ct);

            var rater = await _context.Users.FirstOrDefaultAsync(r => r.Id == request.raterId);
            var ratedUser = await _context.Users.FirstOrDefaultAsync(r=>r.Id==request.ratedUserId);

            rater.GiveRating(ratingResult.Value);
            ratedUser.ReceiveRating(ratingResult.Value);

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Exchange with id {ExchangeId} rated successfully by user {RaterId}" , request.exchangeId , request.raterId);


            await _notificationService.CreateAndSendNotificationAsync(request.ratedUserId ,
                                                                      NotificationType.NewRating ,
                                                                      NotificationMessage.NewRating(request.score) ,
                                                                      ct);

            return Result.Created;
        }
    }
}
