using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sharik.Api.Extensions;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Common.Responses;
using Sharik.Application.Featuers.User.Commands.CompleteProfile;
using Sharik.Application.Featuers.User.Commands.UpdateProfile;
using Sharik.Application.Featuers.User.Dtos;
using Sharik.Application.Featuers.User.Queries.GetNotification;
using Sharik.Application.Featuers.User.Queries.GetProfile;
using Sharik.Domain.Common.Results;

namespace Sharik.Api.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app , ApiVersionSet set)
        {
            var endpoints = app.MapGroup("/api/v{version:ApiVersion}/users")
                .WithApiVersionSet(set)
                .HasApiVersion(1.0)
                .RequireAuthorization()
                .WithTags("Users");

            endpoints.MapPost("profile" , CompleteProfile)
                    .WithSummary("Complete user profile")
                    .WithDescription("Completes the user profile with additional information after registration");

            endpoints.MapPut("profile" , UpdateProfile)
                    .WithSummary("Update user profile")
                    .WithDescription("Updates the authenticated user's profile information");

            endpoints.MapGet("profile" , GetProfile)
                .WithSummary("Get user profile")
                .WithDescription("Retrieves the authenticated user's profile information");

            endpoints.MapGet("profile/{userId:guid}" , GetUserProfile)
                .WithSummary("Get user profile by ID")
                .WithDescription("Retrieves a user's profile information by their unique identifier");

            endpoints.MapGet("notification" , GetUserNotification)
                .WithSummary("Get user notifications")
                .WithDescription("Retrieves all notifications for the authenticated user.");


        }

        private static async Task<IResult> GetUserNotification(ISender sender ,
                                                               IUser _user ,
                                                               CancellationToken ct)
        {
            var result = await sender.Send(new GetNotificationQuery(_user.UserId) , ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<List<NotificationDto>>(Data: value ,
               Status: StatusCodes.Status200OK ,
               Message: "Notification retrieved successfully")) ,
               errors => errors.ToProblem());
        }

        private static async Task<IResult> GetUserProfile(ISender sender ,
                                                          [FromRoute] Guid userId ,
                                                          CancellationToken ct)
        {
            var result = await sender.Send(new GetProfileQuery(userId) , ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<CompleteUserProfileDto>(Data: value ,
               Status: StatusCodes.Status200OK ,
               Message: "Profile retrieved successfully")) ,
               errors => errors.ToProblem());

        }

        private static async Task<IResult> GetProfile(ISender sender ,
                                                         IUser user ,
                                                         CancellationToken ct)
        {
            var result = await sender.Send(new GetProfileQuery(user.UserId) , ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<CompleteUserProfileDto>(Data: value ,
               Status: StatusCodes.Status200OK ,
               Message: "Profile retrieved successfully")) ,
               errors => errors.ToProblem());

        }


        private static async Task<IResult> CompleteProfile(ISender sender ,
                                                           IUser user ,
                                                           [FromBody] CompleteProfileRequest request ,
                                                           CancellationToken ct)
        {
            var result = await sender.Send(new CompleteProfileCommand(user.UserId ,
                                                                      request.FirstName ,
                                                                      request.LastName ,
                                                                      request.Bio) , ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<Updated>(Data: value ,
               Status: StatusCodes.Status200OK ,
               Message: "complete profile successfully")) ,
               errors => errors.ToProblem());

        }

        private static async Task<IResult> UpdateProfile(ISender sender ,
                                                           IUser user ,
                                                           [FromBody] UpdateProfileRequest request ,
                                                           CancellationToken ct)
        {
            var result = await sender.Send(new UpdateProfileCommand(user.UserId ,
                                                                      request.FirstName ,
                                                                      request.LastName ,
                                                                      request.Bio) , ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<Updated>(Data: value ,
               Status: StatusCodes.Status200OK ,
               Message: "update profile successfully")) ,
               errors => errors.ToProblem());

        }
    }
}
