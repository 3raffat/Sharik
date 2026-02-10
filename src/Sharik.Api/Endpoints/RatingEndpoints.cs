using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sharik.Api.Extensions;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Common.Responses;
using Sharik.Application.Featuers.Exchanges.RateExchanges;
using Sharik.Domain.Common.Results;

namespace Sharik.Api.Endpoints
{
    public static class RatingEndpoints
    {

        public static void MapRatingEndpoints(this IEndpointRouteBuilder app, ApiVersionSet set)
        {
            var endpoints = app.MapGroup("/api/v{version:apiVersion}/exchanges")
                .WithApiVersionSet(set)
                .HasApiVersion(1.0)
                .WithTags("User:Rating")
                .RequireAuthorization();

            endpoints.MapPost("{exchangeId:guid}/rate" , RateExchange)
                .WithSummary("Rate an exchange")
                .WithDescription("Submits a rating for a completed exchange");
        }

        private static async Task<IResult> RateExchange(ISender sender ,
                                                        IUser user ,
                                                        [FromRoute] Guid exchangeId,
                                                        [FromBody] RateExchangesRequest request ,
                                                        CancellationToken ct)
        {
            var result = await sender.Send(new RateExchangesCommand(exchangeId, user.UserId,request.ratedUserId, request.score, request.comment), ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<Created>(Data: value,
                Status: StatusCodes.Status200OK,
                Message: "Rating created successfully")),
                errors => errors.ToProblem());
        }
    }
}
