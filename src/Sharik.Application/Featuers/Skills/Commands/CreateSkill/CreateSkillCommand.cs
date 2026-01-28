using MediatR;
using Sharik.Application.Featuers.Skills.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Skills.Commands.CreateSkill
{
    public sealed record CreateSkillCommand(Guid CategoryId,
                                            string Name) : IRequest<Result<SkillDto>>;
}
