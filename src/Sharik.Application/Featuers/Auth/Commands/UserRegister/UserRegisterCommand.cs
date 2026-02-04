using MediatR;
using Sharik.Application.Featuers.Auth.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Auth.Commands.UserRegister
{
    public sealed record UserRegisterCommand(string username, string email, string password):IRequest<Result<RegisterUserDto>>;

}
