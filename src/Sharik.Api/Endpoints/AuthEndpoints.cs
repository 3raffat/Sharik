
using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sharik.Api.Extensions;
using Sharik.Application.Common.Responses;
using Sharik.Application.Featuers.Auth.Commands.ConfirmEmail;
using Sharik.Application.Featuers.Auth.Commands.UserLogin;
using Sharik.Application.Featuers.Auth.Commands.UserRegister;
using Sharik.Application.Featuers.Auth.Dtos;
using Sharik.Domain.Common.Results;

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

            group.MapPost("/login" , Login)
                .WithSummary("User login")
                .WithDescription("Authenticates a user and returns a JWT token");

            group.MapPost("/register" , Register)
                   .WithSummary("User registration")
                   .WithDescription("Registers a new user account");

            group.MapPost("/confirm-email" , ConfirmEmail)
                .WithSummary("Confirm email")
                .WithDescription("Confirms a user's email address using a token");

        }

        private static async Task<IResult> ConfirmEmail(ISender _sender,string userId , string token)
        {

            var result = await _sender.Send(new ConfirmEmailCommand(userId , token));

                return result.Match(value => Results.Ok(new StandardSuccessResponse<Success>(Data: value ,
                    Status: StatusCodes.Status200OK ,
                    Message: "Email confirmed successfully! You can now log in to your account.")) ,
                    errors => errors.ToProblem());

            throw new NotImplementedException();
        }

        private static async Task<IResult> Register(ISender sender , [FromBody] UserRegisterRequest request)
        {
            var result = await sender.Send(new UserRegisterCommand(request.UserName , request.Email , request.Password));

            return result.Match(value => Results.Ok(new StandardSuccessResponse<Success>(Data: value ,
                Status: StatusCodes.Status200OK ,
                Message: "Account created successfully! Please check your email to confirm your account..")) ,
                errors => errors.ToProblem());
        }

        private static async Task<IResult> Login(ISender sender , [FromBody] UserLoginRequest request)
        {
            var result = await sender.Send(new UserLoginCommand(request.Email , request.Password));

            return result.Match(value => Results.Ok(new StandardSuccessResponse<LoginUserDto>(Data: value ,
                Status: StatusCodes.Status200OK ,
                Message: "User login successfully.")) ,
                errors => errors.ToProblem());
        }
    }
}
