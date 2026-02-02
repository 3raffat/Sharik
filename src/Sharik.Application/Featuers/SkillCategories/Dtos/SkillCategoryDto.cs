namespace Sharik.Application.Featuers.SkillCategories.Dtos
{
    public sealed record SkillCategoryDto(Guid Id, string Name);
    public sealed record CreateCategoryRequest(string Name);
    public sealed record UpdateCategoryRequest(string Name);

}
