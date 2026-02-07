using MediatR;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.SkillCategories.Commands.DeleteSkill
{
    public sealed record DeleteSkillCommand(Guid SkillId, Guid CategoryId) : IRequest<Result<Deleted>>;


}
