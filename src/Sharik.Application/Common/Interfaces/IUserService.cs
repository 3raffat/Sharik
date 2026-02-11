using Sharik.Application.Featuers.Auth.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<Result<LoginUserDto>> LoginAsync(string email , string password , CancellationToken ct);
        Task<Result<Success>> RegisterAsync(string username , string email , string password , CancellationToken ct);
        Task<Result<Success>> ConfirmEmailAsync(string userId , string token , CancellationToken ct);

    }
}
