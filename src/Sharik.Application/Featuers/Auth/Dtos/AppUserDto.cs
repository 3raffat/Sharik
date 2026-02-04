using System.Security.Claims;

namespace Sharik.Application.Featuers.Auth.Dtos
{
    public sealed record AppUserDto(string UserId, string UserEmail, IList<string> Roles, IList<Claim> Claims);
    public sealed record UserLoginRequest(string Email, string Password);
    public sealed record UserRegisterRequest(string UserName,string Email, string Password);

}
