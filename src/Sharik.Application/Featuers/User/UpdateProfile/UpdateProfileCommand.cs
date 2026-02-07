using MediatR;
using Sharik.Application.Featuers.User.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.User.UpdateProfile
{
    public sealed record UpdateProfileCommand(Guid userId, string FirstName, string LastName, string Bio) : IRequest<Result<Updated>>;


}
