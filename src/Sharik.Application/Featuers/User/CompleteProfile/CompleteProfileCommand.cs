using MediatR;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.User.CompleteProfile
{
    public sealed record CompleteProfileCommand(Guid userId, string firstName, string lastName, string bio) : IRequest<Result<Updated>>;


}
