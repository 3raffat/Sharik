namespace Sharik.Application.Featuers.Auth.Dtos
{
    public sealed record AppUserDto(string UserId, string UserEmail, List<string> Roles, List<string> Claims);

}
