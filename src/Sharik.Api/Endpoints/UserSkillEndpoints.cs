using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sharik.Api.Extensions;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Common.Responses;
using Sharik.Application.Featuers.UserSkills.Commands.CreateUserSkill;
using Sharik.Application.Featuers.UserSkills.Commands.DeleteUserSkill;
using Sharik.Application.Featuers.UserSkills.Commands.UpdateUserSkill;
using Sharik.Application.Featuers.UserSkills.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Api.Endpoints
{
    public static class UserSkillEndpoints
    {

        public static void MapUserSkillEndpoints(this IEndpointRouteBuilder app, ApiVersionSet set)
        {

            var endpoints = app.MapGroup("/api/v{version:ApiVersion}/user-skills")
                .WithApiVersionSet(set)
                .HasApiVersion(1.0)
                .WithTags("User Skill")
                .RequireAuthorization();

            endpoints.MapPost("", CreateSkill);

            endpoints.MapPut("{skillId:guid}", UpdateSkill);

            endpoints.MapDelete("{skillId:guid}", DeleteSkill);

        }

        private static async Task<IResult> UpdateSkill(ISender sender,
                                                       [FromRoute] Guid skillId,
                                                       [FromBody] UpdateUserSkillRequest request,
                                                       IUser user,
                                                       CancellationToken ct)
        {
            var result = await sender.Send(new UpdateUserSkillCommand(user.UserId,
                                                                      skillId,
                                                                      request.skillLevel,
                                                                      request.pointPerHour), ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<Updated>(Data: value,
               Status: StatusCodes.Status200OK,
               Message: "User skill has been updated successfully.")),
               errors => errors.ToProblem());

        }

        private static async Task<IResult> DeleteSkill(ISender sender,
                                                       [FromRoute] Guid skillId,
                                                       IUser user,
                                                       CancellationToken ct)
        {
            var result = await sender.Send(new DeleteUserSkillCommand(user.UserId, skillId), ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<Deleted>(Data: value,
               Status: StatusCodes.Status200OK,
               Message: "User skill has been deleted successfully.")),
               errors => errors.ToProblem());

        }

        private static async Task<IResult> CreateSkill(ISender sender,
                                                       [FromBody] CreateUserSkillRequest request,
                                                       IUser user,
                                                       CancellationToken ct)
        {

            var result = await sender.Send(new CreateUserSkillCommand(user.UserId,
                                                                      request.skillId,
                                                                      request.skillLevel,
                                                                      request.pointPerHour), ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<UserSkillDto>(Data: value,
               Status: StatusCodes.Status200OK,
               Message: "User skill has been created successfully.")),
               errors => errors.ToProblem());

        }
    }
}
