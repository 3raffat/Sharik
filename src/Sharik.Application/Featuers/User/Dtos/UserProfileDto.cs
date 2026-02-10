using Sharik.Application.Featuers.UserSkills.Dtos;

namespace Sharik.Application.Featuers.User.Dtos
{
    public sealed record UserProfileDto(string FirstName , string LastName , string Bio);

    public sealed record CompleteUserProfileDto(string Username , string FullName , string Bio , int TotalPointsEarned , double Rating , List<UserSkillsDto> UserSkills);


}
