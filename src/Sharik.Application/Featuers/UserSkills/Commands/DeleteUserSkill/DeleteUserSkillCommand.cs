using MediatR;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.UserSkills.Commands.DeleteUserSkill
{
    public sealed record DeleteUserSkillCommand(Guid UserId, Guid SkillId) : IRequest<Result<Deleted>>;

}
