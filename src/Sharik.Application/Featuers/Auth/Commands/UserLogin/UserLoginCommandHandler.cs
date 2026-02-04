using MediatR;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Auth.Dtos;
using Sharik.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.Auth.Commands.UserLogin
{
    public sealed class UserLoginCommandHandler(IUserService _userService) : IRequestHandler<UserLoginCommand, Result<LoginUserDto>>
    {
        public async Task<Result<LoginUserDto>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {

            var userLoginResult = await _userService.LoginAsync(request.email, request.password, cancellationToken);

            if(userLoginResult.IsFailure)
                return userLoginResult.Errors;

            var user = userLoginResult.Value;

            return user;
        }
    }
}
