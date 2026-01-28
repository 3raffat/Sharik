using MediatR;
using Sharik.Application.Featuers.SkillCategories.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.SkillCategories.Commands.CreateSkillCategory
{
    public sealed record CreateSkillCategoryCommand(string Name) : IRequest<Result<SkillCategoryDto>>;

}
