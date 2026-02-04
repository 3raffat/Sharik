using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Auth.Dtos;
using Sharik.Domain.Auth;
using Sharik.Domain.Common.Results;
using Sharik.Domain.User.Enums;

namespace Sharik.Infrastructure.Auth
{
    public sealed class UserService(ILogger<UserService> _logger, UserManager<AppUser> _manager, ITokenProvider _token) : IUserService
    {

        public async Task<Result<LoginUserDto>> LoginAsync(string email, string password, CancellationToken ct)
        {

            var user = await _manager.FindByEmailAsync(email);

            if (user is null)
            {
                _logger.LogWarning("Login attempt failed: User with email {Email} not found.", email);
                return AuthErrors.InvalidEmailOrPassword;
            }

            var validPassword = await _manager.CheckPasswordAsync(user, password);

            if (!validPassword)
            {
                _logger.LogWarning("Login attempt failed: Invalid password for user with email {Email}.", email);
                return AuthErrors.InvalidEmailOrPassword;
            }

            var userInfo = await GetUserInfoAsync(user);

            var tokenResult = await _token.GenerateJwtTokenAsync(userInfo, ct);

            if (tokenResult.IsFailure)
            {
                _logger.LogError("Token generation failed for user with email {Email}.", email);
                return tokenResult.Errors;
            }


            return new LoginUserDto(email, tokenResult.Value);
        }

        public async Task<Result<RegisterUserDto>> RegisterAsync(string username, string email, string password, CancellationToken ct)
        {
            var existingUsername = await _manager.FindByNameAsync(username);

            if (existingUsername is not null)
            {
                _logger.LogWarning("Registration attempt failed: Username {Username} is already taken.", username);
                return AuthErrors.UsernameAlreadyExists;
            }

            var existingEmail = await _manager.FindByEmailAsync(email);

            if (existingEmail is not null)
            {
                _logger.LogWarning("Registration attempt failed: Email {Email} is already registered.", email);
                return AuthErrors.InvalidEmail;
            }

            var newUser = AppUser.Create(username, email);

            if (newUser.IsFailure)
                return newUser.Errors;

            var createResult = await _manager.CreateAsync(newUser.Value, password);

            if (!createResult.Succeeded)
                return createResult.Errors.Select(e => Error.Failure(e.Code, e.Description)).ToList();

            var addToRoleResult = await _manager.AddToRoleAsync(newUser.Value, nameof(Role.User));

            if (!addToRoleResult.Succeeded)
            {
                _logger.LogError("Failed to assign role to user with email {Email}. Rolling back user creation.", email);
                await _manager.DeleteAsync(newUser.Value); 
                return addToRoleResult.Errors.Select(e => Error.Failure(e.Code, e.Description)).ToList();
            }

            _logger.LogInformation("User with email {Email} successfully registered.", email);

            return new RegisterUserDto(username, email);
        }

        private async Task<AppUserDto> GetUserInfoAsync(AppUser user)
        {

            var roles = await _manager.GetRolesAsync(user);

            var claims = await _manager.GetClaimsAsync(user);


            return new AppUserDto(user.GetIdString(), user.Email!, roles, claims);

        }

    }
}
