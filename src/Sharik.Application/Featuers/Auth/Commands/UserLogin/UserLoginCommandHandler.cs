using MediatR;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Auth.Dtos;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Notifications;
using Sharik.Domain.Notifications.Enums;

namespace Sharik.Application.Featuers.Auth.Commands.UserLogin
{
    public sealed class UserLoginCommandHandler(ILogger<UserLoginCommandHandler> _logger , IUserService _userService) : IRequestHandler<UserLoginCommand , Result<LoginUserDto>>
    {
        public async Task<Result<LoginUserDto>> Handle(UserLoginCommand request , CancellationToken ct)
        {

            var userLoginResult = await _userService.LoginAsync(request.email , request.password , ct);

            if (userLoginResult.IsFailure)
                return userLoginResult.Errors;

            var user = userLoginResult.Value;

            _logger.LogInformation("User with Email {Email} logged in successfully at {LoginTime}." ,
                                   user.Email ,
                                   DateTime.UtcNow);

           
            return user;
        }
    }
}
