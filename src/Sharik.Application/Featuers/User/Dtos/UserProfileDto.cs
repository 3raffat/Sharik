using Sharik.Application.Featuers.UserSkills.Dtos;

namespace Sharik.Application.Featuers.User.Dtos
{
    public sealed record UserProfileDto(string FirstName , string LastName , string Bio);

    public sealed record CompleteUserProfileDto(string Username , string FullName , string Bio , int TotalPointsEarned , double AverageRating , List<UserSkillsDto> UserSkills , List<RatingDto> ratings);

    public sealed record RatingDto(string raterName , int score , string? comment);

    public sealed record UserExchange(string name);

}
