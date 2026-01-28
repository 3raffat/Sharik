using MediatR;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Skills.Commands.DeleteSkill
{
    public sealed record DeleteSkillCommand(Guid SkillId):IRequest<Result<Deleted>>;


}
