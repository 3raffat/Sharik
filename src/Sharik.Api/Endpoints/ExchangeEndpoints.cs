using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sharik.Api.Extensions;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Common.Responses;
using Sharik.Application.Featuers.Exchanges.AcceptExchanges;
using Sharik.Application.Featuers.Exchanges.CancelleExchanges;
using Sharik.Application.Featuers.Exchanges.CompleteExchanges;
using Sharik.Application.Featuers.Exchanges.CreateExchanges;
using Sharik.Application.Featuers.Exchanges.CreateTeachingExchanges;
using Sharik.Application.Featuers.Exchanges.Dtos;
using Sharik.Application.Featuers.Exchanges.Queries.GetExchangesByProviderId;
using Sharik.Application.Featuers.Exchanges.RejectExchanges;
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

            group.MapPost("/swap" , CreateSwapExchange)
                 .WithSummary("Create a new exchange with swap type")
                 .WithDescription("Creates a new exchange between users");

            group.MapPost("/teaching" , CreateTeachingExchange)
                 .WithSummary("Create a new exchange with teaching type")
                 .WithDescription("Creates a new exchange between users");

            group.MapPut("{exchangeId:guid}/accept" , AcceptExchange)
                .WithSummary("Accept an exchange")
                .WithDescription("Accepts a pending exchange by its ID");

            group.MapPut("{exchangeId:guid}/cancel" , CancelleExchange)
                 .WithSummary("Cancel an exchange")
                 .WithDescription("Cancels an existing exchange by its ID");

            group.MapPut("{exchangeId:guid}/complete" , CompleteExchange)
                .WithName("CompleteExchange")
                .WithSummary("Complete an exchange")
                .WithDescription("Marks an exchange as completed by its ID");

            group.MapPut("{exchangeId:guid}/reject" , RejectExchange)
                .WithName("RejectExchange")
                .WithSummary("Reject an exchange")
                .WithDescription("Marks an exchange as rejected by its ID.");


            group.MapGet("" , GetExchanges)
                .WithSummary("Get all exchanges")
                .WithDescription("Retrieves a list of all exchanges for the authenticated user");
        }

        private static async Task<IResult> RejectExchange(ISender sender , IUser user , [FromRoute] Guid exchangeId , CancellationToken ct)
        {
            var result = await sender.Send(new RejectExchangeCommand(user.UserId , exchangeId) , ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<Updated>(Data: value ,
                Status: StatusCodes.Status200OK ,
                Message: "Exchange Rejected successfully")) ,
                errors => errors.ToProblem());
        }

        private static async Task<IResult> GetExchanges(ISender sender , IUser user , CancellationToken ct)
        {
            var result = await sender.Send(new GetExchangesByProviderIdQuery(user.UserId) , ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<List<ProviderExchangeDto>>(Data: value ,
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



        private async static Task<IResult> CreateSwapExchange(ISender sender ,
                                                    IUser user ,
                                                    [FromBody] CreateSwapExchangeRequest request ,
                                                    CancellationToken ct)
        {


            var result = await sender.Send(new CreateSwapExchangesCommand(user.UserId ,
                                                                      request.providerId ,
                                                                      request.skillOfferedId ,
                                                                      request.skillRequestedId ,
                                                                      request.requesterMessage) , ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<Success>(Data: value ,
                Status: StatusCodes.Status200OK ,
                Message: "Exchange Send successfully")) ,
                errors => errors.ToProblem());
        }

        private async static Task<IResult> CreateTeachingExchange(ISender sender ,
                                                   IUser user ,
                                                   [FromBody] CreateTeachingExchangeRequest request ,
                                                   CancellationToken ct)
        {


            var result = await sender.Send(new CreateTeachingExchangesCommand(user.UserId ,
                                                                              request.providerId ,
                                                                              request.skillRequestedId ,
                                                                              request.duration,
                                                                              request.requesterMessage) , ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<Success>(Data: value ,
                Status: StatusCodes.Status200OK ,
                Message: "Exchange Send successfully")) ,
                errors => errors.ToProblem());
        }
    }
}