using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Exchanges.Validation;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Exchanges;

namespace Sharik.Application.Featuers.Exchanges.CreateExchanges
{
    public sealed class CreateExchangesCommandHandler(
        ILogger<CreateExchangesCommandHandler> _logger ,
        IAppDbContext _context,IExchangeBusinessValidator _validator) : IRequestHandler<CreateExchangesCommand , Result<Success>>
    {
        public async Task<Result<Success>> Handle(CreateExchangesCommand request , CancellationToken ct)
        {

         var validationResult = await _validator.ValidateCreateExchangeAsync(request , ct);

            if(validationResult.IsFailure)
               return validationResult.Errors;

            var exchangeResult = Exchange.Create(request.requesterId ,
                request.providerId ,
                request.skillOfferedId ,
                request.skillRequestedId ,
                request.type ,
                request.duration ,
                request.pointsValue ,
                request.requesterMessage);


            if (exchangeResult.IsFailure)
                return exchangeResult.Errors;

            await _context.Exchanges.AddAsync(exchangeResult.Value , ct);

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Exchange created successfully with id {ExchangeId}" , exchangeResult.Value.Id);

            return Result.Success;
        }
    }
}
