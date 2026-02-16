using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sharik.Api.Extensions;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Common.Responses;
using Sharik.Application.Featuers.Chat.Dtos;
using Sharik.Application.Featuers.Chat.Queries.GetMessages;

namespace Sharik.Api.Endpoints
{
    public static class ChatEndpoints
    {
        public static void MapChatEndpoints(this IEndpointRouteBuilder app , ApiVersionSet set)
        {
            var group = app.MapGroup("/api/v{version:ApiVersion}/exchanges/{exchangeId:guid}/messages")
                .WithApiVersionSet(set)
                .HasApiVersion(1.0)
                .RequireAuthorization()
                .WithTags("Chat");

            group.MapGet("" , GetMessages)
                .WithSummary("Get chat messages")
                .WithDescription("Retrieves all chat messages for a specific exchange");
        }

        private static async Task<IResult> GetMessages(
            ISender sender ,
            IUser user ,
            [FromRoute] Guid exchangeId ,
            CancellationToken ct)
        {
            var result = await sender.Send(new GetMessagesQuery(exchangeId , user.UserId) , ct);

            return result.Match(
                value => Results.Ok(new StandardSuccessResponse<List<MessageDto>>(
                    Data: value ,
                    Status: StatusCodes.Status200OK ,
                    Message: "Messages retrieved successfully")) ,
                errors => errors.ToProblem());
        }
    }
}
