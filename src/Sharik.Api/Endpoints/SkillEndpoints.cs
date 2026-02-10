using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sharik.Api.Extensions;
using Sharik.Application.Common.Responses;
using Sharik.Application.Featuers.SkillCategories.Commands.CreateSkill;
using Sharik.Application.Featuers.SkillCategories.Commands.DeleteSkill;
using Sharik.Application.Featuers.SkillCategories.Commands.UpdateSkill;
using Sharik.Application.Featuers.SkillCategories.Dtos;
using Sharik.Application.Featuers.SkillCategories.Queries.GetSkillsQuery;
using Sharik.Domain.Common.Results;
using Sharik.Domain.User.Enums;
using System.Reflection;
using static Sharik.Application.Common.Caching.CacheKeys;

namespace Sharik.Api.Endpoints
{
    public static class SkillEndpoints
    {
        public static void MapSkillEndpoints(this IEndpointRouteBuilder app, ApiVersionSet set)
        {
            var endpoints = app.MapGroup("/api/v{version:apiVersion}")
                .WithApiVersionSet(set)
                .HasApiVersion(1.0)
                .WithTags("Admin:Skill")
                .RequireAuthorization(policy =>
                   policy.RequireRole(nameof(Role.Admin), nameof(Role.SuperAdmin)));


            endpoints.MapPost("/categories{categoryId:guid}/skills" , CreateSkill);

            endpoints.MapDelete("/categories{categoryId:guid}/skills/{skillId:guid}" , DeleteSkill);

            endpoints.MapPut("/categories{categoryId:guid}/skills/{skillId:guid}" , UpdateSkill);

            endpoints.MapGet("/skills", GetSkills)
                .WithTags("Public:Skill")
                .AllowAnonymous();

        }

        private static async Task<IResult> GetSkills(ISender sender ,CancellationToken ct)
        {

            var result = await sender.Send(new GetSkillsQuery() , ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<List<SkillDto>>(Data: value ,
                Status: StatusCodes.Status200OK ,
                Message: "Skills retrieved successfully")) ,
                errors => errors.ToProblem());
        }

        private static async Task<IResult> DeleteSkill(ISender sender,
                                                       [FromRoute] Guid skillId,
                                                       [FromRoute] Guid categoryId,
                                                       CancellationToken ct)
        {

            var result = await sender.Send(new DeleteSkillCommand(skillId, categoryId), ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<Deleted>(Data: value,
                Status: StatusCodes.Status200OK,
                Message: "Skill deleted successfully")),
                errors => errors.ToProblem());

        }

        private static async Task<IResult> UpdateSkill(ISender sender,
                                                       [FromRoute] Guid skillId,
                                                       [FromRoute] Guid categoryId,
                                                       [FromBody] UpdateSkillRequest request,
                                                       CancellationToken ct)
        {

            var result = await sender.Send(new UpdateSkillCommand(skillId,
                                                                  request.Name,
                                                                  categoryId), ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<Updated>(
                Data: value,
                Status: StatusCodes.Status200OK,
                Message: "Skill updated successfully")),
                errors => errors.ToProblem());

        }

        private static async Task<IResult> CreateSkill(ISender sender,
                                                       [FromRoute] Guid categoryId,
                                                       [FromBody] CreateSkillRequest request,
                                                       CancellationToken ct)
        {

            var result = await sender.Send(new CreateSkillCommand(categoryId,
                                                                  request.Name), ct);


            return result.Match(value => Results.Ok(new StandardSuccessResponse<SkillDto>(Data: value,
                Status: StatusCodes.Status200OK,
                Message: "Skill created successfully")),
                errors => errors.ToProblem());

        }
    }
}
