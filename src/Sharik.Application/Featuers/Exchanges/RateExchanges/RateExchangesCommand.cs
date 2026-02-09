using MediatR;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Exchanges.RateExchanges
{
    public sealed record RateExchangesCommand(Guid exchangeId , Guid raterId , Guid ratedUserId , int score , string? comment) : IRequest<Result<Created>>;

    public sealed record RateExchangesRequest(Guid ratedUserId , int score , string? comment);
}
