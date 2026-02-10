using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sharik.Api.Extensions;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Common.Responses;
using Sharik.Application.Featuers.Exchanges.AcceptExchanges;
using Sharik.Application.Featuers.Exchanges.AcceptExchanges.CompleteExchanges;
using Sharik.Application.Featuers.Exchanges.CancelleExchanges;
using Sharik.Application.Featuers.Exchanges.CreateExchanges;
using Sharik.Application.Featuers.Exchanges.Dtos;
using Sharik.Application.Featuers.Exchanges.Queries.GetExchanges;
using Sharik.Domain.Common.Results;

namespace Sharik.Api.Endpoints
{
    public static class ExchangeEndpoints
    {

        public static void MapExchangeEndpoints(this IEndpointRouteBuilder app , ApiVersionSet set)
        {
            var group = app.MapGroup("/api/v{version:ApiVersion}/exchanges")
                .WithApiVersionSet(set)
                .HasApiVersion(1.0)
                .RequireAuthorization()
                .WithTags("Exchanges");

            group.MapPost("" , CreateExchange);

            group.MapPut("{exchangeId:guid}/Accept" , AcceptExchange);

            group.MapPut("{exchangeId:guid}/Cancel" , CancelleExchange);

            group.MapPut("{exchangeId:guid}/Complete" , CompleteExchange);

            group.MapGet("" , GetExchanges)
                .AllowAnonymous();
        }

        private static async Task<IResult> GetExchanges(ISender sender , CancellationToken ct)
        {
            var result = await sender.Send(new GetExchangesQuery() , ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<List<ExchangeDto>>(Data: value ,
                Status: StatusCodes.Status200OK ,
                Message: "Exchange retrieved successfully")) ,
                errors => errors.ToProblem());
        }

        private static async Task<IResult> AcceptExchange(ISender sender , IUser user , [FromRoute] Guid exchangeId , CancellationToken ct)
        {
            var result = await sender.Send(new AcceptExchangesCommand(exchangeId , user.UserId) , ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<Updated>(Data: value ,
                Status: StatusCodes.Status200OK ,
                Message: "Exchange Accepted successfully")) ,
                errors => errors.ToProblem());
        }


        private static async Task<IResult> CancelleExchange(ISender sender ,
                                                            IUser user ,
                                                            [FromRoute] Guid exchangeId ,
                                                            [FromBody] CancelleExchangesRequest request ,
                                                            CancellationToken ct)
        {
            var result = await sender.Send(new CancelleExchangesCommand(user.UserId , exchangeId , request.cancellationReason) , ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<Updated>(Data: value ,
                Status: StatusCodes.Status200OK ,
                Message: "Exchange Cancelle successfully")) ,
                errors => errors.ToProblem());
        }


        private static async Task<IResult> CompleteExchange(ISender sender ,
                                                    IUser user ,
                                                    [FromRoute] Guid exchangeId ,
                                                    CancellationToken ct)
        {
            var result = await sender.Send(new CompleteExchangesCommand(user.UserId , exchangeId) , ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<Updated>(Data: value ,
                Status: StatusCodes.Status200OK ,
                Message: "Exchange complete successfully")) ,
                errors => errors.ToProblem());
        }



        private async static Task<IResult> CreateExchange(ISender sender ,
                                                    IUser user ,
                                                    [FromBody] CreateExchangeRequest request ,
                                                    CancellationToken ct)
        {


            var result = await sender.Send(new CreateExchangesCommand(user.UserId ,
                                                                      request.providerId ,
                                                                      request.skillOfferedId ,
                                                                      request.skillRequestedId ,
                                                                      request.type ,
                                                                      request.duration ,
                                                                      request.pointsValue ,
                                                                      request.requesterMessage) , ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<Success>(Data: value ,
                Status: StatusCodes.Status200OK ,
                Message: "Exchange Send successfully")) ,
                errors => errors.ToProblem());
        }


    }
}