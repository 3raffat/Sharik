using MediatR;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.User.Commands.CompleteProfile
{
    public sealed record CompleteProfileCommand(Guid userId, string firstName, string lastName, string bio) : IRequest<Result<Updated>>;


}
