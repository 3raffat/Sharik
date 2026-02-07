namespace Sharik.Application.Featuers.User.Dtos
{
    public sealed record CompleteProfileRequest(string FirstName, string LastName, string Bio);
    public sealed record UpdateProfileRequest(string FirstName, string LastName, string Bio);


}
