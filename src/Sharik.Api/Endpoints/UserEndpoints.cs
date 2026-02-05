using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sharik.Api.Extensions;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Common.Responses;
using Sharik.Application.Featuers.User.CompleteProfile;
using Sharik.Application.Featuers.User.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Api.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app, ApiVersionSet set)
        {
            var endpoints = app.MapGroup("/api/v{version:ApiVersion}/users")
                .WithApiVersionSet(set)
                .HasApiVersion(1.0)
                .RequireAuthorization()
                .WithTags("Users");

            endpoints.MapPost("profile", CompleteProfile);

        }

        private static async Task<IResult> CompleteProfile(ISender sender, IUser user, [FromBody] CompleteProfileRequest request, CancellationToken ct)
        {
            var result = await sender.Send(new CompleteProfileCommand(user.UserId,
                                                                      request.FirstName,
                                                                      request.LastName,
                                                                      request.Bio), ct);
           
            return result.Match(value => Results.Ok(new StandardSuccessResponse<Updated>(Data: value,
               Status: StatusCodes.Status200OK,
               Message: "complete profile successfully")),
               errors => errors.ToProblem());

        }
    }
}
