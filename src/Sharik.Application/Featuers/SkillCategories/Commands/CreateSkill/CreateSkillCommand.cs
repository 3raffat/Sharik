using MediatR;
using Sharik.Application.Featuers.SkillCategories.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.SkillCategories.Commands.CreateSkill
{
    public sealed record CreateSkillCommand(Guid CategoryId, string Name) : IRequest<Result<SkillDto>>;
}
