using MediatR;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Auth.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Auth.Commands.UserRegister
{
    public sealed class UserRegisterCommandHandler(IUserService _userService) : IRequestHandler<UserRegisterCommand, Result<RegisterUserDto>>
    {
        public async Task<Result<RegisterUserDto>> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            var userRegisteResult = await _userService.RegisterAsync(request.username, request.email, request.password, cancellationToken);

            if (userRegisteResult.IsFailure)
                return userRegisteResult.Errors;

            var user = userRegisteResult.Value;

            return user;
        }
    }
}
