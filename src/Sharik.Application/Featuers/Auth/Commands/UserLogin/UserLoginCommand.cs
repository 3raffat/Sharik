using MediatR;
using Sharik.Application.Featuers.Auth.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Auth.Commands.UserLogin
{
    public sealed record UserLoginCommand(string email, string password):IRequest<Result<LoginUserDto>>;

}
