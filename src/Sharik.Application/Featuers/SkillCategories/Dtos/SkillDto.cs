namespace Sharik.Application.Featuers.SkillCategories.Dtos
{
    public sealed record SkillDto(Guid Id, string Name);
    public sealed record CreateSkillRequest(string Name);
    public sealed record UpdateSkillRequest(string Name);

}
