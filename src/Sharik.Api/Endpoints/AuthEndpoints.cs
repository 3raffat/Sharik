
using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sharik.Api.Extensions;
using Sharik.Application.Common.Responses;
using Sharik.Application.Featuers.Auth.Commands.UserLogin;
using Sharik.Application.Featuers.Auth.Commands.UserRegister;
using Sharik.Application.Featuers.Auth.Dtos;

namespace Sharik.Api.Endpoints
{
    public static class AuthEndpoints
    {

        public static void MapAuthEndpoints(this IEndpointRouteBuilder app , ApiVersionSet set)
        {
            var group = app.MapGroup("/auth/v{version:ApiVersion}")
                .WithApiVersionSet(set)
                .HasApiVersion(1.0)
                .WithTags("Authentication");

            group.MapPost("/login", Login)
                .WithSummary("User login")
                .WithDescription("Authenticates a user and returns a JWT token");

            group.MapPost("/register", Register)
                   .WithSummary("User registration")
                   .WithDescription("Registers a new user account");

        }

        private static async Task<IResult> Register(ISender sender, [FromBody] UserRegisterRequest request)
        {
            var result = await sender.Send(new UserRegisterCommand(request.UserName,request.Email, request.Password));

            return result.Match(value => Results.Ok(new StandardSuccessResponse<RegisterUserDto>(Data: value,
                Status: StatusCodes.Status200OK,
                Message: "User registered successfully.")),
                errors => errors.ToProblem());
        }

        private static async Task<IResult> Login(ISender sender, [FromBody] UserLoginRequest request)
        {
            var result = await sender.Send(new UserLoginCommand(request.Email, request.Password));

            return result.Match(value => Results.Ok(new StandardSuccessResponse<LoginUserDto>(Data: value,
                Status: StatusCodes.Status200OK,
                Message: "User login successfully.")),
                errors => errors.ToProblem());
        }
    }
}
