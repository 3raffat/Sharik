using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sharik.Api.Extensions;
using Sharik.Application.Common.Responses;
using Sharik.Application.Featuers.SkillCategories.Commands.CreateSkillCategory;
using Sharik.Application.Featuers.SkillCategories.Commands.DeleteSkillCategory;
using Sharik.Application.Featuers.SkillCategories.Commands.UpdateSkillCategory;
using Sharik.Application.Featuers.SkillCategories.Dtos;
using Sharik.Domain.Common.Results;
using Sharik.Domain.User.Enums;

namespace Sharik.Api.Endpoints
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(this IEndpointRouteBuilder app, ApiVersionSet set)
        {
            var endpoints = app.MapGroup("/api/v{version:apiVersion}/categories")
                .WithApiVersionSet(set)
                .HasApiVersion(1.0)
                .WithTags("Admin:Category")
                .RequireAuthorization(policy =>
                   policy.RequireRole(nameof(Role.Admin),nameof(Role.SuperAdmin)));

            endpoints.MapPost("", CreateCategory)
                 .WithSummary("Create a new category")
                 .WithDescription("Creates a new category in the system");

            endpoints.MapDelete("{categoryId:guid}", DeleteCategory)
                .WithSummary("Delete a category")
                .WithDescription("Deletes an existing category by its ID");

            endpoints.MapPut("{categoryId:guid}", UpdateCategory)
                .WithSummary("Update a category")
                .WithDescription("Updates an existing category by its ID");

        }

        private static async Task<IResult> UpdateCategory([FromRoute] Guid categoryId,
                                                          [FromBody] UpdateCategoryRequest request,
                                                          ISender sender,
                                                          CancellationToken ct)
        {

            var result = await sender.Send(new UpdateSkillCategoryCommand(categoryId,
                                                                          request.Name), ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<SkillCategoryDto>(Data: value,
                Status: StatusCodes.Status200OK,
                Message: "Category updated successfully")),
                errors => errors.ToProblem());

        }

        private static async Task<IResult> DeleteCategory([FromRoute] Guid categoryId,
                                                          ISender sender,
                                                          CancellationToken ct)
        {

            var result = await sender.Send(new DeleteSkillCategoryCommand(categoryId), ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<Deleted>(Data: value,
                Status: StatusCodes.Status200OK,
                Message: "Category deleted successfully")),
                errors => errors.ToProblem());

        }

        private static async Task<IResult> CreateCategory([FromBody] CreateCategoryRequest request,
                                                          ISender sender,
                                                          CancellationToken ct)
        {

            var result = await sender.Send(new CreateSkillCategoryCommand(request.Name), ct);

            return result.Match(value => Results.Ok(new StandardSuccessResponse<SkillCategoryDto>(Data: value,
                Status: StatusCodes.Status200OK,
                Message: "Category created successfully")),
                errors => errors.ToProblem());

        }
    }
}
