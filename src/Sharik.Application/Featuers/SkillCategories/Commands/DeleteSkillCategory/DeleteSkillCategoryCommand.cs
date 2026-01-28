using MediatR;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.SkillCategories.Commands.DeleteSkillCategory
{
    public sealed record DeleteSkillCategoryCommand(Guid Id) : IRequest<Result<Deleted>>;

}
