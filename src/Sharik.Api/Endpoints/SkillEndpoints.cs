using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sharik.Api.Extensions;
using Sharik.Application.Common.Responses;
using Sharik.Application.Featuers.Skills.Commands.CreateSkill;
using Sharik.Application.Featuers.Skills.Commands.DeleteSkill;
using Sharik.Application.Featuers.Skills.Commands.UpdateSkill;
using Sharik.Application.Featuers.Skills.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Api.Endpoints
{
    public static class SkillEndpoints
    {
        public static void MapSkillEndpoints(this IEndpointRouteBuilder app, ApiVersionSet set)
        {
            var endpoints = app.MapGroup("/api/v{version:apiVersion}/skills")
                .WithApiVersionSet(set)
                .HasApiVersion(1.0)
                .WithTags("Admin:Skills");

            endpoints.MapPost("", CreateSkill);

            endpoints.MapDelete("{id:guid}", DeleteSkill);

            endpoints.MapPut("{id:guid}", UpdateSkill);

        }
        private static async Task<IResult> DeleteSkill(ISender sender, [FromRoute] Guid id, CancellationToken ct)
        {
            var result = await sender.Send(new DeleteSkillCommand(id), ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<Deleted>(Data: value,
                Status: StatusCodes.Status200OK,
                Message: "Skill deleted successfully")),
                errors => errors.ToProblem());
        }
        private static async Task<IResult> UpdateSkill(ISender sender, [FromRoute] Guid id, [FromBody] CreateSkillRequest request, CancellationToken ct)
        {
            var result = await sender.Send(new UpdateSkillCommand(id, request.Name, request.CategoryId), ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<SkillDto>(
                Data: value,
                Status: StatusCodes.Status200OK,
                Message: "Skill updated successfully")),
                errors => errors.ToProblem());

        }

        private static async Task<IResult> CreateSkill(ISender sender, [FromBody] CreateSkillRequest request, CancellationToken ct)
        {
            var result = await sender.Send(new CreateSkillCommand(request.CategoryId, request.Name), ct);


            return result.Match(value => Results.Ok(new StandardSuccessResponse<SkillDto>(Data: value,
                Status: StatusCodes.Status200OK,
                Message: "Skill created successfully")),
                errors => errors.ToProblem());

        }
    }
}
