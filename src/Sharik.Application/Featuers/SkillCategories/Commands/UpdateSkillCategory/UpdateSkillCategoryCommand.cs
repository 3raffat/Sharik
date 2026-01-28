using MediatR;
using Sharik.Application.Featuers.SkillCategories.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.SkillCategories.Commands.UpdateSkillCategory
{
    public sealed record UpdateSkillCategoryCommand(Guid Id, string Name):IRequest<Result<SkillCategoryDto>>;
}
